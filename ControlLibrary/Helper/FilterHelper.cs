using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using ControlLibrary.Datagrid;
using ControlLibrary.Model;
using DynamicQueryLibrary;
using System.Collections;
using ControlLibrary.Helper;
using Common;
using ControlLibrary.DatagridColumns;
using System.Windows.Controls;
using ControlLibrary.Model.Visitor;
using System.Collections.Specialized;

namespace ControlLibrary.Helpers
{
    public class FilterHelper
    {
        public FilterHelper(ExtendedDatagridControl extendedDatagrid)
        {
            CurrentExtendedDatagrid = extendedDatagrid;
            FilteredColumnInfo = new ObservableCollection<ColumnInfo>();
            DatagridRows = new Dictionary<ExtendedTemplateColumn, List<DataGridCell>>();
        }

        public Dictionary<ExtendedTemplateColumn,List<DataGridCell>> DatagridRows { get; set; }

        public ExtendedDatagridControl CurrentExtendedDatagrid { get; set; }

        public ObservableCollection<ColumnInfo> FilteredColumnInfo { get; set; }

        public ColumnInfo CurrentColumnInfo { get; set; }

        private List<object> _filteredItemSourceCollection;

        private IDataType GetDataType()
        {
            var dataType = string.Empty;
            TypeSwitch.Do(
                CurrentColumnInfo.Column.FieldNameType,
                TypeSwitch.Case<string>(() => dataType = "StringDataType"),
                TypeSwitch.Case<DateTime>(() => dataType = "DatetimeDataType"),
                TypeSwitch.Case<DateTime?>(() => dataType = "DatetimeDataType"),
                TypeSwitch.Default(() => dataType = "DefaultType")
            );
            return DataTypeFactory.GetInstance().CreateInstance(dataType);
        }

        private void InsertRowValue(object rowValue, string displayRowValue, IDataType dataType)
        {
            var currentColumnRowValue = CurrentColumnInfo.RowValues.FirstOrDefault(x => x.DisplayRowValue == displayRowValue);

            if (currentColumnRowValue == null)
            {
                currentColumnRowValue = new RowData(displayRowValue, dataType);
                CurrentColumnInfo.RowValues.Add(currentColumnRowValue);
            }
            currentColumnRowValue.AddRowValue(rowValue);
        }

        public ObservableCollection<CheckedListItem> GetDistinctColumnValues()
        {
            if (CollectionViewSource.GetDefaultView(CurrentExtendedDatagrid.ItemsSource) == null) return null;
          
            var isColumnFiltered = IsCurrenColumnFiltered();

            var distinctRowValues = new List<object>();

            var dataType = GetDataType();

            if (CurrentColumnInfo.Column.IsDefaultBinding)
            {
               distinctRowValues.AddRange(CurrentExtendedDatagrid.ItemsSource.SelectExpression<object>(
                    CurrentColumnInfo.Column.FieldName.GetSelectLamdaExpressionPredicate(CurrentColumnInfo.Column.IsNullableBinding), null).Distinct().ToList());

                foreach (var rowValue in distinctRowValues)
                {
                    InsertRowValue(rowValue, dataType.GetDisplayValue(rowValue), dataType);
                }
            }
            else
            {
                var datagridCells = DatagridRows.First(c => c.Key == CurrentColumnInfo.Column).Value;
                foreach (var row in datagridCells)
                {
                    var textBlock = FindDataGridControl.GetTextBlock(row);
                    var rowValue = textBlock.DataContext.GetPropertyValue(CurrentColumnInfo.Column.FieldName);

                    if (distinctRowValues.Contains(rowValue)) continue;

                    distinctRowValues.Add(rowValue);
                    InsertRowValue(rowValue, dataType.GetDisplayValue(textBlock.Text,false), dataType);
                }
            }

            CurrentColumnInfo.RowValues.ToList().ForEach(x => x.ListItem.IsSelected = !isColumnFiltered);
           
            if (isColumnFiltered)
            {
                var existingFilteredRowValues = FilteredColumnInfo.First(x => x.Column.FieldName == CurrentColumnInfo.Column.FieldName).RowValues;

                foreach (var existingFilteredRowValue in existingFilteredRowValues)
                {
                    var listItem = CurrentColumnInfo.RowValues.FirstOrDefault(x => x.DisplayRowValue == existingFilteredRowValue.DisplayRowValue);
                    if (listItem != null)
                        listItem.ListItem.IsSelected = true;
                }
            }

            var currentFilteredRowValues = new ObservableCollection<CheckedListItem>();
            foreach (var listItem in CurrentColumnInfo.RowValues.Select(x => x.ListItem))
            {
                currentFilteredRowValues.Add(listItem);
            }
            return currentFilteredRowValues;
        }

        public bool IsCurrenColumnFiltered()
        {
            return FilteredColumnInfo.Any(x => x.Column.FieldName == CurrentColumnInfo.Column.FieldName);
        }

        public void ClearColumnFilter(string fieldName)
        {
            var existingFilteredColumnDefinition = FilteredColumnInfo.FirstOrDefault(x => x.Column.FieldName == fieldName);

            if (existingFilteredColumnDefinition != null)
            {
                FilteredColumnInfo.Remove(existingFilteredColumnDefinition);
            }
        }

        public void SetColumnFilter()
        {
            var existingFilteredColumnDefinition = FilteredColumnInfo.FirstOrDefault(x => x.Column.FieldName == CurrentColumnInfo.Column.FieldName);

            if (CurrentColumnInfo.RowValues.Select(x => x.ListItem).All(x => x.IsSelected))
            {
                FilteredColumnInfo.Remove(existingFilteredColumnDefinition);
            }
            else
            {
                var currentFilteredRowValues = new ObservableCollection<RowData>();
                foreach (var rowValue in CurrentColumnInfo.RowValues.Where(x => x.ListItem.IsSelected))
                {
                    currentFilteredRowValues.Add(rowValue);
                }
                
                if (existingFilteredColumnDefinition != null)
                    existingFilteredColumnDefinition.RowValues = currentFilteredRowValues;
                else
                    FilteredColumnInfo.Add(new ColumnInfo()
                    {
                        Column = CurrentColumnInfo.Column,
                        RowValues = currentFilteredRowValues
                    }) ;
            }
            ApplyFilter();
        }

        public void ApplyFilter()
        {
            var view = CollectionViewSource.GetDefaultView(CurrentExtendedDatagrid.ItemsSource);
            string filterExpression = string.Empty;
            var data = new List<object>();
            int count = 0;

            foreach (var columnDefinition in FilteredColumnInfo)
            {
                var tempfilterExpression = string.Empty;
                foreach (var rowValue in columnDefinition.RowValues)
                {
                    foreach (var rowData in rowValue.RowValue)
                    {
                        if (rowData == null)
                        {
                            tempfilterExpression += columnDefinition.Column.FieldName.GetNullCheckLamdaExpressionPredicate(columnDefinition.Column.IsNullableBinding) + " OR ";
                        }
                        else
                        {
                            var notNullCheckLamdaExpressionPredicate = columnDefinition.Column.FieldName.GetNotNullCheckLamdaExpressionPredicate(columnDefinition.Column.IsNullableBinding);

                            var condition = columnDefinition.Column.FieldName + rowValue.GetEqualSyntaxForLambdaExpression() + "(@" + count.ToString() + ")";
                            data.Add(rowData);
                            count++;

                            if (!string.IsNullOrWhiteSpace(notNullCheckLamdaExpressionPredicate))
                                condition = "(" + notNullCheckLamdaExpressionPredicate + " AND " + condition + ")";

                            tempfilterExpression += condition + " OR ";
                        }
                    }
                }
                var orLastIndex = tempfilterExpression.LastIndexOf("OR");
                if (orLastIndex != -1)
                {
                    tempfilterExpression = tempfilterExpression.Substring(0, orLastIndex);
                }

                if (!string.IsNullOrWhiteSpace(tempfilterExpression))
                    filterExpression += "(" + tempfilterExpression + ") AND ";
            }
            var andLastIndex = filterExpression.LastIndexOf("AND");
            if (andLastIndex != -1)
            {
                filterExpression = filterExpression.Substring(0, andLastIndex).Trim();
            }

            var collection = (CurrentExtendedDatagrid.ItemsSource) as IEnumerable;

            _filteredItemSourceCollection = (!string.IsNullOrWhiteSpace(filterExpression))
                ? collection.WhereExpression<object>(filterExpression, data.ToList<object>())
                 : null;

            view.Filter = new Predicate<object>(itemPassesFilter);
        }

        private bool itemPassesFilter(object item)
        {
            if (_filteredItemSourceCollection == null) return true;
            return _filteredItemSourceCollection.Contains(item);
        }
    }
}

using System.Windows.Controls;
using ControlLibrary.Helpers;
using System.Windows;
using ControlLibrary.Helper;
using ControlLibrary.DatagridColumns;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using Common;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace ControlLibrary.Datagrid
{
    public class ExtendedDatagridControl : DataGrid
    {
        public FilterHelper FilterHelper { get; private set; }

        public ObservableCollection<object> itemSource1;
        public ExtendedDatagridControl()
        {
            Loaded += OnExtendedDatagridControlLoaded;
        }
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    GetRowDataGridCell(e.NewStartingIndex);
                    FilterHelper.ApplyFilter();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void OnExtendedDatagridControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            FilterHelper = new FilterHelper(this);
            EvaluateDatagridColumn();
            EvaluateDatagridRows();
        }

        private void GetRowDataGridCell(int rowNo)
        {
            DataGridRow rowContainer = FindDataGridControl.GetRow(this, rowNo);
            foreach (ExtendedTemplateColumn col in this.Columns)
            {
                var datagridCell = FindDataGridControl.GetCell(this, rowNo, col.DisplayIndex);
                if (!FilterHelper.DatagridRows.ContainsKey(col))
                {
                    var datagridCollection = new List<DataGridCell>();
                    datagridCollection.Add(datagridCell);
                    FilterHelper.DatagridRows.Add(col, datagridCollection);
                }
                else
                {
                    FilterHelper.DatagridRows[col].Add(datagridCell);
                }
            }
        }
        private void EvaluateDatagridRows()
        {
            for (var i = 0; i < this.Items.Count; i++)
            {
                GetRowDataGridCell(i);
            }
        }

        private void EvaluateDatagridColumn()
        {
            DataGridRow rowContainer = FindDataGridControl.GetRow(this, 0);
            Type itemSourceType = (this.ItemsSource as IEnumerable).AsQueryable().ElementType;

            foreach (ExtendedTemplateColumn col in this.Columns)
            {
                col.FieldNameType = ReflectionHelper.GetPropertyType(col.FieldName, itemSourceType);
                col.IsNullableBinding = (col.FieldNameType == typeof(string)) || (Nullable.GetUnderlyingType(col.FieldNameType) != null);
                if (col.IsDefaultBinding) continue;
                var datagridCell = FindDataGridControl.GetCell(this, 0, col.DisplayIndex);
                var framework = FindControl.FindChild<FrameworkElement>((ContentPresenter)datagridCell.Content, string.Empty);

                var textBlock = framework as TextBlock;
                col.IsDefaultBinding = textBlock == null;
            }
        }

        public void ClearColumnFilter(string fieldName)
        {
            FilterHelper.ClearColumnFilter(fieldName);
        }

        public void Refresh()
        {
            FilterHelper.ApplyFilter();
        }
    }
}

using Common;
using ControlLibrary.Model.Visitor;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace ControlLibrary.Model
{
    public class RowData : BindableBase
    {
        public List<object> RowValue { get; set; }

        public string DisplayRowValue { get; set; }

        public CheckedListItem ListItem { get; set; }

        private readonly IDataType _dataType;

        public RowData(string displayRowValue, IDataType datatype)
        {
            RowValue = new List<object>();
            DisplayRowValue = displayRowValue;
            SetCheckedListItem();
            _dataType = datatype;
        }

        public void AddRowValue(object rowValue)
        {
            if (!RowValue.Contains(rowValue))
                RowValue.Add(rowValue);
        }

        private void SetCheckedListItem()
        {
            ListItem = (new CheckedListItem()
            {
                Name = DisplayRowValue,
                IsSelected = true,
            });
        }

        public string GetEqualSyntaxForLambdaExpression()
        {
            return _dataType.GetEqualSyntaxForLambdaExpression();
        }
    }
}

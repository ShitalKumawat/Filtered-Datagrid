using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ControlLibrary.DatagridColumns
{
    public class ExtendedTemplateColumn : DataGridTemplateColumn
    {
        public static readonly DependencyProperty IsDefaultBindingProperty = DependencyProperty.Register(
       "IsDefaultBinding", typeof(bool), typeof(ExtendedTemplateColumn), new PropertyMetadata(false));

        public bool IsDefaultBinding
        {
            get { return (bool)GetValue(IsDefaultBindingProperty); }
            set { SetValue(IsDefaultBindingProperty, value); }
        }

        public static readonly DependencyProperty FieldNameTypeProperty = DependencyProperty.Register(
       "FieldNameType", typeof(Type), typeof(ExtendedTemplateColumn), new PropertyMetadata(null));

        public Type FieldNameType
        {
            get { return (Type)GetValue(FieldNameTypeProperty); }
            set { SetValue(FieldNameTypeProperty, value); }
        }

        public static readonly DependencyProperty IsNullableBindingProperty = DependencyProperty.Register(
        "IsNullableBinding", typeof(bool), typeof(ExtendedTemplateColumn), new PropertyMetadata(false));

        public bool IsNullableBinding
        {
            get { return (bool)GetValue(IsNullableBindingProperty); }
            set { SetValue(IsNullableBindingProperty, value); }
        }

        public static readonly DependencyProperty FieldNameProperty = DependencyProperty.Register(
         "FieldName", typeof(string), typeof(ExtendedTemplateColumn), new PropertyMetadata(string.Empty));

        public string FieldName
        {
            get { return (string)GetValue(FieldNameProperty); }
            set { SetValue(FieldNameProperty, value); }
        }

        public static readonly DependencyProperty AllowFilterProperty = DependencyProperty.Register(
           "AllowFilter", typeof(bool), typeof(ExtendedTemplateColumn), new PropertyMetadata(true));

        public static readonly DependencyProperty IsFilteredProperty = DependencyProperty.Register(
            "IsFiltered", typeof(bool), typeof(ExtendedTemplateColumn), new PropertyMetadata(true));

        public bool AllowFilter
        {
            get { return (bool)GetValue(AllowFilterProperty); }
            set { SetValue(AllowFilterProperty, value); }
        }

        public bool IsFiltered
        {
            get { return (bool)GetValue(IsFilteredProperty); }
            set { SetValue(IsFilteredProperty, value); }
        }


        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var col = cell.Column as ExtendedTemplateColumn;

            if (col.CellTemplate != null)
                return base.GenerateElement(cell, dataItem);

            var binding = new Binding(this.FieldName) { Source = dataItem };
            var content = new ContentControl { ContentTemplate = (DataTemplate)cell.FindResource("CustomTemplate") };
            content.SetBinding(ContentControl.ContentProperty, binding);
            col.IsDefaultBinding = true;
            return content;
        }
    }
}

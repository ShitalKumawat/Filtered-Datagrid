using ControlLibrary.Datagrid;
using System.Windows;
using System.Windows.Controls;

namespace ControlLibrary.DatagridColumns
{
    public partial class ColumnChooser : Window
    {
        public ExtendedDatagridControl datagrid { get; set; }
        public ColumnChooser()
        {
            InitializeComponent();
            var filterView = ColumnValuesList;
            filterView.btnClear.Click += onBtnClick;
            filterView.btnOk.Click += onBtnClick;
            filterView.btnCancel.Click += onBtnClick;
        }

        private void onBtnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            switch (button.Name.ToString())
            {
                case "btnOk":
                    foreach (var item in ColumnValuesList.ListItems)
                    {
                        foreach (ExtendedTemplateColumn col in datagrid.Columns)
                        {
                            if (col.Header.ToString() == item.Name)
                            {
                                col.Visibility = item.IsSelected ? Visibility.Visible : Visibility.Collapsed;
                                if (!item.IsSelected)
                                {
                                    datagrid.ClearColumnFilter(col.FieldName);
                                    col.IsFiltered = false;
                                }
                                break;
                            }
                        }
                    }
                    datagrid.Refresh();
                    break;
                case "btnCancel":
                    break;
                case "btnClear":
                    foreach (ExtendedTemplateColumn col in datagrid.Columns)
                    {
                        if (col.Visibility == Visibility.Collapsed)
                        {
                            col.IsFiltered = false;
                            col.Visibility = Visibility.Visible;
                        }
                    }
                    break;
            }
            this.Close();
        }
    }
}

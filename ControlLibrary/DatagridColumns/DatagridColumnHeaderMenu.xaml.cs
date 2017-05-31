using ControlLibrary.Datagrid;
using ControlLibrary.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlLibrary.DatagridColumns
{
    public partial class DatagridColumnHeaderMenu : UserControl
    {
        public DatagridColumnHeaderMenu()
        {
            InitializeComponent();
        }
        public T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            var parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }

        private void OnColumnChooserClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var contextMenu = FindParent<ContextMenu>(button) ?? (ContextMenu)
                       ((MenuItem)(((DatagridColumnHeaderMenu)(((StackPanel)(button.Parent)).Parent)).Parent)).Parent;
           var datagrid = contextMenu.PlacementTarget != null ?
                    FindParent<ExtendedDatagridControl>(contextMenu.PlacementTarget)
                    : contextMenu.Tag as ExtendedDatagridControl;
          
            var columns = new ObservableCollection<CheckedListItem>();
            foreach (var col in datagrid.Columns)
            {
                columns.Add(new CheckedListItem() { Name = col.Header.ToString(), IsSelected = col.Visibility == Visibility.Visible });
            }

            var columnChooser = new ColumnChooser() { datagrid = datagrid };
            columnChooser.ColumnValuesList.ListItems = columns;
            columnChooser.ShowDialog();
        }
    }
}

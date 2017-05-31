using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ControlLibrary.DatagridColumns;
using ControlLibrary.Helper;
using ControlLibrary.ListViewControl;

namespace ControlLibrary.Datagrid
{
    public partial class ExtendedDatagridEventHandler
    {
        private ExtendedTemplateColumn _currentColumn;
        private void AutoFilterMouseDown(object sender, MouseButtonEventArgs e)
        {
            var columnHeader = FindControl.FindParent<DataGridColumnHeader>((ContentControl)sender);
            _currentColumn = columnHeader.Column as ExtendedTemplateColumn;
            if (_currentColumn == null) return;

            var popup = FindControl.FindChild<Popup>(columnHeader, "popupColumnFilter");
            if (popup == null) return;
            popup.IsOpen = true;

            var popupSize = new Size(popup.ActualWidth, popup.ActualHeight);
            var position = new Point { X = columnHeader.ActualWidth - 19, Y = columnHeader.ActualHeight };
            popup.PlacementRectangle = new Rect(position, popupSize);

            var mainGrid = FindControl.FindParent<ExtendedDatagridControl>(popup);
            mainGrid.FilterHelper.CurrentColumnInfo = new Model.ColumnInfo(){ Column=_currentColumn};

            var filterViewControl = FindControl.FindChild<FilterView>(popup.Child, "ColumnValuesList");
            filterViewControl.ListItems = mainGrid.FilterHelper.GetDistinctColumnValues();
            InitializeFilterView(filterViewControl);
        }

        private void InitializeFilterView(FilterView filterview)
        {
            var okButton = FindControl.FindChild<Button>(filterview, "btnOk");
            okButton.Click += OnPopupButtonClick;

            var cancelButton = FindControl.FindChild<Button>(filterview, "btnCancel");
            cancelButton.Click += OnPopupButtonClick;

            var clearButton = FindControl.FindChild<Button>(filterview, "btnClear");
            clearButton.Click += OnPopupButtonClick;

            filterview.LostFocus -= PopLostFocus;
            filterview.LostFocus += PopLostFocus;
            filterview.Focus();
            filterview.UpdateLayout();
        }

        private void OnPopupButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;
            var popup = GetPopupParentFromFilterView(button);
            if (popup == null) return;

            var mainGrid = FindControl.FindParent<ExtendedDatagridControl>(popup);
            var currentColumn = _currentColumn as ExtendedTemplateColumn;

            switch (button.Name)
            {
                case "btnOk":
                    mainGrid.FilterHelper.SetColumnFilter();
                    if(currentColumn!=null)
                        currentColumn.IsFiltered = mainGrid.FilterHelper.IsCurrenColumnFiltered();
                    break;
                case "btnCancel":
                    break;
                case "btnClear":
                    mainGrid.FilterHelper.ClearColumnFilter(mainGrid.FilterHelper.CurrentColumnInfo.Column.FieldName);
                    mainGrid.FilterHelper.ApplyFilter();
                     if (currentColumn != null)
                        currentColumn.IsFiltered = false;
                    break;
            }
            popup.IsOpen = false;
        }

        private Popup GetPopupParentFromFilterView(object sender)
        {
            var button = sender as Button;
            if (button == null) return null;

            var filterView = FindControl.FindParent<FilterView>((ContentControl)sender);
            if (filterView == null) return null;

            var stackPanel = FindControl.FindParent<StackPanel>(filterView);
            if (stackPanel == null) return null;

            var popup = stackPanel.Parent as Popup;
            return popup;
            
        }

        private void PopLostFocus(object sender, RoutedEventArgs e)
        {
            var stackPanel = sender as StackPanel == null
                ? FindControl.FindParent<StackPanel>((FrameworkElement)sender)
                : (StackPanel)sender;
            if (stackPanel == null) return;
            var popup =stackPanel.Parent as Popup;
            if (popup == null) return;
            if (popup.IsMouseOver) return;
            var currentFocueedElement = FocusManager.GetFocusedElement(popup);
            if (currentFocueedElement == null)
                popup.IsOpen = false;
        }


     
    }
}

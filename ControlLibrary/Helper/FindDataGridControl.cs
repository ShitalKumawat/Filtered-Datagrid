using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ControlLibrary.Helper
{
    public class FindDataGridControl
    {
        public static TextBlock GetTextBlock(DataGridCell datagridCell)
        {
            var framework = FindControl.FindChild<FrameworkElement>((ContentPresenter)datagridCell.Content, string.Empty);
            var textBlock = framework as TextBlock;
            return textBlock;
        }

        public static TextBlock GetTextBlock(DataGridRow rowContainer, int column)
        {
            DataGridCellsPresenter presenter = FindControl.FindChild<DataGridCellsPresenter>(rowContainer, string.Empty);
            DataGridCell datagridCell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
            return GetTextBlock(datagridCell);
        }

        public static DataGridCell GetCell(DataGrid datagrid, int row, int column)
        {
            DataGridRow rowContainer = GetRow(datagrid,row);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindControl.FindChild<DataGridCellsPresenter>(rowContainer, string.Empty);
                if (presenter == null)
                {
                    datagrid.ScrollIntoView(rowContainer, datagrid.Columns[column]);
                    presenter = FindControl.FindChild<DataGridCellsPresenter>(rowContainer,string.Empty);
                }
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public static DataGridRow GetRow(DataGrid datagrid ,int index)
        {
            DataGridRow row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                datagrid.UpdateLayout();
                datagrid.ScrollIntoView(datagrid.Items[index]);
                row = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
    }
}

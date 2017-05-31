using ControlLibrary.DatagridColumns;
using System.Collections.ObjectModel;


namespace ControlLibrary.Model
{
    public class ColumnInfo
    {
        public ExtendedTemplateColumn Column { get; set; }
        public ObservableCollection<RowData> RowValues { get; set; }
        public ColumnInfo()
        {
            RowValues = new ObservableCollection<RowData>();
        }
    }
}

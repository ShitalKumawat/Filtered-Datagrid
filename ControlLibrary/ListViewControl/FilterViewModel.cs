using Common;
using ControlLibrary.Model;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace ControlLibrary.ListViewControl
{
    public class FilterViewModel : BindableBase
    {
        private ICollectionView filteredListItemView;

        private ObservableCollection<CheckedListItem> _listItems;
        public ObservableCollection<CheckedListItem> ListItems
        {
            get { return _listItems; }
            set
            {
                _listItems = value;
                SetProperty(ref this._listItems, value);
                RefreshListItemSource();
                filteredListItemView = CollectionViewSource.GetDefaultView(ListItems);
                filteredListItemView.Filter = o => ApplyFilter((CheckedListItem)o);
                IsFilterApplied= !(ListItems != null && ListItems.All(x => x.IsSelected));
                RaisePropertyChanged("ListItems");
            }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                SetProperty(ref this._filter, value);
                filteredListItemView.Refresh();
            }
        }

        private bool _isFilterApplied;
        public bool IsFilterApplied
        {
            get { return _isFilterApplied; }
            set
            {
                _isFilterApplied = value;
                SetProperty(ref this._isFilterApplied, value);
                RaisePropertyChanged("IsFilterApplied");
            }
        }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??
                       (_okCommand =
                           new RelayCommand(p => { }, q => { return ListItems != null && ListItems.Any(x => x.IsSelected); }));
            }
        }

        private bool ApplyFilter(CheckedListItem item)
        {
            if (string.IsNullOrEmpty(Filter)) return true;
            return ((item as CheckedListItem).Name.ToUpper().Contains(Filter.ToUpper()));
        }

        private void RefreshListItemSource()
        {
            if (ListItems == null) return;
            var selectAllCheckItemList = new SelectAllCheckListItem() { Name = "Select All" };
            foreach (var item in ListItems)
            {
                selectAllCheckItemList.AddCheckListItem(item);
            }

            selectAllCheckItemList.IsSelected = ListItems.Aggregate(true, (current, item) => current && item.IsSelected);
            ListItems.Insert(0, selectAllCheckItemList);
        }
    }
}

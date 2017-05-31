using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Model
{
    public class SelectAllCheckListItem : CheckedListItem
    {
        private bool _isSelectAllSelected;
        private readonly List<CheckedListItem> _items;

        public SelectAllCheckListItem()
        {
            _items = new List<CheckedListItem>();
        }

        public override bool IsSelected
        {
            get
            {
                return base.IsSelected;
            }

            set
            {
                if (_isSelectAllSelected)
                {
                    foreach (var item in this._items)
                    {
                        item.PropertyChanged -= this.OnItemPropertyChanged;
                        item.IsSelected = value;
                        item.PropertyChanged += this.OnItemPropertyChanged;
                    }
                }

                base.IsSelected = value;
                _isSelectAllSelected = true;
            }
        }

        public void AddCheckListItem(CheckedListItem item)
        {
            item.PropertyChanged += OnItemPropertyChanged;
            _items.Add(item);
        }

        private void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSelected"))
            {
                _isSelectAllSelected = false;
                var selectAllValue = this._items.Aggregate(true, (current, item) => current && item.IsSelected);
                IsSelected = selectAllValue;
            }
        }
    }
}

using ControlLibrary.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ControlLibrary.ListViewControl
{
    public partial class FilterView
    {
        public FilterView()
        {
            InitializeComponent();
            ViewModel = new FilterViewModel();

        }

        private FilterViewModel _viewModel;

        public FilterViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel == value) return;
                if (_viewModel != null)
                    _viewModel.PropertyChanged -= ViewModelPropertyChanged;

                _viewModel = value;
                DataContext = _viewModel;
                if (_viewModel != null)
                    _viewModel.PropertyChanged += ViewModelPropertyChanged;
            }
        }


        public static readonly DependencyProperty ListItemsProperty =
            DependencyProperty.Register("ListItems",
                typeof(ObservableCollection<CheckedListItem>), typeof(FilterView),
              new FrameworkPropertyMetadata(default(ObservableCollection<CheckedListItem>),
                   (d, e) =>
                       ((FilterView)d).ViewModel.ListItems =
                           ((FilterView)d).ListItems)
              {
                  BindsTwoWayByDefault = true,
                  DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
              });

        public ObservableCollection<CheckedListItem> ListItems
        {
            get
            {
                return (ObservableCollection<CheckedListItem>)GetValue(ListItemsProperty);
            }
            set
            {
                SetValue(ListItemsProperty, value);
            }
        }

       
        #region Dependency Event

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Dispatcher.CheckAccess())
                CopyViewModelPropertiesToDependencyProperties();
            else
                Dispatcher.Invoke(() => CopyViewModelPropertiesToDependencyProperties());
        }

        private void CopyViewModelPropertiesToDependencyProperties()
        {
            SetCurrentValue(ListItemsProperty, ViewModel.ListItems);
        }

        #endregion
    }
}

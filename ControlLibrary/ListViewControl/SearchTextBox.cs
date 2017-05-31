using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ControlLibrary.ListViewControl
{
    public class SearchTextBox : TextBox
    {
        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTextBox),
                new FrameworkPropertyMetadata(typeof(SearchTextBox)));
        }

        private ButtonBase _clearButtonHost;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clearButtonHost = GetTemplateChild("PART_ClearButtonHost") as ButtonBase;

            if (_clearButtonHost != null)
            {
                _clearButtonHost.Click += OnClearButtonClick;
            }
        }

        void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            this.Text = string.Empty;
        }

    }
}
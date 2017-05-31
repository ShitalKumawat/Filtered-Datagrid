
namespace TestApplication
{

    public partial class TestWindow
    {
        private MainWindowViewModel test;
        public TestWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}

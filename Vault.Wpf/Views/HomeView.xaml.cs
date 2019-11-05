using MvvmCross.Platforms.Wpf.Views;

namespace Vault.Wpf.Views
{
    public partial class HomeView : MvxWpfView
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void MvxWpfView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            PswdBx.Focus();
        }
    }
}
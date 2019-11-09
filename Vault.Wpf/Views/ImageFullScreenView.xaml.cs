using MvvmCross.Platforms.Wpf.Views;
using Vault.Wpf.UI;

namespace Vault.Wpf.Views
{
    [ModalPresentation(OwnerWindowType = typeof(MainWindow))]
    public partial class ImageFullScreenView : MvxWindow
    {
        public ImageFullScreenView()
        {
            InitializeComponent();
        }

        private void MvxWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                Close();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}

using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Input;
using Vault.Core.ViewModels;
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

        private void MvxWindow_KeyDown(object sender, KeyEventArgs e)
        {
            ImageFullScreenViewModel model = (ImageFullScreenViewModel)ViewModel;

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Left:
                    model.CycleImageLeftCommand.Execute();
                    break;
                case Key.Right:
                    model.CycleImageRightCommand.Execute();
                    break;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}

using MvvmCross.Platforms.Wpf.Views;
using MvvmCrossExtensions.Wpf.Presenters.MasterDetail;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
using Vault.Core.ViewModels;

namespace Vault.Wpf.Views
{
    [DetailPresentation]
    public partial class ImageDisplayView : MvxWpfView
    {
        public ImageDisplayView()
        {
            InitializeComponent();
        }

        private void PopImage_Opened(object sender, System.EventArgs e)
        {
            Storyboard sb = (Storyboard)PopImage.FindResource("PopupOpenedStoryboard");
            sb.Begin();
        }

        private void PopImage_Closed(object sender, System.EventArgs e)
        {
            GrdImageCycleLeft.Opacity = 1;
            GrdImageCycleRight.Opacity = 1;
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape && PopImage.IsOpen)
            {
                ImageDisplayViewModel vm = (ImageDisplayViewModel)ViewModel;
                vm.CloseDialogCommand.Execute();
            }
        }
    }
}

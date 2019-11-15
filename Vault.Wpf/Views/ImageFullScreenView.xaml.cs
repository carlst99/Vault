using MvvmCross.Platforms.Wpf.Views;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Vault.Core.ViewModels;
using Vault.Wpf.UI;

namespace Vault.Wpf.Views
{
    [ModalPresentation(OwnerWindowType = typeof(MainWindow))]
    public partial class ImageFullScreenView : MvxWindow
    {
        private bool _cursorVisible;
        private Point _mousePosition;

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

        private void MvxWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_cursorVisible)
                return;

            Cursor = Cursors.Arrow;
            _mousePosition = Mouse.GetPosition(this);
            _cursorVisible = true;
            RunCursorHideTask();
        }

        private void RunCursorHideTask()
        {
            Task.Delay(1000).ContinueWith((_) => Dispatcher.Invoke(() =>
            {
                Point currentMousePos = Mouse.GetPosition(this);
                if (currentMousePos != _mousePosition)
                {
                    _mousePosition = currentMousePos;
                    RunCursorHideTask();
                } else
                {
                    Cursor = Cursors.None;
                    _cursorVisible = false;
                }
            }));
        }

        private void ImgMain_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var st = (ScaleTransform)((TransformGroup)ImgMain.RenderTransform).Children.First(sc => sc is ScaleTransform);
            double zoom = e.Delta > 0 ? .2 : -.2;

            // Don't shrink
            // Only need to check X scale as we are scaling both X and Y evenly
            if (st.ScaleX <= 1 && zoom < 0)
                return;

            // Likewise, don't zoom in too much
            if (st.ScaleX >= 5 && zoom > 0)
                return;

            st.ScaleX += zoom;
            st.ScaleY += zoom;
        }

        private void ImgMain_StretchSourceUpdated(object sender, RoutedEventArgs e)
        {
            var st = (ScaleTransform)((TransformGroup)ImgMain.RenderTransform).Children.First(sc => sc is ScaleTransform);
            st.ScaleX = 1;
            st.ScaleY = 1;
        }
    }
}

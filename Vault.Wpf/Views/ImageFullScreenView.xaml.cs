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
        private Point _lastMousePosition;
        private Point _panStartPosition;
        private Point _panOrigin;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MvxWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (_cursorVisible)
                return;

            Cursor = Cursors.Arrow;
            _lastMousePosition = Mouse.GetPosition(this);
            _cursorVisible = true;
            RunCursorHideTask();
        }

        private void RunCursorHideTask()
        {
            Task.Delay(1000).ContinueWith((_) => Dispatcher.Invoke(() =>
            {
                Point currentMousePos = Mouse.GetPosition(this);
                if (currentMousePos != _lastMousePosition)
                {
                    _lastMousePosition = currentMousePos;
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

            // Centre on cursor location
            Point mouseLoc = Mouse.GetPosition(ImgMain);
            ImgMain.RenderTransformOrigin = new Point(mouseLoc.X / ImgMain.ActualWidth, mouseLoc.Y / ImgMain.ActualHeight);

            // Don't shrink
            // Only need to check X scale as we are scaling both X and Y evenly
            if (st.ScaleX == 1 && zoom < 0)
            {
                return;
            } else if (st.ScaleX < 1 && zoom < 0)
            {
                st.ScaleX = 1;
                return;
            }

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

            var tt = (TranslateTransform)((TransformGroup)ImgMain.RenderTransform).Children.First(tr => tr is TranslateTransform);
            tt.X = 0;
            tt.Y = 0;
        }

        private void ImgMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tt = (TranslateTransform)((TransformGroup)ImgMain.RenderTransform).Children.First(tr => tr is TranslateTransform);
            _panStartPosition = e.GetPosition(GrdImageContainer);
            _panOrigin = new Point(tt.X, tt.Y);
            ImgMain.CaptureMouse();
        }

        private void ImgMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (ImgMain.IsMouseCaptured)
            {
                var tt = (TranslateTransform)((TransformGroup)ImgMain.RenderTransform).Children.First(tr => tr is TranslateTransform);
                Vector v = _panStartPosition - e.GetPosition(GrdImageContainer);
                tt.X = _panOrigin.X - v.X;
                tt.Y = _panOrigin.Y - v.Y;
            }
        }

        private void ImgMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImgMain.ReleaseMouseCapture();
        }
    }
}

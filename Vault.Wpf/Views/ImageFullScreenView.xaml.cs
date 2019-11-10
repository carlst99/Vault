using MvvmCross.Platforms.Wpf.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
    }
}

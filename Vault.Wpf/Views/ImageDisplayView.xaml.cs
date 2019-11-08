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
    }
}

using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCrossExtensions.Wpf.Presenters.MasterDetail;
using System.Windows.Controls;

namespace Vault.Wpf
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new MasterDetailPresenter(root);
        }
    }
}

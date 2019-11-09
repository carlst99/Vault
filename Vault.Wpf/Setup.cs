using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;
using System.Windows.Controls;
using Vault.Wpf.UI;

namespace Vault.Wpf
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new ModalPresenter(root);
        }
    }
}

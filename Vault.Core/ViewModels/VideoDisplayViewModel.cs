using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;

namespace Vault.Core.ViewModels
{
    public class VideoDisplayViewModel : Base.ViewModelBase
    {
        public VideoDisplayViewModel(IMvxNavigationService navigationService, IMvxMessenger messenger)
            : base(navigationService, messenger)
        {
        }
    }
}
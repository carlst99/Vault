using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using Realms;
using Vault.Core.Model.DbContext;

namespace Vault.Core.ViewModels.Base
{
    public interface IViewModelBase
    {
        string this[string index] { get; }
        IMvxNavigationService NavigationService { get; }
        Realm RealmInstance { get; }
        Preferences UserPreferences { get; }
        IMvxMessenger Messenger { get; }
    }
}

using MvvmCross.Navigation;
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
    }
}

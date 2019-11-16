using Vault.Core.Resources;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Serilog;
using Realms;
using Vault.Core.Model.DbContext;

namespace Vault.Core.ViewModels.Base
{
    public abstract class ViewModelBase<T> : MvxViewModel<T>, IViewModelBase
    {
        public string this[string index] => AppStrings.ResourceManager.GetString(index);
        public IMvxNavigationService NavigationService { get; }
        public Realm RealmInstance { get; }
        public Preferences UserPreferences { get; }

        protected ViewModelBase(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            Log.Verbose("Navigated to " + GetType().Name);

            RealmInstance = RealmHelpers.GetRealmInstance();
            UserPreferences = RealmHelpers.GetUserPreferences(RealmInstance);
        }
    }

    public abstract class ViewModelBase : MvxViewModel, IViewModelBase
    {
        public string this[string index] => AppStrings.ResourceManager.GetString(index);
        public IMvxNavigationService NavigationService { get; }
        public Realm RealmInstance { get; }
        public Preferences UserPreferences { get; }

        protected ViewModelBase(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            Log.Verbose("Navigated to " + GetType().Name);

            RealmInstance = RealmHelpers.GetRealmInstance();
            UserPreferences = RealmHelpers.GetUserPreferences(RealmInstance);
        }
    }
}

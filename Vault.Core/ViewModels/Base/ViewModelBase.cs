using Vault.Core.Resources;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Serilog;
using Realms;
using Vault.Core.Model.DbContext;
using MvvmCross.Plugin.Messenger;
using Vault.Core.Model.Messages;

namespace Vault.Core.ViewModels.Base
{
    public abstract class ViewModelBase<T> : MvxViewModel<T>, IViewModelBase
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly MvxSubscriptionToken _mediaUpdatedMessageSubToken;
#pragma warning restore IDE0052 // Remove unread private members
        private Realm _realmInstance;
        private Preferences _userPreferences;

        public string this[string index] => AppStrings.ResourceManager.GetString(index);
        public IMvxNavigationService NavigationService { get; }

        public Realm RealmInstance
        {
            get => _realmInstance ?? (_realmInstance = RealmHelpers.GetRealmInstance());
            set => _realmInstance = value;
        }

        public Preferences UserPreferences
        {
            get => _userPreferences ?? (_userPreferences = RealmHelpers.GetUserPreferences());
            set => _userPreferences = value;
        }

        public IMvxMessenger Messenger { get; }

        protected ViewModelBase(IMvxNavigationService navigationService, IMvxMessenger messenger)
        {
            NavigationService = navigationService;
            Log.Verbose("Navigated to " + GetType().Name);

#if DEBUG
            RealmInstance = RealmHelpers.GetRealmInstance();
            UserPreferences = RealmHelpers.GetUserPreferences(RealmInstance);
#endif
            Messenger = messenger;

            _mediaUpdatedMessageSubToken = messenger.SubscribeOnMainThread<MediaUpdatedMessage>(OnMediaUpdatedMessage);
        }

        protected virtual void OnMediaUpdatedMessage(MediaUpdatedMessage media)
        {
        }
    }

    public abstract class ViewModelBase : MvxViewModel, IViewModelBase
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly MvxSubscriptionToken _mediaUpdatedMessageSubToken;
#pragma warning restore IDE0052 // Remove unread private members
        private Realm _realmInstance;
        private Preferences _userPreferences;

        public string this[string index] => AppStrings.ResourceManager.GetString(index);
        public IMvxNavigationService NavigationService { get; }

        public Realm RealmInstance
        {
            get => _realmInstance ?? (_realmInstance = RealmHelpers.GetRealmInstance());
            set => _realmInstance = value;
        }

        public Preferences UserPreferences
        {
            get => _userPreferences ?? (_userPreferences = RealmHelpers.GetUserPreferences());
            set => _userPreferences = value;
        }

        public IMvxMessenger Messenger { get; }

        protected ViewModelBase(IMvxNavigationService navigationService, IMvxMessenger messenger)
        {
            NavigationService = navigationService;
            Log.Verbose("Navigated to " + GetType().Name);

#if DEBUG
            RealmInstance = RealmHelpers.GetRealmInstance();
            UserPreferences = RealmHelpers.GetUserPreferences(RealmInstance);
#endif
            Messenger = messenger;

            _mediaUpdatedMessageSubToken = messenger.SubscribeOnMainThread<MediaUpdatedMessage>(OnMediaUpdatedMessage);
        }

        protected virtual void OnMediaUpdatedMessage(MediaUpdatedMessage media)
        {
        }
    }
}

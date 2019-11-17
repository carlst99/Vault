using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
#if !DEBUG
using MvvmCross.Plugin.Messenger;
using Vault.Core.Model.DbContext;
using Vault.Core.Model.Messages;
using Vault.Core.Services;
#endif

namespace Vault.Core.ViewModels
{
    public class HomeViewModel : Base.ViewModelBase
    {
        #region Fields

#if !DEBUG
        private readonly IPasswordService _passwordService;
#endif

        private string _password;

        #endregion

        #region Commands

        public IMvxCommand OnUnlockCommand => new MvxCommand(OnUnlock);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user password
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

#if DEBUG
        public HomeViewModel(IMvxNavigationService navigationService, IMvxMessenger messenger)
            : base(navigationService, messenger)
        {
        }
#else
        public HomeViewModel(IMvxNavigationService navigationService, IMvxMessenger messenger, IPasswordService passwordService)
            : base(navigationService, messenger)
        {
            _passwordService = passwordService;
        }
#endif

        private async void OnUnlock()
        {
#if DEBUG
            await NavigationService.Navigate<HubViewModel>().ConfigureAwait(false);
#else
            if (await _passwordService.TryVerifyPasswordAsync(Password).ConfigureAwait(false))
            {
                await RealmHelpers.SetEncryptionKeyAsync(Password).ConfigureAwait(false);
                EncryptorAssistant.SetEncryptorPassword(Password);

                RealmInstance = RealmHelpers.GetRealmInstance();
                UserPreferences = RealmHelpers.GetUserPreferences(RealmInstance);
                await NavigationService.Navigate<HubViewModel>().ConfigureAwait(false);
            }
            else
            {
                Messenger.Publish(
                    new DialogMessage(
                        this,
                        "Incorrect Password!", "That's the wrong password. Please check the Caps Lock status of your computer and try again",
                        DialogMessage.DialogMessageType.Error));
                Password = string.Empty;
            }
#endif
        }
    }
}

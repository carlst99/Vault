using MvvmCross.Commands;
using MvvmCross.Navigation;
#if RELEASE
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

#if RELEASE
        private readonly IPasswordService _passwordService;
        private readonly IMvxMessenger _messenger;
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
        public HomeViewModel(IMvxNavigationService navigationService)
            : base(navigationService)
        {
        }
#else
        public HomeViewModel(IMvxNavigationService navigationService, IPasswordService passwordService, IMvxMessenger messenger)
            : base(navigationService)
        {
            _passwordService = passwordService;
            _messenger = messenger;
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
                await NavigationService.Navigate<HubViewModel>().ConfigureAwait(false);
            }
            else
            {
                _messenger.Publish(
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

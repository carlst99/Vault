using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using Vault.Core.Model.DbContext;
using Vault.Core.Model.Messages;
using Vault.Core.Services;

namespace Vault.Core.ViewModels
{
    public class SetupViewModel : Base.ViewModelBase<bool>
    {
        #region Fields

        private readonly IPasswordService _passwordService;
        private readonly IMvxMessenger _messenger;

        private string _password;
        private string _verifyPassword;
        private string _oldPassword;
        private bool _changeExistingPassword;

        #endregion

        #region Commands

        public IMvxCommand ChangePasswordCommand => new MvxCommand(OnChangePassword);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the users password
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// Gets or sets the users verification password
        /// </summary>
        public string VerifyPassword
        {
            get => _verifyPassword;
            set => SetProperty(ref _verifyPassword, value);
        }

        /// <summary>
        /// Gets or sets the users old password
        /// </summary>
        public string OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view should present a first-time setup model
        /// </summary>
        public bool ChangeExistingPassword
        {
            get => _changeExistingPassword;
            set => SetProperty(ref _changeExistingPassword, value);
        }

        #endregion

        public SetupViewModel(IMvxNavigationService navigationService, IPasswordService passwordService, IMvxMessenger messenger)
            : base(navigationService)
        {
            _passwordService = passwordService;
            _messenger = messenger;
        }

        private async void OnChangePassword()
        {
#if !DEBUG
            PasswordChangeResult result = await _passwordService.TryChangePasswordAsync(OldPassword, Password).ConfigureAwait(false);
            switch (result)
            {
                case PasswordChangeResult.Success:
                    await RealmHelpers.SetEncryptionKeyAsync(Password).ConfigureAwait(false);
                    EncryptorAssistant.SetEncryptorPassword(Password);
                    await NavigationService.Navigate<HubViewModel>().ConfigureAwait(false);
                    break;
                case PasswordChangeResult.OldPasswordIncorrect:
                    _messenger.Publish(
                        new DialogMessage(
                            this,
                            "Incorrect Password!", "You've entered the wrong old password. Please check the Caps Lock status of your computer and try again",
                            DialogMessage.DialogMessageType.Error));
                    OldPassword = string.Empty;
                    break;
                case PasswordChangeResult.Failure:
                    _messenger.Publish(
                        new DialogMessage(
                            this,
                            "Oops!", "Sorry, changing your password failed. Please try again.",
                            DialogMessage.DialogMessageType.Error));
                    break;
            }
#endif
        }

        public override void Prepare(bool parameter)
        {
            ChangeExistingPassword = parameter;
        }
    }
}

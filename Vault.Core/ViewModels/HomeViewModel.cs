using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using System.Diagnostics;
using System.Threading.Tasks;
using Vault.Core.Model.Messages;
using Vault.Core.Services;

namespace Vault.Core.ViewModels
{
    public class HomeViewModel : Base.ViewModelBase
    {
        #region Fields

        private readonly IPasswordService _passwordService;
        private readonly IMvxMessenger _messenger;

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

        public HomeViewModel(IMvxNavigationService navigationService, IPasswordService passwordService, IMvxMessenger messenger)
            : base(navigationService)
        {
            _passwordService = passwordService;
            _messenger = messenger;
        }

        private void OnUnlock()
        {
#if DEBUG
            NavigationService.Navigate<HubViewModel>();
#else
            if (_passwordService.VerifyPassword(Password))
            {
                NavigationService.Navigate<HubViewModel>();
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

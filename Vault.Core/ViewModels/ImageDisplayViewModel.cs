using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using System.Collections.Generic;
using Vault.Core.Model.Messages;
using Vault.Core.Services;

namespace Vault.Core.ViewModels
{
    public class ImageDisplayViewModel : Base.ViewModelBase
    {
        #region Fields

        private readonly IMvxMessenger _messenger;
        private readonly IImportService _importService;

        private bool _isImportInProgress;

        #endregion

        #region Commands

        public IMvxCommand ImportImagesCommand => new MvxCommand(ImportImages);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of images
        /// </summary>
        public int ImageCount => 0;

        /// <summary>
        /// Gets a value indicating whether or not an import is in progress
        /// </summary>
        public bool IsImportInProgress
        {
            get => _isImportInProgress;
            set => SetProperty(ref _isImportInProgress, value);
        }

        #endregion

        public ImageDisplayViewModel(IMvxNavigationService navigationService, IMvxMessenger messenger, IImportService importService)
            : base(navigationService)
        {
            _messenger = messenger;
            _importService = importService;
        }

        private void ImportImages()
        {
            IsImportInProgress = true;

            _messenger.Publish(new FileMessage(
                this,
                "Select images to import",
                FileMessage.DialogType.OpenFile,
                FileMessage.DefaultImageFilters)
            {
                Callback = CompleteImport
            });
        }

        private void CompleteImport(FileMessage.DialogResult result, IEnumerable<string> imagePaths)
        {
            IsImportInProgress = false;

            if (result == FileMessage.DialogResult.Failed)
                return;

            foreach (string element in imagePaths)
                _importService.TryImportImageAsync(element);
        }
    }
}

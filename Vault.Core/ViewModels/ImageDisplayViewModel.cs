using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vault.Core.Model.DbContext;
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
        private ObservableCollection<Media> _images;
        private Media _selectedImage;

        #endregion

        #region Commands

        public IMvxCommand ImportImagesCommand => new MvxCommand(ImportImages);
        public IMvxCommand ExportImageCommand => new MvxCommand(ExportImage);
        public IMvxCommand RemoveImageCommand => new MvxCommand(RemoveImage);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of images
        /// </summary>
        public int ImageCount => Images.Count;

        /// <summary>
        /// Gets a value indicating whether or not an import is in progress
        /// </summary>
        public bool IsImportInProgress
        {
            get => _isImportInProgress;
            set => SetProperty(ref _isImportInProgress, value);
        }

        /// <summary>
        /// Gets or sets the list of images
        /// </summary>
        public ObservableCollection<Media> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        /// <summary>
        /// Gets or sets the selected image
        /// </summary>
        public Media SelectedImage
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value);
        }

        #endregion

        public ImageDisplayViewModel(IMvxNavigationService navigationService, IMvxMessenger messenger, IImportService importService)
            : base(navigationService)
        {
            _messenger = messenger;
            _importService = importService;
            UpdateImageList();
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

        private async void CompleteImport(FileMessage.DialogResult result, IEnumerable<string> imagePaths)
        {
            if (result == FileMessage.DialogResult.Failed)
                return;

            foreach (string element in imagePaths)
            {
                Media media = await _importService.TryImportImageAsync(element).ConfigureAwait(true);
                Images.Add(media);
            }

            await RaisePropertyChanged(nameof(ImageCount)).ConfigureAwait(false);
            IsImportInProgress = false;
        }

        private void ExportImage()
        {
            throw new NotImplementedException();
        }

        private async void RemoveImage()
        {
            Media toRemove = SelectedImage;
            SelectedImage = null;
            Images.Remove(toRemove);
            await _importService.TryRemoveMediaAsync(toRemove).ConfigureAwait(true);
            await RaisePropertyChanged(nameof(ImageCount)).ConfigureAwait(true);
        }

        private void UpdateImageList()
        {
            Realm realm = RealmHelpers.GetRealmInstance();
            Images = new ObservableCollection<Media>(realm.All<Media>().Where(m => m.TypeRaw == (int)MediaType.Image).ToList());
        }
    }
}

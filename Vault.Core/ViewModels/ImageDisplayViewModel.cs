using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vault.Core.Model.DbContext;
using Vault.Core.Model.Messages;
using Vault.Core.Services;

namespace Vault.Core.ViewModels
{
    public class ImageDisplayViewModel : Base.ViewModelBase
    {
        #region Fields

        private readonly IImportService _importService;
        private readonly IMediaLoaderService _mediaLoaderService;

        private bool _isImportInProgress;
        private ObservableCollection<Media> _images;
        private Media _selectedImage;

        #endregion

        #region Commands

        public IMvxCommand ImportImagesCommand => new MvxCommand(ImportImages);
        public IMvxCommand ExportImageCommand => new MvxCommand(ExportImage);
        public IMvxCommand RemoveImageCommand => new MvxCommand(RemoveImage);
        public IMvxCommand OpenDialogCommand => new MvxCommand(() => NavigationService.Navigate<ImageFullScreenViewModel, Media>(SelectedImage));
        public IMvxCommand OpenDialogFromButtonCommand => new MvxCommand<Media>((m) => NavigationService.Navigate<ImageFullScreenViewModel, Media>(m));

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

        public ImageDisplayViewModel(
            IMvxNavigationService navigationService,
            IMvxMessenger messenger,
            IImportService importService,
            IMediaLoaderService mediaLoaderService)
            : base(navigationService, messenger)
        {
            _importService = importService;
            _mediaLoaderService = mediaLoaderService;

            Images = new ObservableCollection<Media>();
            UpdateImageList();
        }

        private void ImportImages()
        {
            IsImportInProgress = true;

            Messenger.Publish(new OpenFileDialogMessage(
                this,
                "Select images to import",
                OpenFileDialogMessage.DefaultImageFilters)
            {
                Callback = CompleteImport
            });
        }

        private async void CompleteImport(bool result, IEnumerable<string> imagePaths)
        {
            if (!result)
            {
                IsImportInProgress = false;
                return;
            }

            foreach (string element in imagePaths)
            {
                Media media = await _importService.TryImportImageAsync(element).ConfigureAwait(true);
            }

            UpdateImageList();
            await RaisePropertyChanged(nameof(ImageCount)).ConfigureAwait(false);
            IsImportInProgress = false;
        }

        private void ExportImage()
        {
            if (SelectedImage == null)
            {
                Messenger.Publish(new DialogMessage(this, "Woah!", "Please select an image to export", DialogMessage.DialogMessageType.Info));
                return;
            }

            IsImportInProgress = true;

            Messenger.Publish(new SaveFileDialogMessage(
                this,
                "Image Export",
                SelectedImage.Name,
                ".png")
            {
                Callback = CompleteExport
            });
        }

        private async void CompleteExport(bool result, string imagePath)
        {
            if (!result)
            {
                IsImportInProgress = false;
                return;
            }

            await _importService.TryExportMediaAsync(SelectedImage, imagePath).ConfigureAwait(false);
            IsImportInProgress = false;
        }

        private void RemoveImage()
        {
            async void CompleteRemoveImage(DialogMessage.DialogButton button)
            {
                if ((button & DialogMessage.DialogButton.Ok) == 0)
                    return;

                Media toRemove = SelectedImage;
                SelectedImage = null;
                Images.Remove(toRemove);
                await _importService.TryRemoveMediaAsync(toRemove).ConfigureAwait(true);
                await RaisePropertyChanged(nameof(ImageCount)).ConfigureAwait(true);
            }

            Messenger.Publish(new DialogMessage(
                this,
                "Confirm Deletion",
                "Are you sure you want to delete this item?",
                DialogMessage.DialogMessageType.Interaction)
            {
                Callback = CompleteRemoveImage
            });
        }

        private async void UpdateImageList()
        {
            IEnumerable<Media> diff = RealmInstance.All<Media>().Where(m => m.TypeRaw == (int)MediaType.Image).ToList();
            diff = diff.Except(Images);

            foreach (Media element in diff)
            {
                if (Images.Contains(element)) // If the image list has it but the realm doesn't, remove it
                {
                    Images.Remove(element);
                }
                else // If the realm has it but the image list doesn't, add it
                {
                    element.ContentStream = await _mediaLoaderService.LoadImageAsync(element.ThumbPath).ConfigureAwait(true);
                    Images.Add(element);
                }
            }

            await RaisePropertyChanged(nameof(ImageCount)).ConfigureAwait(false);
        }

        protected override void OnMediaUpdatedMessage(MediaUpdatedMessage message)
        {
            if (message.UpdatedMedia.Type != MediaType.Image)
                return;

            switch (message.Type)
            {
                case MediaUpdatedMessage.UpdateType.Added:
                    UpdateImageList();
                    break;
                case MediaUpdatedMessage.UpdateType.Removed:
                    if (SelectedImage == message.UpdatedMedia)
                        SelectedImage = null;
                    Images.Remove(message.UpdatedMedia);
                    RaisePropertyChanged(nameof(ImageCount));
                    break;
                case MediaUpdatedMessage.UpdateType.Updated:
                    Images.Remove(message.UpdatedMedia);
                    UpdateImageList();
                    break;
            }
        }
    }
}

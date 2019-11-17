using MvvmCross.Commands;
using MvvmCross.Navigation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vault.Core.Model.DbContext;
using Vault.Core.Services;

namespace Vault.Core.ViewModels
{
    public class ImageFullScreenViewModel : Base.ViewModelBase<Media>
    {
        #region Fields

        private readonly IImportService _importService;
        private readonly IMediaLoaderService _mediaLoaderService;

        private readonly List<Media> _images;
        private Media _selectedImage;
        private int _selectedImageIndex;
        private bool _canEditImage = true;

        #endregion

        #region Commands

        public IMvxCommand CycleImageLeftCommand => new MvxCommand(() => OnCycleImage(true));
        public IMvxCommand CycleImageRightCommand => new MvxCommand(() => OnCycleImage(false));
        public IMvxCommand RemoveImageCommand => new MvxCommand(OnRemoveImage);
        public IMvxCommand RotateImageCommand => new MvxCommand(OnRotateImage);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected image
        /// </summary>
        public Media SelectedImage
        {
            get => _selectedImage;
            set => SetProperty(ref _selectedImage, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the selected image can be edited
        /// </summary>
        public bool CanEditImage
        {
            get => _canEditImage;
            set => SetProperty(ref _canEditImage, value);
        }

        #endregion

        public ImageFullScreenViewModel(
            IMvxNavigationService navigationService,
            IImportService importService,
            IMediaLoaderService mediaLoaderService)
            : base(navigationService)
        {
            _importService = importService;
            _mediaLoaderService = mediaLoaderService;
            _images = RealmInstance.All<Media>().Where(m => m.TypeRaw == (int)MediaType.Image).ToList();
            SelectedImage = _images[_selectedImageIndex];
        }

        private void OnCycleImage(bool cycleLeft)
        {
            if (cycleLeft)
            {
                _selectedImageIndex--;
                if (_selectedImageIndex == -1)
                    _selectedImageIndex = _images.Count - 1;
            }
            else
            {
                _selectedImageIndex++;
                if (_selectedImageIndex == _images.Count)
                    _selectedImageIndex = 0;
            }

            SelectedImage = _images[_selectedImageIndex];
        }

        private async void OnRemoveImage()
        {
            CanEditImage = false;
            Media toRemove = SelectedImage;
            OnCycleImage(false);
            _images.Remove(toRemove);
            await _importService.TryRemoveMediaAsync(toRemove).ConfigureAwait(true);
            CanEditImage = true;
        }

        private async void OnRotateImage()
        {
            CanEditImage = false;
            string imagePath = SelectedImage.FilePath;
            string thumbPath = SelectedImage.ThumbPath;
            await Task.Run(() =>
            {
                // Update image
                using (Image image = Image.Load(_mediaLoaderService.LoadImageAsync(imagePath).Result))
                using (MemoryStream rotatedImage = new MemoryStream())
                {
                    image.Mutate(i => i.Rotate(RotateMode.Rotate90));
                    image.SaveAsPng(rotatedImage);
                    _mediaLoaderService.TryUpdateMediaAsync(imagePath, rotatedImage).Wait();
                }

                // Update thumb
                using (Image image = Image.Load(_mediaLoaderService.LoadImageAsync(thumbPath).Result))
                using (MemoryStream rotatedImage = new MemoryStream())
                {
                    image.Mutate(i => i.Rotate(RotateMode.Rotate90));
                    image.SaveAsPng(rotatedImage);
                    _mediaLoaderService.TryUpdateMediaAsync(thumbPath, rotatedImage).Wait();
                }
            }).ConfigureAwait(false);
            await RaisePropertyChanged(nameof(SelectedImage)).ConfigureAwait(false);
            CanEditImage = true;
        }

        public override void Prepare(Media parameter)
        {
            _selectedImage = parameter;
            _selectedImageIndex = _images.IndexOf(parameter);
        }
    }
}

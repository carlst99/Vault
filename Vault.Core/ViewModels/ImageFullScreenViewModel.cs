using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.Generic;
using System.Linq;
using Vault.Core.Model.DbContext;
using Vault.Core.Services;

namespace Vault.Core.ViewModels
{
    public class ImageFullScreenViewModel : Base.ViewModelBase<Media>
    {
        #region Fields

        private readonly IImportService _importService;
        private readonly List<Media> _images;
        private Media _selectedImage;
        private int _selectedImageIndex;

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

        #endregion

        public ImageFullScreenViewModel(IMvxNavigationService navigationService, IImportService importService)
            : base(navigationService)
        {
            _importService = importService;
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
            Media toRemove = SelectedImage;
            OnCycleImage(false);
            _images.Remove(toRemove);
            await _importService.TryRemoveMediaAsync(toRemove).ConfigureAwait(true);
        }

        private void OnRotateImage()
        {

        }

        public override void Prepare(Media parameter)
        {
            _selectedImage = parameter;
            _selectedImageIndex = _images.IndexOf(parameter);
        }
    }
}

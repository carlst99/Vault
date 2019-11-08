using MvvmCross.Commands;
using MvvmCross.Navigation;
using Realms;
using System.Collections.Generic;
using System.Linq;
using Vault.Core.Model.DbContext;

namespace Vault.Core.ViewModels
{
    public class ImageFullScreenViewModel : Base.ViewModelBase
    {
        #region Fields

        private readonly List<Media> _images;
        private Media _selectedImage;
        private int _selectedImageIndex;

        #endregion

        #region Commands

        public IMvxCommand CycleImageLeftCommand => new MvxCommand(() => OnCycleImage(true));
        public IMvxCommand CycleImageRightCommand => new MvxCommand(() => OnCycleImage(false));

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

        public ImageFullScreenViewModel(IMvxNavigationService navigationService)
            : base(navigationService)
        {
            Realm realm = RealmHelpers.GetRealmInstance();
            _images = realm.All<Media>().Where(m => m.TypeRaw == (int)MediaType.Image).ToList();
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
    }
}

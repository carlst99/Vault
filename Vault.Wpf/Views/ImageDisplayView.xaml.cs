using MvvmCross.Platforms.Wpf.Views;
using MvvmCrossExtensions.Wpf.Presenters.MasterDetail;
using Realms;
using System.Windows;
using Vault.Core.Model.DbContext;

namespace Vault.Wpf.Views
{
    [DetailPresentation]
    public partial class ImageDisplayView : MvxWpfView
    {
        private readonly Realm _realmInstance;
        private readonly Preferences _userPreferences;

        public ImageDisplayView()
        {
            InitializeComponent();
            _realmInstance = RealmHelpers.GetRealmInstance();
            _userPreferences = RealmHelpers.GetUserPreferences(_realmInstance);

            SldrThumbnailSize.Value = _userPreferences.ImageThumbnailSize;
        }

        private void SldrThumbnailSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _realmInstance?.Write(() => _userPreferences.ImageThumbnailSize = (int)e.NewValue);
        }
    }
}

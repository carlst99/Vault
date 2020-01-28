using MvvmCross.Platforms.Wpf.Views;
using MvvmCrossExtensions.Wpf.Presenters.MasterDetail;
using Realms;
using System.Windows;
using Vault.Core.Model.DbContext;
using Vault.Core.ViewModels;

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

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Handle any double clicks
            if (e.ClickCount == 2)
            {
                ((ImageDisplayViewModel)ViewModel).OpenDialogCommand.Execute();
            }
        }
    }
}

using MaterialDesignThemes.Wpf;
using MvvmCross.Platforms.Wpf.Views;
using Realms;
using System;
using Vault.Core.Model.DbContext;

namespace Vault.Wpf.Views
{
    public partial class HubView : MvxWpfView
    {
        private readonly Realm _realmInstance;
        private readonly Preferences _userPreferences;

        public HubView()
        {
            InitializeComponent();
            _realmInstance = RealmHelpers.GetRealmInstance();
            _userPreferences = RealmHelpers.GetUserPreferences(_realmInstance);

            if (_userPreferences.DarkModeEnabled)
            {
                EnableDarkMode(this, null);
                TglBtnDarkMode.IsChecked = true;
            }
        }

        private void EnableDarkMode(object sender, System.Windows.RoutedEventArgs e)
        {
            ModifyTheme(t => t.SetBaseTheme(Theme.Dark));
            _realmInstance.Write(() => _userPreferences.DarkModeEnabled = true);
        }

        private void DisableDarkMode(object sender, System.Windows.RoutedEventArgs e)
        {
            ModifyTheme(t => t.SetBaseTheme(Theme.Light));
            _realmInstance.Write(() => _userPreferences.DarkModeEnabled = false);
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}

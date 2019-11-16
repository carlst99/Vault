using MaterialDesignThemes.Wpf;
using MvvmCross.Platforms.Wpf.Views;
using Realms;
using System;
using Vault.Core.Model.DbContext;

namespace Vault.Wpf.Views
{
    public partial class HubView : MvxWpfView
    {
        private readonly Realm _realm;
        private readonly Preferences _preferences;

        public HubView()
        {
            InitializeComponent();
            _realm = RealmHelpers.GetRealmInstance();
            _preferences = RealmHelpers.GetPreferences(_realm);

            if (_preferences.DarkModeEnabled)
            {
                EnableDarkMode(this, null);
                TglBtnDarkMode.IsChecked = true;
            }
        }

        private void EnableDarkMode(object sender, System.Windows.RoutedEventArgs e)
        {
            ModifyTheme(t => t.SetBaseTheme(Theme.Dark));
            _realm.Write(() => _preferences.DarkModeEnabled = true);
        }

        private void DisableDarkMode(object sender, System.Windows.RoutedEventArgs e)
        {
            ModifyTheme(t => t.SetBaseTheme(Theme.Light));
            _realm.Write(() => _preferences.DarkModeEnabled = false);
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

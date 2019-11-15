using MaterialDesignThemes.Wpf;
using MvvmCross.Platforms.Wpf.Views;
using System;

namespace Vault.Wpf.Views
{
    public partial class HubView : MvxWpfView
    {
        public HubView()
        {
            InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ModifyTheme(t => t.SetBaseTheme(Theme.Dark));
        }

        private void ToggleButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ModifyTheme(t => t.SetBaseTheme(Theme.Light));
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

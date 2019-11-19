using Vault.Core.Model.DbContext;
using Xunit;

namespace Vault.Core.Tests.Model.DbContext
{
    public class PreferencesTests
    {
        [Fact]
        public void TestCtor()
        {
            Preferences preferences = new Preferences();
            Assert.True(preferences.ImageThumbnailSize != 0);
        }

        [Fact]
        public void TestPropertyGetters()
        {
            const bool darkMode = true;
            Preferences preferences = new Preferences
            {
                DarkModeEnabled = darkMode
            };

            Assert.Equal(darkMode, preferences.DarkModeEnabled);
        }
    }
}

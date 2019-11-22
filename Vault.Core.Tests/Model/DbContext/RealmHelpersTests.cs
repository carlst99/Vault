#if !DEBUG
using System;
using System.IO;
#endif
using Vault.Core.Model.DbContext;
using Vault.Core.Tests.Contexts;
using Xunit;

namespace Vault.Core.Tests.Model.DbContext
{
    public class RealmHelpersTests
    {
#if DEBUG
        [Fact]
        public void TestGetRealmInstance()
        {
            Assert.NotNull(RealmHelpers.GetRealmInstance());
        }

        [Fact]
        public void TestGetNextId()
        {
            RealmContext.RunInMemory((realm) =>
            {
                Media media = new Media(0, MediaType.Image)
                {
                    Name = "test"
                };
                realm.Write(() => realm.Add(media));
                Media media1 = new Media(1, MediaType.Image)
                {
                    Name = "test"
                };
                realm.Write(() => realm.Add(media1));

                int id = RealmHelpers.GetNextId<Media>(realm);
                Assert.True(id == 2);

                // Check that it gets a realm instance when not provided with one
                Assert.True(RealmHelpers.GetNextId<Media>() >= 0);
            });
        }

        [Fact]
        public void TestGetUserPreferences()
        {
            RealmContext.RunInMemory((realm) =>
            {
                Preferences pref = RealmHelpers.GetUserPreferences(realm);
                Assert.NotNull(pref);

                bool value = pref.DarkModeEnabled;
                realm.Write(() => pref.DarkModeEnabled = !value);
                pref = RealmHelpers.GetUserPreferences(realm);
                Assert.Equal(!value, pref.DarkModeEnabled);

                // Check that it gets a realm instance when not provided with one
                Assert.NotNull(RealmHelpers.GetUserPreferences());
            });
        }
#else
        [Fact]
        public void TestGetRealmInstance()
        {
            Assert.Throws<InvalidOperationException>(RealmHelpers.GetRealmInstance);
        }
#endif
    }
}

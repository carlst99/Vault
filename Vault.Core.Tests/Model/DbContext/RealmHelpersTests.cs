using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using Vault.Core.Model.DbContext;
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
#endif

        [Fact]
        public void TestGetNextId()
        {
            Media media = new Media(0, MediaType.Image)
            {
                Name = "test"
            };
            Realm realm = Realm.GetInstance(new InMemoryConfiguration("inMemoryRealm"));

            //int id = RealmHelpers.GetNextId<Media>(realm);
            //Assert.True(id > 0);
        }
    }
}

using Nito.AsyncEx;
using Realms;
using System;
using System.Threading;
using Vault.Core.Model.DbContext;

namespace Vault.Core.Tests.Contexts
{
    public static class RealmContext
    {
        [Obsolete("Use of RunInMemory(Action<Realm> run) is preferred")]
        public static Realm Get()
        {
            // Make use of a media object to fix occasional issues where Realm can't find any `RealmObject`s
#pragma warning disable CS0168 // Variable is declared but never used
            Media media;
#pragma warning restore CS0168 // Variable is declared but never used

            // Unset the synchronisation context as xUnit makes use of one which Realm cannot use
            var context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);

            Realm realm =  Realm.GetInstance(new InMemoryConfiguration("inMemoryRealm"));
            SynchronizationContext.SetSynchronizationContext(context);

            return realm;
        }

        public static void RunInMemory(Action<Realm> run)
        {
            AsyncContext.Run(() =>
            {
                InMemoryConfiguration config = new InMemoryConfiguration("inMemoryRealm")
                {
                    ObjectClasses = new Type[] { typeof(Media), typeof(Preferences) }
                };
                using (Realm realm = Realm.GetInstance(config))
                    run.Invoke(realm);
                Realm.DeleteRealm(config);
            });
        }

        public static void RunOnDisk(Action<Realm> run)
        {
            AsyncContext.Run(() =>
            {
                RealmConfiguration config = new RealmConfiguration(".\\vaultTestRealm.realm")
                {
                    ObjectClasses = new Type[] { typeof(Media), typeof(Preferences) }
                };
                using (Realm realm = Realm.GetInstance(config))
                    run.Invoke(realm);
                Realm.DeleteRealm(config);
            });
        }
    }
}

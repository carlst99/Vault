using Realms;
using System;
using System.Collections.Generic;

namespace Vault.Core.Model.DbContext
{
    public static class RealmHelpers
    {
        private static readonly Dictionary<Type, int> _currentIds = new Dictionary<Type, int>();

        public static Realm GetRealmInstance()
        {
            string realmPath = App.GetAppdataFilePath("Vault.realm");

            RealmConfiguration config = new RealmConfiguration(realmPath)
            {
                EncryptionKey = new byte[64]
                {
                    0xff, 0xad, 0x8c, 0x9e, 0x29, 0x8d, 0xec, 0xd6, 0x6c, 0x34, 0xb6, 0x1f, 0x84, 0xac, 0x3d, 0xec,
                    0x41, 0x79, 0x35, 0x72, 0x36, 0x7a, 0x49, 0x80, 0x35, 0x36, 0x1c, 0x9a, 0xbd, 0xb6, 0xa9, 0x43,
                    0x17, 0xc2, 0x6c, 0x1d, 0x52, 0xe6, 0xf4, 0xf7, 0x2f, 0x27, 0x12, 0xb3, 0x69, 0x8f, 0x1c, 0x0b,
                    0x8b, 0xf9, 0x34, 0x80, 0x18, 0xd0, 0x29, 0x64, 0xb7, 0xcb, 0xc7, 0xe3, 0xa4, 0x6d, 0xf5, 0x4f
                }
            };
            return Realm.GetInstance(config);
        }

        public static int GetNextId<T>(Realm realm = null) where T : RealmObject, IContextItem
        {
            if (realm == null)
                realm = GetRealmInstance();

            if (!_currentIds.ContainsKey(typeof(T)))
            {
                int max = 0;
                foreach (T element in realm.All<T>())
                {
                    if (element.Id > max)
                        max = element.Id;
                }
                _currentIds.Add(typeof(T), max);
            }

            return ++_currentIds[typeof(T)];
        }

        public static void ClearNextIds() => _currentIds.Clear();
    }
}

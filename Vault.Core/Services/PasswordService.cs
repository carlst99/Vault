using Realms;
using Serilog;
using System;
using System.Linq;
using Vault.Core.Model.DbContext;

namespace Vault.Core.Services
{
    public class PasswordService : IPasswordService
    {
        public bool TryChangePassword(string oldPassword, string newPassword)
        {
            Realm realm = RealmHelpers.GetRealmInstance();
            IQueryable<Password> passwords = realm.All<Password>();

            if (passwords.Count() == 0) // No password in storage, need to set new one
            {
                return TrySetPassword(newPassword);
            } else
            {
                // Check that old password matches that in database
                if (!VerifyPassword(oldPassword))
                    return false;

                return TrySetPassword(newPassword);
            }
        }

        public bool VerifyPassword(string password)
        {
            Realm realm = RealmHelpers.GetRealmInstance();
            IQueryable<Password> passwords = realm.All<Password>();

            if (passwords.Count() == 1)
            {
                Password stored = passwords.First();
                return BCrypt.Net.BCrypt.EnhancedVerify(password, stored.Hash);
            } else
            {
                return false;
            }
        }

        private bool TrySetPassword(string password)
        {
            try
            {
                Realm realm = RealmHelpers.GetRealmInstance();
                IQueryable<Password> passwords = realm.All<Password>();
                string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

                if (passwords.Count() == 1) // Update an existing password
                {
                    Password stored = passwords.First();
                    realm.Write(() => stored.Hash = hashedPassword);
                } else // Add a new password
                {
                    Password p = new Password
                    {
                        Hash = hashedPassword
                    };
                    realm.Write(() => realm.Add(p));
                }
                return true;
            } catch (Exception ex)
            {
                Log.Error(ex, "Could not set new password");
                return false;
            }
        }
    }
}

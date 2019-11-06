using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Vault.Core.Services
{
    public class PasswordService : IPasswordService
    {
        private static readonly string HASH_FILE_LOCATION = App.GetAppdataFilePath("hash");
        public static readonly string SALT_FILE_LOCATION = App.GetAppdataFilePath("salt");

        public async Task<bool> TryChangePasswordAsync(string oldPassword, string newPassword)
        {
            if (!File.Exists(HASH_FILE_LOCATION)) // No password in storage, need to set new one
            {
                return await TrySetPasswordAsync(newPassword).ConfigureAwait(false);
            } else
            {
                // Check that old password matches that in database
                if (!await TryVerifyPasswordAsync(oldPassword).ConfigureAwait(false))
                    return false;

                return await TrySetPasswordAsync(newPassword).ConfigureAwait(false);
            }
        }

        public async Task<bool> TryVerifyPasswordAsync(string password)
        {
            return await Task.Run(() =>
            {
                if (File.Exists(HASH_FILE_LOCATION))
                {
                    using (StreamReader sr = new StreamReader(HASH_FILE_LOCATION))
                        return BCrypt.Net.BCrypt.EnhancedVerify(password, sr.ReadLine());
                }
                else
                {
                    throw App.CreateError<InvalidOperationException>("A password has not yet been set!");
                }
            }).ConfigureAwait(false);
        }

        private async Task<bool> TrySetPasswordAsync(string password)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
                // Save the hashed password
                using (StreamWriter sw = new StreamWriter(HASH_FILE_LOCATION))
                    await sw.WriteLineAsync(hashedPassword).ConfigureAwait(false);
                // Save a salt to use when deriving the password into bytes for opening the realm and encryptor
                using (StreamWriter sw = new StreamWriter(SALT_FILE_LOCATION))
                    await sw.WriteLineAsync(BCrypt.Net.BCrypt.GenerateSalt()).ConfigureAwait(false);
                return true;
            } catch (Exception ex)
            {
                Log.Error(ex, "Could not set new password");
                return false;
            }
        }
    }
}

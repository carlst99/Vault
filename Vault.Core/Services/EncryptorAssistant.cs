using StreamEncryptor.Predefined;
#if !DEBUG
using System;
#endif

namespace Vault.Core.Services
{
    public static class EncryptorAssistant
    {
        private static readonly AesHmacEncryptor _encryptor = new AesHmacEncryptor("V7GAe5ZRJ4GtxZ3S8jJLCZNQP2SXTyO4");
        private static bool _initialised;

        public static AesHmacEncryptor GetEncryptor()
        {
#if DEBUG
            return _encryptor;
#else
            if (!_initialised)
                throw App.CreateError<InvalidOperationException>("A password must be set for the encryptor before it can be used");
            else
                return _encryptor;
#endif
        }

        public static void SetEncryptorPassword(string password)
        {
            _encryptor.SetPassword(password);
            _initialised = true;
        }
    }
}

using StreamEncryptor.Predefined;
using System;

namespace Vault.Core.Services
{
    public static class EncryptorAssistant
    {
        private static readonly AesHmacEncryptor _encryptor = new AesHmacEncryptor("undefined");
        private static bool _initialised;

        public static AesHmacEncryptor GetEncryptor()
        {
            if (!_initialised)
                throw App.CreateError<InvalidOperationException>("A password must be set for the encryptor before it can be used");
            else
                return _encryptor;
        }

        public static void SetEncryptorPassword(string password)
        {
            _encryptor.SetPassword(password);
            _initialised = true;
        }
    }
}

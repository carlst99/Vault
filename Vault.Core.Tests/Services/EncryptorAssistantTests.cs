using Vault.Core.Services;
using Xunit;
#if !DEBUG
using StreamEncryptor;
using System;
#endif

namespace Vault.Core.Tests.Services
{
    public class EncryptorAssistantTests
    {
#if DEBUG
        [Fact]
        public void TestGetEncryptor()
        {
            Assert.NotNull(EncryptorAssistant.GetEncryptor());
        }
#else
        [Fact]
        public void TestGetEncryptor()
        {
            Assert.Throws<InvalidOperationException>(EncryptorAssistant.GetEncryptor);
            EncryptorAssistant.SetEncryptorPassword("rand0m");
            IEncryptor encryptor = EncryptorAssistant.GetEncryptor();
            Assert.NotNull(encryptor);
        }
#endif
    }
}

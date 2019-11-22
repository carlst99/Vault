using System;
using System.Collections.Generic;
using System.Text;
using Vault.Core.Model.Messages;
using Xunit;

namespace Vault.Core.Tests.Model.Messages
{
    public class SaveFileDialogMessageTests
    {
        [Fact]
        public void TestCtor()
        {
            const string title = "testTitle";
            const string fileName = "defaultFileName";
            const string extension = ".default";

            SaveFileDialogMessage message = new SaveFileDialogMessage(this, title, fileName, extension);
            Assert.Equal(this, message.Sender);
            Assert.Equal(title, message.Title);
            Assert.Equal(fileName, message.DefaultFileName);
            Assert.Equal(extension, message.DefaultFileExtension);
        }

        [Fact]
        public void TestProperties()
        {
            Action<bool, string> callback = new Action<bool, string>((_, __) => Math.Max(0, 1));

            SaveFileDialogMessage message = new SaveFileDialogMessage(this, null, null, null)
            {
                Callback = callback
            };
            Assert.Equal(callback, message.Callback);
        }
    }
}

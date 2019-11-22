using System;
using System.Collections.Generic;
using Vault.Core.Model.Messages;
using Xunit;

namespace Vault.Core.Tests.Model.Messages
{
    public class OpenFileDialogMessageTests
    {
        [Fact]
        public void TestCtor()
        {
            const string title = "testTitle";
            var filters = OpenFileDialogMessage.DefaultImageFilters;
            const bool multiSelect = true;

            OpenFileDialogMessage message = new OpenFileDialogMessage(this, title, filters, multiSelect);
            Assert.Equal(this, message.Sender);
            Assert.Equal(title, message.Title);
            Assert.Equal(filters, message.Filters);
            Assert.Equal(multiSelect, message.MultiSelect);
        }

        [Fact]
        public void TestProperties()
        {
            Action<bool, IEnumerable<string>> callback = new Action<bool, IEnumerable<string>>((_, __) => Math.Max(0, 1));
            OpenFileDialogMessage message = new OpenFileDialogMessage(this, null, null, false)
            {
                Callback = callback
            };

            Assert.Equal(callback, message.Callback);
        }
    }
}

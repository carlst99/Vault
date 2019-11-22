using System;
using Vault.Core.Model.Messages;
using Xunit;

namespace Vault.Core.Tests.Model.Messages
{
    public class DialogMessagesTests
    {
        [Fact]
        public void TestCtor()
        {
            const string title = "testTitle";
            const string content = "testContent";
            const DialogMessage.DialogMessageType type = DialogMessage.DialogMessageType.Info;

            DialogMessage message = new DialogMessage(this, title, content, type);
            Assert.Equal(this, message.Sender);
            Assert.Equal(title, message.Title);
            Assert.Equal(content, message.Content);
            Assert.Equal(type, message.MessageType);
        }

        [Fact]
        public void TestProperties()
        {
            Action<DialogMessage.DialogButton> callback = new Action<DialogMessage.DialogButton>((_) => Math.Max(0, 1));

            DialogMessage message = new DialogMessage(this, null, null, DialogMessage.DialogMessageType.Error)
            {
                Callback = callback
            };

            Assert.Equal(callback, message.Callback);
        }
    }
}

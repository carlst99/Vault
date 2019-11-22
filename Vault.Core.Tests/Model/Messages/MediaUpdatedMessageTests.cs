using Vault.Core.Model.DbContext;
using Vault.Core.Model.Messages;
using Xunit;

namespace Vault.Core.Tests.Model.Messages
{
    public class MediaUpdatedMessageTests
    {
        [Fact]
        public void TestCtor()
        {
            Media media = new Media(int.MaxValue, MediaType.Image);
            const MediaUpdatedMessage.UpdateType updateType = MediaUpdatedMessage.UpdateType.Removed;

            MediaUpdatedMessage message = new MediaUpdatedMessage(this, media, updateType);
            Assert.Equal(this, message.Sender);
            Assert.Equal(media, message.UpdatedMedia);
            Assert.Equal(updateType, message.Type);
        }
    }
}

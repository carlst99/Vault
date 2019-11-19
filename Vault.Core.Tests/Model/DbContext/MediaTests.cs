using System;
using System.Collections.Generic;
using System.Text;
using Vault.Core.Model.DbContext;
using Xunit;

namespace Vault.Core.Tests.Model.DbContext
{
    public class MediaTests
    {
        [Fact]
        public void TestTypeProperty()
        {
            Media media = GetMedia();
            media.Type = MediaType.Video;
            Assert.Equal((int)media.Type, media.TypeRaw);
        }

        [Fact]
        public void TestEquals()
        {
            Media media = GetMedia();
            Assert.Equal(media, GetMedia());
            media.Id++;
            Assert.NotEqual(media, GetMedia());
        }

        [Fact]
        public void TestGetHashCode()
        {
            Media media = GetMedia();
            media.Id = 1;
            Assert.True(media.GetHashCode() != default);
            Assert.NotEqual(media.GetHashCode(), GetMedia().GetHashCode());
        }

        private Media GetMedia()
        {
            return new Media
            {
                FilePath = "null",
                ThumbPath = "nullThumb",
                Id = 0,
                Name = "MediaObject",
                Type = MediaType.Image
            };
        }
    }
}

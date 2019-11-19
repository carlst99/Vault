using System;
using System.Collections.Generic;
using System.IO;
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
            Assert.NotEqual(media, new object());
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

        [Fact]
        public void TestToString()
        {
            Media media = GetMedia();
            Assert.Equal(media.Name, media.ToString());
        }

        [Fact]
        public void TestCtor()
        {
            const int id = 99;
            const MediaType type = MediaType.Video;

            Media media = new Media(id, type);
            Assert.Equal(id, media.Id);
            Assert.Equal(type, media.Type);
        }

        [Fact]
        public void TestPropertyGetters()
        {
            const string filePath = "filePath";
            const string thumbPath = "thumbPath";
            Stream contentStream = Stream.Null;

            Media media = new Media
            {
                FilePath = filePath,
                ThumbPath = thumbPath,
                ContentStream = contentStream
            };

            Assert.Equal(filePath, media.FilePath);
            Assert.Equal(thumbPath, media.ThumbPath);
            Assert.Equal(contentStream, media.ContentStream);
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

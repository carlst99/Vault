using Moq;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Vault.Core.Model.Messages;
using Vault.Core.Services;
using Xunit;

namespace Vault.Core.Tests.Services
{
    public class ImportServiceTests
    {
        //protected override void AdditionalSetup()
        //{
        //    var mockMessenger = new Mock<IMvxMessenger>();
        //    mockMessenger.Setup(m => m.Publish(It.IsAny<DialogMessage>())).Verifiable();
        //    Ioc.RegisterSingleton(mockMessenger.Object);
        //}

        [Fact]
        public void TestSavePaths()
        {
            Assert.False(string.IsNullOrEmpty(ImportService.IMAGE_FOLDER_PATH));
            Assert.False(string.IsNullOrEmpty(ImportService.VIDEO_FOLDER_PATH));
        }

        [Fact]
        public void TestCheckDirectoryExists()
        {
            ImportService importService = GetImportService();

        }

        [Fact]
        public void TestTryImportImageAsync()
        {
            string directoryPath = "C:\\vault";
            ImportService importService = GetImportService(out MockFileSystem fileSystem);
            Assert.False(fileSystem.Directory.Exists(directoryPath));
            importService.CheckDirectoryExists(directoryPath);
            Assert.True(fileSystem.Directory.Equals(directoryPath));
        }

        private ImportService GetImportService(out MockFileSystem fileSystem)
        {
            MockFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
                { @"c:\demo\jQuery.js", new MockFileData("some js") },
                { @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
            });
            return new ImportService(null, fileSystem);
        }
    }
}

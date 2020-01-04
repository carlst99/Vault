using Moq;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Tests;
using System;
using System.Collections.Generic;
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
        public void TestTryImportImageAsync()
        {

        }
    }
}

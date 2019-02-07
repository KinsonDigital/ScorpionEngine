using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System;
using System.IO;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class SetupDeployServiceTests
    {
        #region Method Tests
        [Test]
        public void Deploy_WhenInvoked_DeploysSetup()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var deployService = new SetupDeployService(mockDirService.Object, mockFileService.Object);

            //Act
            deployService.Deploy("test-project", "test-setup", @"C:\temp");

            //Assert
            mockFileService.Verify(m => m.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Deploy_WhenInvoked_UsesCorrectSetupPath()
        {
            //Arrange
            var actual = "";
            var expected = @"\test-project\test-setup.json";
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Copy(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((setupPath, destinationPath) =>
            {
                var sections = setupPath.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length >= 2 ? sections[1] : "";
            });

            var deployService = new SetupDeployService(mockDirService.Object, mockFileService.Object);

            //Act
            deployService.Deploy("test-project", "test-setup", @"C:\temp");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Deploy_WhenInvokedWithInvalidDestinationPath_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var deployService = new SetupDeployService(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(DirectoryNotFoundException), () =>
            {
                deployService.Deploy("test-project", "test-setup", @"C:\temp");
            });
        }


        [Test]
        public void Deploy_WhenInvokedWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() => false);

            var mockFileService = new Mock<IFileService>();

            var deployService = new SetupDeployService(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                deployService.Deploy("test-project", "test-setup", @"C:\temp");
            });
        }
        #endregion
    }
}

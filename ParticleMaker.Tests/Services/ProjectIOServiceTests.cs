using Moq;
using NUnit.Framework;
using ParticleMaker.Services;
using System;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class ProjectIOServiceTests
    {
        #region Method Tests
        [Test]
        public void CheckRootProjectsFolder_WhenInvokingWhenDirectoryExists_DoesNotCreateFolder()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var service = new ProjectIOService(mockDirService.Object, new Mock<IFileService>().Object);

            //Act
            service.CheckRootProjectsFolder();

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Never());
        }


        [Test]
        public void CheckRootProjectsFolder_WhenInvokingWhenDirectoryDoesNotExist_CreatesFolder()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var service = new ProjectIOService(mockDirService.Object, new Mock<IFileService>().Object);

            //Act
            service.CheckRootProjectsFolder();

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void ProjectExists_WhenInvokingWithExitingPath_ReturnsTrue()
        {
            //Arrange
            var expected = true;

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var service = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            var actual = service.ProjectExists("test-project");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CheckRootSetupsFolder_WhenInvoked_ChecksIfDirectoryExists()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();

            var mockFileService = new Mock<IFileService>();

            var projIOService = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            projIOService.CheckRootSetupsFolder(It.IsAny<string>());

            //Assert
            mockDirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void CheckRootSetupsFolder_WhenInvokedWithNonExistingDirectory_CreatesSetupDirectory()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();

            var mockFileService = new Mock<IFileService>();

            var projIOService = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            projIOService.CheckRootSetupsFolder(It.IsAny<string>());

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void CheckRootSetupsFolder_WhenInvoked_BuildsCorrectSetupsPath()
        {
            //Arrange
            var expected = "Setups";
            var actual = string.Empty;

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns<string>((path) =>
            {
                var pathSections = path.Split(new[] { "test-project" }, StringSplitOptions.None);

                actual = pathSections.Length >= 1 ? pathSections[pathSections.Length - 1].Replace("\\", "") : string.Empty;


                return true;
            });

            var mockFileService = new Mock<IFileService>();

            var projIOService = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            projIOService.CheckRootSetupsFolder("test-project");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ContainsIllegalCharacters_WhenInvokingWithIllegalCharacters_ReturnsTrue()
        {
            //Arrange
            var expected = true;

            var mockDirService = new Mock<IDirectoryService>();
            var mockFileService = new Mock<IFileService>();

            var service = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            var actual = service.ContainsIllegalCharacters(@"test*item");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ContainsIllegalCharacters_WhenInvokingWithNoIllegalCharacters_ReturnsFalse()
        {
            //Arrange
            var expected = false;

            var mockDirService = new Mock<IDirectoryService>();
            var mockFileService = new Mock<IFileService>();

            var service = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            var actual = service.ContainsIllegalCharacters(@"testitem");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetupExists_WhenInvoked_ChecksIfDirectoryExists()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            var mockFileService = new Mock<IFileService>();

            var service = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            service.SetupExists("test-project", "test-setup");

            //Assert
            mockDirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void SetupExists_WhenInvoked_ChecksIfFileExists()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var service = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            //Act
            service.SetupExists("test-project", "test-setup");

            //Assert
            mockFileService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
        }
        #endregion
    }
}

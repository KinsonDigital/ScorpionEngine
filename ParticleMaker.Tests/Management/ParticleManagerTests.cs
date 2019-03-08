using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Management;
using ParticleMaker.Services;
using System;

namespace ParticleMaker.Tests.Management
{
    [TestFixture]
    public class ParticleManagerTests
    {
        #region Method Tests
        [Test]
        public void AddParticle_WhenInvoked_AddsParticleToSetup()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.AddParticle("test-project", "test-setup", "");

            //Assert
            mockFileService.Verify(m => m.Copy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }


        [Test]
        public void AddParticle_WhenInvoked_BuildsCorrectDestinationPath()
        {
            //Arrange
            var expected = @"\test-project\Setups\test-setup\test-particle.png";
            var actual = string.Empty;

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockFileService.Setup(m => m.Copy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Callback<string, string, bool>((sourcePath, destinationPath, overwrite) =>
            {
                var destSection = destinationPath.Split(new string[] { "Projects" }, StringSplitOptions.RemoveEmptyEntries);

                actual = destSection.Length <= 1 ?
                    "" :
                    destSection[1];
            });

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.AddParticle("test-project", "test-setup", @"C:\temp\test-particle.png");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AddParticle_WhenInvokedWithNoExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.AddParticle("test-project", "test-setup", "");
            });
        }


        [Test]
        public void AddParticle_WhenInvokedWithNullProjectName_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.AddParticle(null, "test-setup", "");
            });
        }


        [Test]
        public void AddParticle_WhenInvokedWithNoExistingSetup_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.AddParticle("test-project", "test-setup", "");
            });
        }


        [Test]
        public void AddParticle_WhenInvokedWithNullSetupName_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.AddParticle("test-project", null, "");
            });
        }
        #endregion
    }
}

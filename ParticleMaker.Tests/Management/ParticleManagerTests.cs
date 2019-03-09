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
            mockFileService.Verify(m => m.Copy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
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


        [Test]
        public void RenameParticle_WhenInvoked_RenamesParticle()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.RenameParticle("test-project", "test-setup", "test-particle", "new-particle");

            //Assert
            mockFileService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void RenameParticle_WhenInvoked_BuildsCorrectDestinationPath()
        {
            //Arrange
            var expected = @"\test-project\Setups\test-setup\test-particle.png";
            var actual = string.Empty;

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockFileService.Setup(m => m.Rename(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((path, newName) =>
            {
                var destSection = path.Split(new string[] { "Projects" }, StringSplitOptions.RemoveEmptyEntries);

                actual = destSection.Length <= 1 ?
                    "" :
                    destSection[1];
            });

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.RenameParticle("test-project", "test-setup", "test-particle", "new-particle");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RenameParticle_WhenInvokedWithNoExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.RenameParticle("test-project", "test-setup", "test-particle", "new-particle");
            });
        }


        [Test]
        public void RenameParticle_WhenInvokedWithNullProjectName_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.RenameParticle(null, "test-setup", "test-particle", "new-particle");
            });
        }


        [Test]
        public void RenameParticle_WhenInvokedWithNoExistingSetup_ThrowsException()
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
                manager.RenameParticle("test-project", "test-setup", "test-particle", "new-particle");
            });
        }


        [Test]
        public void RenameParticle_WhenInvokeWithNullSetupName_ThrowsException()
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
                manager.RenameParticle("test-project", null, "test-particle", "new-particle");
            });
        }


        [Test]
        public void DeleteParticle_WhenInvoked_DeletesParticle()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.DeleteParticle("test-project", "test-setup", "test-particle");

            //Assert
            mockFileService.Verify(m => m.Delete(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void DeleteParticle_WhenInvoked_BuildsCorrectDestinationPath()
        {
            //Arrange
            var expected = @"\test-project\Setups\test-setup\test-particle.png";
            var actual = string.Empty;

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockFileService.Setup(m => m.Delete(It.IsAny<string>())).Callback<string>((path) =>
            {
                var destSection = path.Split(new string[] { "Projects" }, StringSplitOptions.RemoveEmptyEntries);

                actual = destSection.Length <= 1 ?
                    "" :
                    destSection[1];
            });

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.DeleteParticle("test-project", "test-setup", "test-particle");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DeleteParticle_WhenInvokedWithNoExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.DeleteParticle("test-project", "test-setup", "test-particle");
            });
        }


        [Test]
        public void DeleteParticle_WhenInvokedWithNullProjectName_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.DeleteParticle(null, "test-setup", "test-particle");
            });
        }


        [Test]
        public void DeleteParticle_WhenInvokedWithNoExistingSetup_ThrowsException()
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
                manager.DeleteParticle("test-project", "test-setup", "test-particle");
            });
        }


        [Test]
        public void DeleteParticle_WhenInvokedWithNullSetupName_ThrowsException()
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
                manager.DeleteParticle("test-project", null, "test-particle");
            });
        }
        #endregion
    }
}

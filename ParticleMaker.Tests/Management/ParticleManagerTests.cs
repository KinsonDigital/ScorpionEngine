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
        #region Fields
        private Mock<IDirectoryService> _mockProjIODirService;
        private Mock<IFileService> _mockProjIOFileService;
        private ProjectIOService _projIOService;
        #endregion


        #region Method Tests
        [Test]
        public void AddParticle_WhenInvoked_AddsParticleToSetup()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.AddParticle("test-project", "test-setup", "");

            //Assert
            mockFileService.Verify(m => m.Copy(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once());
        }


        [Test]
        public void AddParticle_WhenInvoked_BuildsCorrectDestinationPath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

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

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.RenameParticle("test-project", "test-setup", "test-particle", "new-particle");

            //Assert
            mockFileService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void RenameParticle_WhenInvoked_BuildsCorrectDestinationPath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

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

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.DeleteParticle("test-project", "test-setup", "test-particle");

            //Assert
            mockFileService.Verify(m => m.Delete(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void DeleteParticle_WhenInvoked_BuildsCorrectDestinationPath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

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

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

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
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.DeleteParticle("test-project", null, "test-particle");
            });
        }
        

        [Test]
        public void GetParticlePaths_WhenInvoked_ReturnsListOfParticlePaths()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockDirectoryService.Setup(m => m.GetFiles(It.IsAny<string>())).Returns(new string[]
            {
                @"C:\temp\particle1.png",
                @"C:\temp\particle2.png"
            });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act
            var actual = manager.GetParticlePaths("test-project", "test-setup");

            //Assert
            mockDirectoryService.Verify(m => m.GetFiles(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void GetParticlePaths_WhenInvoked_BuildsCorrectDestinationPath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\Setups\test-setup";
            var actual = string.Empty;
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockDirectoryService.Setup(m => m.GetFiles(It.IsAny<string>())).Returns<string>((path) =>
            {
                var sections = path.Split(new string[] { "Projects" }, StringSplitOptions.RemoveEmptyEntries);

                actual = sections.Length < 2 ?
                    "" :
                    sections[1];

                return new string[]
                {
                    @"C:\temp\particle1.png",
                    @"C:\temp\particle2.png"
                };
            });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.GetParticlePaths("test-project", "test-setup");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetParticlePaths_WhenInvokedWithNoExistingProject_ThrowsException()
        {
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.GetParticlePaths("test-project", It.IsAny<string>());
            });
        }


        [Test]
        public void GetParticlePaths_WhenInvokedWithNullProjectName_ThrowsException()
        {
            //Arrange
            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.GetParticlePaths(null, It.IsAny<string>());
            });
        }


        [Test]
        public void GetParticlePaths_WhenInvokedWithNoExistingSetup_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.GetParticlePaths("test-project", "test-setup");
            });
        }


        [Test]
        public void GetParticlePaths_WhenInvokedWithNullSetupName_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.GetParticlePaths("test-project", null);
            });
        }
        #endregion


        #region SetUp & TearDown
        [SetUp]
        public void Setup()
        {
            _mockProjIODirService = new Mock<IDirectoryService>();
            _mockProjIOFileService = new Mock<IFileService>();
            _projIOService = new ProjectIOService(_mockProjIODirService.Object, _mockProjIOFileService.Object);
        }


        [TearDown]
        public void TearDown()
        {
            _mockProjIODirService = null;
            _mockProjIOFileService = null;
            _projIOService = null;
        }
        #endregion
    }
}

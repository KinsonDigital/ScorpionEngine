using System;
using Moq;
using Xunit;
using ParticleMaker.Exceptions;
using ParticleMaker.Management;
using ParticleMaker.Services;
using System.Collections.Generic;

namespace ParticleMaker.Tests.Management
{
    /// <summary>
    /// Unit tests to test the <see cref="ParticleManager"/> class.
    /// </summary>
    public class ParticleManagerTests : IDisposable
    {
        #region Private Fields
        private Mock<IDirectoryService> _mockProjIODirService;
        private Mock<IFileService> _mockProjIOFileService;
        private ProjectIOService _projIOService;
        #endregion


        #region Constructors
        public ParticleManagerTests()
        {
            _mockProjIODirService = new Mock<IDirectoryService>();
            _mockProjIOFileService = new Mock<IFileService>();
            _projIOService = new ProjectIOService(_mockProjIODirService.Object, _mockProjIOFileService.Object);
        }
        #endregion


        #region Method Tests
        [Fact]
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


        [Fact]
        public void AddParticle_WhenInvoked_BuildsCorrectParticlePath()
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
        public void RenameParticle_WhenInvoked_BuildsCorrectParticlePath()
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
        public void DeleteParticle_WhenInvoked_BuildsCorrectParticlePath()
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
            Assert.Equal(expected, actual);
        }


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
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


        //[Fact]
        [Theory]
        [MemberData(nameof(ParticlePathData))]
        public void GetParticlePaths_WhenInvoked_ReturnsListOfParticlePaths(string particlePath)
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            _mockProjIOFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirectoryService = new Mock<IDirectoryService>();
            mockDirectoryService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockDirectoryService.Setup(m => m.GetFiles(It.IsAny<string>())).Returns(new string[] { particlePath });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act
            var actual = manager.GetParticlePaths("test-project", "test-setup");

            //Assert
            mockDirectoryService.Verify(m => m.GetFiles(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void GetParticlePaths_WhenInvoked_BuildsCorrectParticlePath()
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

                return new string[0];
            });

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new ParticleManager(_projIOService, mockDirectoryService.Object, mockFileService.Object);

            //Act
            manager.GetParticlePaths("test-project", "test-setup");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
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


        [Fact]
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


        [Fact]
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


        [Fact]
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


        #region Test Data
        public static IEnumerable<object[]> ParticlePathData => new List<object[]>
        {
            new object[] { @"C:\temp\particle1.png" },
            new object[] { @"C:\temp\particle2" },
            new object[] { "" }
        };
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _mockProjIODirService = null;
            _mockProjIOFileService = null;
            _projIOService = null;
        }
        #endregion
    }
}

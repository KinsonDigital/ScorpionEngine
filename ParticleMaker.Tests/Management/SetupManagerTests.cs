using System;
using Moq;
using Xunit;
using ParticleMaker.Exceptions;
using ParticleMaker.Management;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Management
{
    public class SetupManagerTests : IDisposable
    {
        #region Fields
        private Mock<IDirectoryService> _mockProjIODirService;
        private Mock<IFileService> _mockProjIOFileService;
        private ProjectIOService _projIOService;
        #endregion


        #region Constructors
        public SetupManagerTests()
        {
            _mockProjIODirService = new Mock<IDirectoryService>();
            _mockProjIOFileService = new Mock<IFileService>();

            _projIOService = new ProjectIOService(_mockProjIODirService.Object, _mockProjIOFileService.Object);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void GetSetupNames_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.GetSetupNames(It.IsAny<string>());
            });
        }


        [Fact]
        public void GetSetupNames_WhenInvoking_ReturnsCorrectNames()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockDirService.Setup(m => m.GetDirectories(It.IsAny<string>())).Returns<string>((path) =>
            {
                return new[]
                {
                    "setup-A",
                    "setup-B",
                    "setup-C"
                };
            });

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            var expected = new[]
            {
                    "setup-A",
                    "setup-B",
                    "setup-C"
            };

            //Act
            var actual = manager.GetSetupNames("test-project");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetSetupNames_WhenInvoked_BuildsCorrectProjectPath()
        {
            //Arrange
            var expected = "\\test-project";
            var actual = "";

            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockDirService.Setup(m => m.GetDirectories(It.IsAny<string>())).Returns<string>((folder) =>
            {
                var sections = folder.Split(new string[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length >= 2 ? sections[1] : "";


                return new string[0];
            });

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.GetSetupNames("test-project");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetSetupPaths_WhenInvoking_ReturnsCorrectPaths()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var setupPathFolder = string.Empty;

            mockDirService.Setup(m => m.GetDirectories(It.IsAny<string>())).Returns<string>((folder) => 
            {
                setupPathFolder = folder;

                return new[]
                {
                    $@"{folder}\setup-A",
                    $@"{folder}\setup-B",
                    $@"{folder}\setup-C",
                };
            });

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act
            var actual = manager.GetSetupPaths("project-A");
            var expected = new[]
            {
                $@"{setupPathFolder}\setup-A",
                $@"{setupPathFolder}\setup-B",
                $@"{setupPathFolder}\setup-C",
            };

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetSetupPaths_WhenInvoking_ChecksIfSetupsFolderExists()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act
            manager.GetSetupPaths("project-A");

            //Assert
            _mockProjIODirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void GetSetupPaths_WhenInvokingWithNoSetupsFolder_CreatesProjectSetupsFolder()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act
            manager.GetSetupPaths("project-A");

            //Assert
            _mockProjIODirService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Create_WhenInvoking_CreatesSetupFile()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Create("test-project", "test-setup");

            //Assert
            mockFileService.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<ParticleSetup>()), Times.Once());
        }


        [Fact]
        public void Create_WhenInvoking_BuildsCorrectSetupPath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\Setups\test-setup\test-setup.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((path, data) =>
            {
                var sections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Create("test-project", "test-setup");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Create_WhenInvoking_ChecksIfProjectSetupsFolderExists()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Create("test-project", "test-setup");

            //Assert
            _mockProjIODirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void Create_WhenInvokingWithNoProjectSetupsFolder_CreatesProjectSetupFolder()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var existsInvokeCount = 0;

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns<string>((folder) =>
            {
                existsInvokeCount += 1;

                return existsInvokeCount <= 1;
            });

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Create("test-project", "test-setup");

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(1));
        }


        [Fact]
        public void Create_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() => false);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.Create("test-project", "test-setup");
            });
        }


        [Fact]
        public void Create_WhenInvokingWithIllegalSetupName_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<IllegalParticleSetupNameException>(() =>
            {
                manager.Create("test-project", "**test|setup**");
            });
        }


        [Fact]
        public void Create_WhenInvokingWithAlreadyExistingSetupFile_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupAlreadyExistsException>(() =>
            {
                manager.Create("test-project", "test-setup");
            });
        }


        [Fact]
        public void Load_WhenInvoking_BuildsCorrectSetupPath()
        {
            //Arrange
            var expected = "\\test-project\\Setups\\setup-A\\setup-A.json";
            var actual = string.Empty;

            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<ParticleSetup>(It.IsAny<string>())).Returns<string>((path) =>
            {
                var pathSections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = pathSections.Length >= 2 ? pathSections[1] : "";


                return null;
            });

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);
            
            //Act
            manager.Load("test-project", "setup-A");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Load_WhenInvoked_InvokesFileServiceLoadMethod()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<ParticleSetup>(It.IsAny<string>()));

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            var actual = manager.Load("test-project", It.IsAny<string>());

            //Assert
            mockFileService.Verify(m => m.Load<ParticleSetup>(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Load_WhenInvokedWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.Load(It.IsAny<string>(), It.IsAny<string>());
            });
        }


        [Fact]
        public void Load_WhenInvokedWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.Load("test-project", It.IsAny<string>());
            });
        }


        [Fact]
        public void Load_WhenInvoked_ChecksIfProjectSetupsFolderExists()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Load("test-project", "test-setup");

            //Assert
            _mockProjIODirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void Load_WhenInvokedWithNoProjectSetupsFolder_CreatesProjectSetupsFolder()
        {
            //Arrange
            var invokeCount = 0;

            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns<string>((path) =>
            {
                invokeCount += 1;

                return invokeCount <= 1;
            });

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Load("test-project", "test-setup");

            //Assert
            _mockProjIODirService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Save_WhenInvoking_SavesSetupFile()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);
            var setup = new ParticleSetup();

            //Act
            manager.Save("test-project", "test-setup", setup);

            //Assert
            mockFileService.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<ParticleSetup>()), Times.Once());
        }


        [Fact]
        public void Save_WhenInvoking_BuildsCorrectSetupPath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\Setups\test-setup\test-setup.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Save(It.IsAny<string>(), It.IsAny<ParticleSetup>())).Callback<string, object>((path, data) =>
            {
                var sections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);
            
            //Act
            manager.Save("test-project", "test-setup", It.IsAny<ParticleSetup>());

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Save_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.Save(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ParticleSetup>());
            });
        }


        [Fact]
        public void Save_WhenInvokingWithIllegalSetupName_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws<IllegalParticleSetupNameException>(() =>
            {
                manager.Save("test-project", "**test|setup**", It.IsAny<ParticleSetup>());
            });
        }


        [Fact]
        public void Save_WhenInvoked_ChecksIfProjectSetupsFolderExists()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Save("test-project", "test-setup", It.IsAny<ParticleSetup>());

            //Assert
            _mockProjIODirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void Save_WhenInvokedWithNoProjectSetupsFolder_CreatesProjectSetupsFolder()
        {
            //Arrange
            var invokeCount = 0;
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns<string>((path) =>
            {
                invokeCount += 1;

                return invokeCount <= 1;
            });
            
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Save("test-project", "test-setup", It.IsAny<ParticleSetup>());

            //Assert
            _mockProjIODirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(1));
        }


        [Fact]
        public void Rename_WhenInvoking_RenamesSetupFile()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", "old-setup-name", "new-setup-name");

            //Assert
            mockFileService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
            mockFileService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            mockDirService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Rename_WhenInvoking_BuildsCorrectSetupPath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\Setups\old-setup-name\old-setup-name.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Rename(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((setupPath, newName) =>
            {
                var sections = setupPath.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", "old-setup-name", "new-setup-name");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Rename_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(_projIOService, mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.Rename(It.IsAny<string>(), It.IsAny<string>(), "new-setup-name");
            });
        }


        [Fact]
        public void Rename_WhenInvokingWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.Rename("test-project", It.IsAny<string>(), "new-setup-name");
            });
        }


        [Fact]
        public void Rename_WhenInvokingWithSetupName_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<IllegalParticleSetupNameException>(() =>
            {
                manager.Rename("test-project", "old-setup-name", "**new-setup-name**");
            });
        }


        [Fact]
        public void Rename_WhenInvoked_ChecksIfProjectSetupsFolderExists()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", "test-setup", "new-setup");

            //Assert
            _mockProjIODirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void Rename_WhenInvokedWithNoProjectSetupsFolder_CreatesProjectSetupsFolder()
        {
            //Arrange
            var invokeCount = 0;
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns<string>((path) =>
            {
                invokeCount += 1;

                return invokeCount <= 1;
            });

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", "test-setup", "new-setup");

            //Assert
            _mockProjIODirService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Delete_WhenInvoking_DeletesSetup()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Delete("test-project", "test-setup");

            //Assert
            mockFileService.Verify(m => m.Delete(It.IsAny<string>()), Times.Once());
            mockDirService.Verify(m => m.Delete(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Delete_WhenInvoking_BuildsCorrectDeletePath()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\Setups\test-setup\test-setup.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockFileService.Setup(m => m.Delete(It.IsAny<string>())).Callback<string>((path) =>
            {
                var sections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Delete("test-project", "test-setup");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Delete_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() => false);

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.Delete("test-project", "test-setup");
            });
        }


        [Fact]
        public void Delete_WhenInvokingWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ParticleSetupDoesNotExistException>(() =>
            {
                manager.Delete("test-project", "test-setup");
            });
        }


        [Fact]
        public void Delete_WhenInvoked_ChecksIfProjectSetupsFolderExists()
        {
            //Arrange
            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Delete("test-project", "test-setup");

            //Assert
            _mockProjIODirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void Delete_WhenInvokedWithNoProjectSetupsFolder_CreatesProjectSetupsFolder()
        {
            //Arrange
            var invokeCount = 0;

            _mockProjIODirService.Setup(m => m.Exists(It.IsAny<string>())).Returns<string>((path) =>
            {
                invokeCount += 1;

                return invokeCount <= 1;
            });

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(_projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Delete("test-project", "test-setup");

            //Assert
            _mockProjIODirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(1));
        }
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

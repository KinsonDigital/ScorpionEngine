using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Management;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Management
{
    [TestFixture]
    public class ProjectManagerTests
    {
        #region Fields
        private Mock<IDirectoryService> _mockProjDirService;
        private Mock<IFileService> _mockProjFileService;
        private ProjectIOService _projIOService;
        #endregion


        #region Prop Tests
        [Test]
        public void Projects_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.GetDirectories(It.IsAny<string>())).Returns(() =>
            {
                return new[]
                {
                    @"my-dir-a",
                    @"my-dir-b",
                    @"my-dir-c",
                };
            });

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //The expectation is to return the last directory in the path, not the entire path.
            var expected = new[]
            {
                @"my-dir-a",
                @"my-dir-b",
                @"my-dir-c",
            };

            //Act
            var actual = manager.Projects;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ProjectPaths_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.GetDirectories(It.IsAny<string>())).Returns(() =>
            {
                return new[]
                {
                    @"C:\temp\projects\my-dir-a",
                    @"C:\temp\projects\my-dir-b",
                    @"C:\temp\projects\my-dir-c",
                };
            });

            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, mockDirService.Object);
            var expected = new []
            {
                @"C:\temp\projects\my-dir-a",
                @"C:\temp\projects\my-dir-b",
                @"C:\temp\projects\my-dir-c",
            };

            //Act
            var actual = manager.ProjectPaths;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ProjectPaths_WhenGettingValue_InvokesDirectoryServiceExistsMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object);

            //Act
            var actual = manager.ProjectPaths;

            //Assert
            _mockProjDirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Test]
        public void ProjectPaths_WhenGettingValueWhileDirectoryDoesNotExist_InvokesDirectoryServiceCreateMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object);

            //Act
            var actual = manager.ProjectPaths;

            //Assert
            _mockProjDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(2));
        }


        [Test]
        public void Projects_WhenGettingValue_InvokesDirectoryServiceExistsMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object);

            //Act
            var actual = manager.Projects;

            //Assert
            _mockProjDirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Test]
        public void Projects_WhenGettingValueWhileDirectoryDoesNotExist_InvokesDirectoryServiceCreateMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object);

            //Act
            var actual = manager.Projects;

            //Assert
            _mockProjDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(2));
        }
        #endregion


        #region Method Tests
        [Test]
        public void Create_WhenInvokingWithAlreadyExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectAlreadyExistsException), () =>
            {
                manager.Create(It.IsAny<string>());
            });
        }


        [Test]
        public void Create_WhenInvokingWithIllegalProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                manager.Create("**new|name**");
            });
        }


        [Test]
        public void Create_WhenInvokingWithEmptyParam_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                service.Create("");
            });
        }


        [Test]
        public void Create_WhenInvokingWithARootProjectsFolder_CreatesProjectFolder()
        {
            //Arrange
            var existsInvokeCount = 0;
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() =>
            {
                existsInvokeCount += 1;

                return existsInvokeCount == 2;
            });

            mockDirService.Setup(m => m.Create(It.IsAny<string>()));

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<ProjectSettings>()));

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act
            manager.Create("test-project");

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.AtLeastOnce());
        }


        [Test]
        public void Delete_WhenInvokedWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Delete("test-project");
            });
        }


        [Test]
        public void Delete_WhenInvokedWithExistingProject_DeletesProject()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act
            manager.Delete(It.IsAny<string>());

            //Assert
            mockDirService.Verify(m => m.Delete(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Rename_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Rename("old-name", "new-name");
            });
        }


        [Test]
        public void Rename_WhenInvokingWithIllegalProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                manager.Rename("old-name", "**new|name**");
            });
        }


        [Test]
        public void Rename_WhenInvokingWithNullProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var anager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                anager.Rename("old-name", null);
            });
        }


        [Test]
        public void Rename_WhenInvokedWithExistingProject_RenamesProject()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            //Act
            manager.Rename(It.IsAny<string>(), "test-project");

            //Assert
            mockDirService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Exists_WhenInvokingWithExistingDirectory_ReturnsTrue()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockDirService.Object, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object);

            var expected = true;

            //Act
            var actual = manager.Exists("test-project");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region SetUp & TearDown
        [SetUp]
        public void SetUp()
        {
            _mockProjDirService = new Mock<IDirectoryService>();
            _mockProjFileService = new Mock<IFileService>();

            _projIOService = new ProjectIOService(_mockProjDirService.Object, _mockProjFileService.Object);
        }


        [TearDown]
        public void TearDown()
        {
            _mockProjDirService = null;
            _mockProjFileService = null;
            _projIOService = null;
        }
        #endregion
    }
}

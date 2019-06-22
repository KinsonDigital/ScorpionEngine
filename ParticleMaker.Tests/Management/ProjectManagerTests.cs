using System;
using System.Linq;
using Moq;
using Xunit;
using ParticleMaker.Exceptions;
using ParticleMaker.Management;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Management
{
    public class ProjectManagerTests : IDisposable
    {
        #region Fields
        private Mock<IDirectoryService> _mockProjDirService;
        private Mock<IFileService> _mockProjFileService;
        private ProjectIOService _projIOService;
        #endregion


        #region Constructors
        public ProjectManagerTests()
        {
            _mockProjDirService = new Mock<IDirectoryService>();
            _mockProjFileService = new Mock<IFileService>();

            _projIOService = new ProjectIOService(_mockProjDirService.Object, _mockProjFileService.Object);
        }
        #endregion


        #region Prop Tests
        [Fact]
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

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

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
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ProjectPaths_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var valueProjPaths = new[]
            {
                @"C:\temp\projects\my-dir-a\my-dir-a.projs",
                @"C:\temp\projects\my-dir-b\my-dir-b.projs",
                @"C:\temp\projects\my-dir-c\my-dir-c.projs",
            };

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

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns<string>((path) =>
            {
                return valueProjPaths.Contains(path);
            });
                
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, mockDirService.Object, mockFileService.Object);

            var expected = new []
            {
                @"C:\temp\projects\my-dir-a",
                @"C:\temp\projects\my-dir-b",
                @"C:\temp\projects\my-dir-c",
            };

            //Act
            var actual = manager.ProjectPaths;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ProjectPaths_WhenGettingValue_InvokesDirectoryServiceExistsMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object, new Mock<IFileService>().Object);

            //Act
            var actual = manager.ProjectPaths;

            //Assert
            _mockProjDirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void ProjectPaths_WhenGettingValueWhileDirectoryDoesNotExist_InvokesDirectoryServiceCreateMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object, new Mock<IFileService>().Object);

            //Act
            var actual = manager.ProjectPaths;

            //Assert
            _mockProjDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void Projects_WhenGettingValue_InvokesDirectoryServiceExistsMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object, new Mock<IFileService>().Object);

            //Act
            var actual = manager.Projects;

            //Assert
            _mockProjDirService.Verify(m => m.Exists(It.IsAny<string>()), Times.Exactly(2));
        }


        [Fact]
        public void Projects_WhenGettingValueWhileDirectoryDoesNotExist_InvokesDirectoryServiceCreateMethod()
        {
            //Arrange
            var manager = new ProjectManager(It.IsAny<ProjectSettingsManager>(), _projIOService, new Mock<IDirectoryService>().Object, new Mock<IFileService>().Object);

            //Act
            var actual = manager.Projects;

            //Assert
            _mockProjDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(2));
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Create_WhenInvokingWithAlreadyExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectAlreadyExistsException), () =>
            {
                manager.Create(It.IsAny<string>());
            });
        }


        [Fact]
        public void Create_WhenInvokingWithIllegalProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                manager.Create("**new|name**");
            });
        }


        [Fact]
        public void Create_WhenInvokingWithEmptyParam_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var projSettingsManager = new ProjectSettingsManager(_projIOService, new Mock<IFileService>().Object);

            var service = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, new Mock<IFileService>().Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                service.Create("");
            });
        }


        [Fact]
        public void Create_WhenInvokingWithARootProjectsFolder_CreatesProjectFolder()
        {
            //Arrange
            _mockProjDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

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

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Create("test-project");

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.AtLeastOnce());
        }


        [Fact]
        public void Delete_WhenInvokedWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Delete("test-project");
            });
        }


        [Fact]
        public void Delete_WhenInvokedWithExistingProject_DeletesProject()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Delete(It.IsAny<string>());

            //Assert
            mockDirService.Verify(m => m.Delete(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Rename_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<ProjectDoesNotExistException>(() =>
            {
                manager.Rename("old-name", "new-name");
            });
        }


        [Fact]
        public void Rename_WhenInvokingWithIllegalProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws<IllegalProjectNameException>(() =>
            {
                manager.Rename("old-name", "**new|name**");
            });
        }


        [Fact]
        public void Rename_WhenInvokingWithNullProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var projSettingsManager = new ProjectSettingsManager(_projIOService, new Mock<IFileService>().Object);

            var anager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, new Mock<IFileService>().Object);

            //Act & Assert
            Assert.Throws<IllegalProjectNameException>(() =>
            {
                anager.Rename("old-name", null);
            });
        }


        [Fact]
        public void Rename_WhenInvokedWithExistingProject_RenamesProject()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename(It.IsAny<string>(), "test-project");

            //Assert
            mockDirService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Exists_WhenInvokingWithExistingDirectory_ReturnsTrue()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsManager = new ProjectSettingsManager(_projIOService, mockFileService.Object);

            var manager = new ProjectManager(projSettingsManager, _projIOService, mockDirService.Object, mockFileService.Object);

            var expected = true;

            //Act
            var actual = manager.Exists("test-project");

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region SetUp & TearDown
        public void Dispose()
        {
            _mockProjDirService = null;
            _mockProjFileService = null;
            _projIOService = null;
        }
        #endregion
    }
}

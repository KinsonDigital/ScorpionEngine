using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Project;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Project
{
    [TestFixture]
    public class ProjectManagerTests
    {
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

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //The expectation is to return the last directory in the path, not the entire path.
            var expected = new[]
            {
                @"my-dir-a",
                @"my-dir-b",
                @"my-dir-c",
            };

            //Act
            var actual = service.Projects;

            //Assert
            Assert.AreEqual(expected, actual);
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

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectAlreadyExistsException), () =>
            {
                service.Create(It.IsAny<string>());
            });
        }


        [Test]
        public void Create_WhenInvokingWithIllegalProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                service.Create("**new|name**");
            });
        }


        [Test]
        public void Create_WhenInvokingWithEmptyParam_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

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
            var mockDirServiceA = new Mock<IDirectoryService>();
            mockDirServiceA.Setup(m => m.Exists(It.IsAny<string>())).Returns(() =>
            {
                existsInvokeCount += 1;

                return existsInvokeCount == 1 || existsInvokeCount == 3;
            });

            mockDirServiceA.Setup(m => m.Create(It.IsAny<string>()));

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<ProjectSettings>()));

            var projSettingsService = new ProjectSettingsManager(mockDirServiceA.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirServiceA.Object);

            //Act
            service.Create("test-project");

            //Assert
            mockDirServiceA.Verify(m => m.Create(It.IsAny<string>()), Times.AtLeastOnce());
        }


        [Test]
        public void Create_WhenInvokingWithNoRootProjectsFolder_CreatesMainProjectsFolder()
        {
            //Arrange
            var existsInvokeCount = 0;
            var mockDirServiceA = new Mock<IDirectoryService>();
            mockDirServiceA.Setup(m => m.Exists(It.IsAny<string>())).Returns(() =>
            {
                existsInvokeCount += 1;

                return existsInvokeCount == 3;
            });
            mockDirServiceA.Setup(m => m.Create(It.IsAny<string>()));

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<ProjectSettings>()));

            var projSettingsService = new ProjectSettingsManager(mockDirServiceA.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirServiceA.Object);

            //Act
            service.Create("test-project");

            //Assert
            mockDirServiceA.Verify(m => m.Create(It.IsAny<string>()), Times.AtLeastOnce());
        }


        [Test]
        public void Delete_WhenInvokedWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                service.Delete("test-project");
            });
        }


        [Test]
        public void Delete_WhenInvokedWithExistingProject_DeletesProject()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act
            service.Delete(It.IsAny<string>());

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

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                service.Rename("old-name", "new-name");
            });
        }


        [Test]
        public void Rename_WhenInvokingWithIllegalProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                service.Rename("old-name", "**new|name**");
            });
        }


        [Test]
        public void Rename_WhenInvokingWithNullProjectName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalProjectNameException), () =>
            {
                service.Rename("old-name", null);
            });
        }


        [Test]
        public void Rename_WhenInvokedWithExistingProject_RenamesProject()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            //Act
            service.Rename(It.IsAny<string>(), "test-project");

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

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            var service = new ProjectManager(projSettingsService, mockDirService.Object);

            var expected = true;

            //Act
            var actual = service.Exists("test-project");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

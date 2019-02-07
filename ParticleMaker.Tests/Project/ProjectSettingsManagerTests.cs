using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Project;
using ParticleMaker.Services;
using System;

namespace ParticleMaker.Tests.Project
{
    [TestFixture]
    public class ProjectSettingsManagerTests
    {
        #region Method Tests
        [Test]
        public void Save_WhenInvoking_CreatesSettingsFile()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);
            var settings = new ProjectSettings() { ProjectName = "test-project" };

            //Act
            settingsManager.Save("test-project", settings);

            //Assert
            mockFileService.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<ProjectSettings>()), Times.Once());
        }


        [Test]
        public void Save_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
               settingsManager.Save(It.IsAny<string>(), It.IsAny<ProjectSettings>());
            });
        }


        [Test]
        public void Save_WhenInvokingWithEmptyProjectName_ThrowsException()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);
            var settings = new ProjectSettings() { ProjectName = string.Empty };

            //Act & Assert
            Assert.Throws(typeof(ArgumentException), () =>
            {
                settingsManager.Save(It.IsAny<string>(), settings);
            });
        }


        [Test]
        public void Save_WhenInvokingWithEmptyProjectName_ThrowsExceptionWithCorrectMessage()
        {
            //Arrange
            var expected = $"The {nameof(ProjectSettings)}.{nameof(ProjectSettings.ProjectName)} cannot be empty or null.\r\nParameter name: settings";

            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);
            var settings = new ProjectSettings() { ProjectName = string.Empty };

            //Act & Assert
            try
            {
                projSettingsService.Save(It.IsAny<string>(), settings);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }


        [Test]
        public void Save_WhenInvokingWithProjectNameWithIllegalCharacters_ThrowsException()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);
            var settings = new ProjectSettings() { ProjectName = "<illegalname>" };

            //Act & Assert
            Assert.Throws(typeof(IllegalFileNameCharactersException), () =>
            {
                settingsManager.Save("test-project", settings);
            });
        }


        [Test]
        public void Load_WhenInvoking_ReturnsData()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act
            settingsManager.Load("test-project");

            //Assert
            mockFileService.Verify(m => m.Load<ProjectSettings>(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Load_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                settingsManager.Load(It.IsAny<string>());
            });
        }
        #endregion
    }
}

using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Project;
using ParticleMaker.Services;

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

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act
            projSettingsService.Save("test-project", It.IsAny<ProjectSettings>());

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

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
               projSettingsService.Save(It.IsAny<string>(), It.IsAny<ProjectSettings>());
            });
        }


        [Test]
        public void Load_WhenInvoking_ReturnsData()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act
            projSettingsService.Load("test-project");

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

            var projSettingsService = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                projSettingsService.Load(It.IsAny<string>());
            });
        }
        #endregion
    }
}

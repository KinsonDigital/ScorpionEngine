using Moq;
using NUnit.Framework;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class ProjectIOServiceTests
    {
        #region Method Tests
        [Test]
        public void CheckRootProjectsFolder_WhenInvokingWhenDirectoryExists_DoesNotCreateFolder()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var service = new ProjectIOService(mockDirService.Object, new Mock<IFileService>().Object);

            //Act
            service.CheckRootProjectsFolder();

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Never());
        }


        [Test]
        public void CheckRootProjectsFolder_WhenInvokingWhenDirectoryDoesNotExist_CreatesFolder()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var service = new ProjectIOService(mockDirService.Object, new Mock<IFileService>().Object);

            //Act
            service.CheckRootProjectsFolder();

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }
        #endregion
    }
}

using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        #region Method Tests
        [Test]
        public void CreateProject_WhenInvokingWithAlreadyExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var service = new ProjectService(mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectAlreadyExistsException), () =>
            {
                service.Create("test-project");
            });
        }


        [Test]
        public void CreateProject_WhenInvoking_CreatesProjectFolder()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var service = new ProjectService(mockDirService.Object);

            //Act
            service.Create("test-project");

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(2));
        }
        #endregion
    }
}

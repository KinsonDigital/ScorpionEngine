using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Project;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Project
{
    [TestFixture]
    public class SetupManagerTests
    {
        #region Method Tests
        [Test]
        public void Create_WhenInvoking_CreatesSetupFile()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act
            manager.Create("test-setup");

            //Assert
            mockFileService.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<ParticleSetup>()), Times.Once());
        }


        [Test]
        public void Create_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Create("test-setup");
            });
        }


        [Test]
        public void Create_WhenInvokingWithIllegalSetupName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act & Assert
            Assert.Throws(typeof(IllegalParticleSetupNameException), () =>
            {
                manager.Create("**test|setup**");
            });
        }


        [Test]
        public void Create_WhenInvokingWithAlreadyExistingSetupFile_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act & Assert
            Assert.Throws(typeof(ParticleSetupAlreadyExists), () =>
            {
                manager.Create("test-setup");
            });
        }


        [Test]
        public void Rename_WhenInvoking_RenamesSetupFile()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act
            manager.Rename("old-setup-name", "new-setup-name");

            //Assert
            mockFileService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
            mockFileService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Rename_WhenInvokingWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act & Assert
            Assert.Throws(typeof(ParticleSetupDoesNotExist), () =>
            {
                manager.Rename("old-setup-name", "new-setup-name");
            });
        }


        [Test]
        public void Rename_WhenInvokingWithSetupName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act & Assert
            Assert.Throws(typeof(IllegalParticleSetupNameException), () =>
            {
                manager.Rename("old-setup-name", "**new-setup-name**");
            });
        }
        #endregion
    }
}

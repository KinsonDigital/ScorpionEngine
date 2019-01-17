﻿using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        //TODO: Remove this.  Only used for testing.
        //[Test]
        public void MyTest()
        {
            var service = new DirectoryService();

            service.Rename(@"C:\TEMP\FolderRenameTest\MyFolder", "HerFolder");
        }


        #region Method Tests
        [Test]
        public void Create_WhenInvokingWithAlreadyExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var service = new ProjectService(mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectAlreadyExistsException), () =>
            {
                service.Create(It.IsAny<string>());
            });
        }


        [Test]
        public void Create_WhenInvoking_CreatesProjectFolder()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var service = new ProjectService(mockDirService.Object);

            //Act
            service.Create(It.IsAny<string>());

            //Assert
            mockDirService.Verify(m => m.Create(It.IsAny<string>()), Times.Exactly(2));
        }


        [Test]
        public void Delete_WhenInvokedWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var service = new ProjectService(mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistExistException), () =>
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

            var service = new ProjectService(mockDirService.Object);

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

            var service = new ProjectService(mockDirService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistExistException), () =>
            {
                service.Rename("old-name", "new-name");
            });
        }


        [Test]
        public void Rename_WhenInvokedWithExistingProject_RenamesProject()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var service = new ProjectService(mockDirService.Object);

            //Act
            service.Rename(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            mockDirService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }
        #endregion
    }
}

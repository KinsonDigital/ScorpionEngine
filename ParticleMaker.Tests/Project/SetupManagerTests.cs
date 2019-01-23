using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Project;
using ParticleMaker.Services;
using System;

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
        public void Create_WhenInvoking_UsesCorrectSetupPath()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\test-setup.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((path, data) =>
            {
                var sections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act
            manager.Create("test-setup");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Create_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var existsInvokeCount = 0;
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() =>
            {
                existsInvokeCount += 1;

                //If the number of times that the dir service exists() method has been invoked
                //is 1, then its the invoke from the constructor. Return true.  Else return false.
                return existsInvokeCount == 1;
            });

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
        public void Rename_WhenInvoking_UsesCorrectSetupPath()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\old-setup-name.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockFileService.Setup(m => m.Rename(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((setupPath, newName) =>
            {
                var sections = setupPath.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act
            manager.Rename("old-setup-name", "new-setup-name");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Rename_WhenInvokingWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

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


        [Test]
        public void Delete_WhenInvoking_DeletesSetup()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act
            manager.Delete("test-setup");

            //Assert
            mockFileService.Verify(m => m.Delete(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Delete_WhenInvoking_UsesCorrectDeletePath()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\test-setup.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockFileService.Setup(m => m.Delete(It.IsAny<string>())).Callback<string>((path) =>
            {
                var sections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act
            manager.Delete("test-setup");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Delete_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var existsInvokeCount = 0;
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() =>
            {
                existsInvokeCount += 1;

                //If the number of times that the dir service exists() method has been invoked
                //is 1, then its the invoke from the constructor. Return true.  Else return false.
                return existsInvokeCount == 1;
            });

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Delete("test-setup");
            });
        }


        [Test]
        public void Delete_WhenInvokingWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object, "test-project");

            //Act & Assert
            Assert.Throws(typeof(ParticleSetupDoesNotExist), () =>
            {
                manager.Delete("test-setup");
            });
        }
        #endregion
    }
}

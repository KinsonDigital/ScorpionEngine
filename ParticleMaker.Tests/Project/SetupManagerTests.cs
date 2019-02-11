﻿using KDParticleEngine;
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
        public void GetSetupNames_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(mockDirService.Object, It.IsAny<IFileService>());

            //Act

            //Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.GetSetupNames(It.IsAny<string>());
            });
        }


        [Test]
        public void GetSetupNames_WhenInvoking_ReturnsCorrectNames()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            mockDirService.Setup(m => m.GetDirectories(It.IsAny<string>())).Returns<string>((path) =>
            {
                return new[]
                {
                    "setup-A",
                    "setup-B",
                    "setup-C"
                };
            });

            var manager = new SetupManager(mockDirService.Object, It.IsAny<IFileService>());

            var expected = new[]
            {
                    "setup-A",
                    "setup-B",
                    "setup-C"
            };

            //Act
            var actual = manager.GetSetupNames("test-project");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SetupPaths_WhenInvoking_ReturnsCorrectPaths()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            var setupPathFolder = string.Empty;

            mockDirService.Setup(m => m.GetDirectories(It.IsAny<string>())).Returns<string>((folder) => 
            {
                setupPathFolder = folder;

                return new[]
                {
                    $@"{folder}\setup-A",
                    $@"{folder}\setup-B",
                    $@"{folder}\setup-C",
                };
            });

            var manager = new SetupManager(mockDirService.Object, It.IsAny<IFileService>());

            //Act
            var actual = manager.GetSetupPaths("project-A");
            var expected = new[]
            {
                $@"{setupPathFolder}\setup-A",
                $@"{setupPathFolder}\setup-B",
                $@"{setupPathFolder}\setup-C",
            };

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Create_WhenInvoking_CreatesSetupFile()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Create("test-project", "test-setup");

            //Assert
            mockFileService.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<ParticleSetup>()), Times.Once());
        }


        [Test]
        public void Create_WhenInvoking_UsesCorrectSetupPath()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\Setups\test-setup\test-setup.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((path, data) =>
            {
                var sections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Create("test-project", "test-setup");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Create_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() => false);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Create("test-project", "test-setup");
            });
        }


        [Test]
        public void Create_WhenInvokingWithIllegalSetupName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalParticleSetupNameException), () =>
            {
                manager.Create("test-project", "**test|setup**");
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

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ParticleSetupAlreadyExists), () =>
            {
                manager.Create("test-project", "test-setup");
            });
        }


        [Test]
        public void Load_WhenInvoking_BuildsCorrectSetupPath()
        {
            //Arrange
            var expected = "\\test-project\\Setups\\setup-A\\setup-A.json";
            var actual = string.Empty;

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<ParticleSetup>(It.IsAny<string>())).Returns<string>((path) =>
            {
                var pathSections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = pathSections.Length >= 2 ? pathSections[1] : "";


                return null;
            });

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);
            
            //Act
            manager.Load("test-project", "setup-A");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Load_WhenInvoked_InvokesFileServiceLoadMethod()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<ParticleSetup>(It.IsAny<string>()));

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act
            var actual = manager.Load("test-project", It.IsAny<string>());

            //Assert
            mockFileService.Verify(m => m.Load<ParticleSetup>(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Load_WhenInvokedWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Load(It.IsAny<string>(), It.IsAny<string>());
            });
        }


        [Test]
        public void Load_WhenInvokedWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ParticleSetupDoesNotExist), () =>
            {
                manager.Load("test-project", It.IsAny<string>());
            });
        }


        [Test]
        public void Save_WhenInvoking_SavesSetupFile()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);
            var setup = new ParticleSetup();

            //Act
            manager.Save("test-project", "test-setup", setup);

            //Assert
            mockFileService.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<ParticleSetup>()), Times.Once());
        }


        [Test]
        public void Save_WhenInvoking_UsesCorrectSetupPath()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var expected = @"\test-project\test-setup.json";
            var actual = "";

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Save(It.IsAny<string>(), It.IsAny<ParticleSetup>())).Callback<string, object>((path, data) =>
            {
                var sections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = sections.Length == 2 ? sections[1] : "";
            });

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);
            
            //Act
            manager.Save("test-project", "test-setup", It.IsAny<ParticleSetup>());

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Save_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Save(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ParticleSetup>());
            });
        }


        [Test]
        public void Save_WhenInvokingWithIllegalSetupName_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var manager = new SetupManager(mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws(typeof(IllegalParticleSetupNameException), () =>
            {
                manager.Save("test-project", "**test|setup**", It.IsAny<ParticleSetup>());
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

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", "old-setup-name", "new-setup-name");

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

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", "old-setup-name", "new-setup-name");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Rename_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Rename(It.IsAny<string>(), It.IsAny<string>(), "new-setup-name");
            });
        }


        [Test]
        public void Rename_WhenInvokingWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ParticleSetupDoesNotExist), () =>
            {
                manager.Rename("test-project", It.IsAny<string>(), "new-setup-name");
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

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(IllegalParticleSetupNameException), () =>
            {
                manager.Rename("test-project", "old-setup-name", "**new-setup-name**");
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

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Delete("test-project", "test-setup");

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

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Delete("test-project", "test-setup");

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Delete_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(() => false);

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Delete("test-project", "test-setup");
            });
        }


        [Test]
        public void Delete_WhenInvokingWithNonExistingSetup_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();

            var manager = new SetupManager(mockDirService.Object, mockFileService.Object);

            //Act & Assert
            Assert.Throws(typeof(ParticleSetupDoesNotExist), () =>
            {
                manager.Delete("test-project", "test-setup");
            });
        }
        #endregion
    }
}

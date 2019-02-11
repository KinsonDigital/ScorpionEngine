﻿using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Project;
using ParticleMaker.Services;
using System;
using System.Linq;

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
        public void Load_WhenInvoking_LoadsData()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Load<ProjectSettings>(It.IsAny<string>())).Returns(new ProjectSettings()
            {
                ProjectName = "test-project",
                SetupDeploySettings = new DeploymentSetting[]
                {
                    new DeploymentSetting()
                    {
                        SetupName = "setup-A",
                        DeployPath = ""
                    }
                }
            });

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act
            settingsManager.Load("test-project");

            //Assert
            mockFileService.Verify(m => m.Load<ProjectSettings>(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Load_WhenInvoking_CorrectlyBuildsPath()
        {
            //Arrange
            var fileToLoadPath = string.Empty;
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Load<ProjectSettings>(It.IsAny<string>())).Returns<string>((path) =>
            {
                fileToLoadPath = path;

                return null;
            });

            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var settingsManager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);
            var expected = new[]
            {
                "test-project",
                "test-project-project-settings.json"
            };

            //Act
            settingsManager.Load("test-project");

            var pathEndSection = fileToLoadPath.Split(new[] { "Projects" }, StringSplitOptions.None);

            //Get all of the path sections and only return items that are not empty
            var actual = pathEndSection.Length >= 2 ?
                (from s in pathEndSection[1].Split('\\') where !string.IsNullOrEmpty(s) select s).ToArray() :
                new string[0];

            //Assert
            Assert.AreEqual(expected, actual);
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


        [Test]
        public void Rename_WhenInvokingWithNonExistingProject_ThrowsException()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(false);

            var manager = new ProjectSettingsManager(mockDirService.Object, It.IsAny<IFileService>());

            //Act & Assert
            Assert.Throws(typeof(ProjectDoesNotExistException), () =>
            {
                manager.Rename(It.IsAny<string>(), It.IsAny<string>());
            });
        }


        [Test]
        public void Rename_WhenInvoking_InvokesFileServiceRenameMethod()
        {
            //Arrange
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            
            var manager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", It.IsAny<string>());

            //Assert
            mockFileService.Verify(m => m.Rename(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void Rename_WhenInvoking_BuildsCorrectOldPath()
        {
            //Arrange
            var expected = @"\test-project\test-project-project-settings.json";
            var actual = string.Empty;
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Rename(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((path, newName) =>
            {
                var pathSections = path.Split(new[] { "Projects" }, StringSplitOptions.None);

                actual = pathSections.Length >= 2 ? pathSections[1] : "";
            });

            var manager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("test-project", It.IsAny<string>());

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Rename_WhenInvoking_BuildsCorrectNewName()
        {
            //Arrange
            var expected = "new-project-project-settings.json";
            var actual = string.Empty;
            var mockDirService = new Mock<IDirectoryService>();
            mockDirService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);

            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Rename(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((path, newName) =>
            {
                actual = newName;
            });

            var manager = new ProjectSettingsManager(mockDirService.Object, mockFileService.Object);

            //Act
            manager.Rename("old-project", "new-project");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

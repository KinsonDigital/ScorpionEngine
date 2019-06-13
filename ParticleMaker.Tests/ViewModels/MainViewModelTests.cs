using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using NUnit.Framework;
using ParticleMaker.Management;
using ParticleMaker.Services;
using ParticleMaker.UserControls;
using ParticleMaker.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;

namespace ParticleMaker.Tests.ViewModels
{
    [TestFixture]
    public class MainViewModelTests
    {
        #region Fields
        private GraphicsEngine _engine;
        private MainViewModel _viewModel;
        #endregion


        #region Prop Tests
        [Test]
        public void ProjectSetups_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = new[]
            {
                new PathItem() { FilePath = @"C:\Temp\item-A.json" },
                new PathItem() { FilePath = @"C:\Temp\item-B.json" }
            };

            //Act
            _viewModel.ProjectSetups = new[]
            {
                new PathItem() { FilePath = @"C:\Temp\item-A.json" },
                new PathItem() { FilePath = @"C:\Temp\item-B.json" }
            };

            var actual = _viewModel.ProjectSetups;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SettingsChanged_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = true;

            //Act
            _viewModel.RedMin = 123;

            var actual = _viewModel.SettingsChanged;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void NewProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.NewProject;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void RenderSurface_WhenSettingGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = new Control();

            //Act
            _viewModel.RenderSurface = expected;
            var actual = _viewModel.RenderSurface;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TotalParticles_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 1234;

            //Act
            _viewModel.TotalParticlesAliveAtOnce = 1234;
            var actual = _viewModel.TotalParticlesAliveAtOnce;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TotalLivingParticles_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var expected = 2;

            //Act
            var actual = _viewModel.TotalLivingParticles;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TotalDeadParticles_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var expected = 2;

            //Act
            var actual = _viewModel.TotalDeadParticles;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RedMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.RedMin = 123;
            var actual = _viewModel.RedMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RedMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.RedMax = 123;
            var actual = _viewModel.RedMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GreenMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.GreenMin = 123;
            var actual = _viewModel.GreenMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GreenMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.GreenMax = 123;
            var actual = _viewModel.GreenMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BlueMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.BlueMin = 123;
            var actual = _viewModel.BlueMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BlueMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.BlueMax = 123;
            var actual = _viewModel.BlueMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SizeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SizeMin = 123;
            var actual = _viewModel.SizeMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SizeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SizeMax = 123;
            var actual = _viewModel.SizeMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngleMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngleMin = 123;
            var actual = _viewModel.AngleMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngleMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngleMax = 123;
            var actual = _viewModel.AngleMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularVelocityMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngularVelocityMin = 123;
            var actual = _viewModel.AngularVelocityMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularVelocityMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngularVelocityMax = 123;
            var actual = _viewModel.AngularVelocityMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityXMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityXMin = 123;
            var actual = _viewModel.VelocityXMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityXMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityXMax = 123;
            var actual = _viewModel.VelocityXMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityYMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityYMin = 123;
            var actual = _viewModel.VelocityYMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityYMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityYMax = 123;
            var actual = _viewModel.VelocityYMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LifetimeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.LifetimeMin = 123;
            var actual = _viewModel.LifetimeMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LifetimeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.LifetimeMax = 123;
            var actual = _viewModel.LifetimeMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SpawnRateMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SpawnRateMin = 123;
            var actual = _viewModel.SpawnRateMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SpawnRateMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SpawnRateMax = 123;
            var actual = _viewModel.SpawnRateMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UseColorsFromList_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = true;

            //Act
            _viewModel.UseColorsFromList = true;
            var actual = _viewModel.UseColorsFromList;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Colors_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var colors = new ObservableCollection<ColorItem>()
            {
                new ColorItem()
                {
                    ColorBrush = new SolidColorBrush(Color.FromArgb(11, 22, 33, 44))
                }
            };
            var expected = Color.FromArgb(11, 22, 33, 44);

            //Act
            _viewModel.Colors = colors;
            var actual = _viewModel.Colors[0].ColorBrush.Color;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UIDispatcher_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = Dispatcher.CurrentDispatcher;

            //Act
            _viewModel.UIDispatcher = Dispatcher.CurrentDispatcher;
            var actual = _viewModel.UIDispatcher;

            //Assert    
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void WindowTitle_WhenGettingValueWithNullCurrentOpenProjectValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "Particle Maker";

            //Act
            var actual = _viewModel.WindowTitle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void WindowTitle_WhenGettingValueWithSetCurrentOpenProjectValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "Particle Maker - TestProject";

            //Act
            _viewModel.CurrentOpenProject = "TestProject";
            var actual = _viewModel.WindowTitle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CurrentOpenProject_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "TestProject";

            //Act
            var actual = _viewModel.CurrentOpenProject = "TestProject";

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void OpenProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.OpenProject;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void SetupItemSelected_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.SetupItemSelected;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void AddSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.AddSetup != null;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RenameProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.RenameProject != null;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DeleteProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.DeleteProject != null;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Play_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.Play != null;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Pause_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.Pause != null;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SaveSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.SaveSetup != null;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CurrentLoadedSetup_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "current-setup";

            //Act
            _viewModel.CurrentLoadedSetup = "current-setup";
            var actual = _viewModel.CurrentLoadedSetup;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UpdateDeploymentPath_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.UpdateDeploymentPath;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void DeploySetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.DeploySetup;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void RenameSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.RenameSetup;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void DeleteSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.DeleteSetup;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void AddParticle_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.AddParticle;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void RenameParticle_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.RenameParticle;

            //Assert
            Assert.NotNull(actual);
        }


        [Test]
        public void DeleteParticle_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.DeleteParticle;

            //Assert
            Assert.NotNull(actual);
        }
        #endregion


        #region Private Methods
        [SetUp]
        public void Setup()
        {
            var getValueResult = 10f;

            var particleEngine = new ParticleEngine<ParticleTexture>(null);

            var mockRandomizer = new Mock<IRandomizerService>();
            //Mock out the GetValue(float, float) overload
            mockRandomizer.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Callback<float, float>((min, max) =>
            {
                //If the total number of living particles is less than 2, set one life time. Else set another.
                //This will help get 2 living and 2 dead particls setup for testing
                getValueResult = particleEngine.TotalLivingParticles < 2 ? 10 : 30;
            }).Returns(getValueResult);

            //Mock out the GetValue(int, int) overload
            mockRandomizer.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Callback<int, int>((min, max) =>
            {
                getValueResult = particleEngine.TotalLivingParticles < 2 ? 10 : 30;
            }).Returns(() =>
            {
                return (int)getValueResult;
            });

            var mockDirService = new Mock<IDirectoryService>();
            var mockFileService = new Mock<IFileService>();
            var mockRenderer = new Mock<IRenderer>();
            var projIOService = new ProjectIOService(mockDirService.Object, mockFileService.Object);

            var setupDeployService = new SetupDeployService(mockDirService.Object, mockFileService.Object);

            particleEngine.Randomizer = mockRandomizer.Object;
            particleEngine.Add(new ParticleTexture(IntPtr.Zero, 0, 0));
            particleEngine.TotalParticlesAliveAtOnce = 4;
            particleEngine.Update(new TimeSpan(0, 0, 0, 0, 11));

            _engine = new GraphicsEngine(mockRenderer.Object, particleEngine);
            var particleManager = new ParticleManager(projIOService, mockDirService.Object, mockFileService.Object);

            _viewModel = new MainViewModel(_engine, It.IsAny<ProjectManager>(), It.IsAny<ProjectSettingsManager>(), It.IsAny<SetupManager>(), setupDeployService, particleManager)
            {
                RenderSurface = new PictureBox()
            };
        }


        [TearDown]
        public void TearDown()
        {
            _viewModel.ShutdownEngine();
            _engine = null;
        }
        #endregion
    }
}

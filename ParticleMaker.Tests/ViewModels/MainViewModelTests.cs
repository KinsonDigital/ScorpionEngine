using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using Moq;
using Xunit;
using KDParticleEngine;
using KDParticleEngine.Services;
using ParticleMaker.Management;
using ParticleMaker.Services;
using ParticleMaker.UserControls;
using ParticleMaker.ViewModels;

namespace ParticleMaker.Tests.ViewModels
{
    public class MainViewModelTests : IDisposable
    {
        #region Fields
        private GraphicsEngine _engine;
        private MainViewModel _viewModel;
        #endregion


        #region Connstructors
        public MainViewModelTests()
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
        #endregion


        #region Prop Tests
        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SettingsChanged_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = true;

            //Act
            _viewModel.RedMin = 123;

            var actual = _viewModel.SettingsChanged;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void NewProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.NewProject;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void RenderSurface_WhenSettingGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = new Control();

            //Act
            _viewModel.RenderSurface = expected;
            var actual = _viewModel.RenderSurface;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void TotalParticles_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 1234;

            //Act
            _viewModel.TotalParticlesAliveAtOnce = 1234;
            var actual = _viewModel.TotalParticlesAliveAtOnce;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void TotalLivingParticles_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var expected = 2;

            //Act
            var actual = _viewModel.TotalLivingParticles;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void TotalDeadParticles_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var expected = 2;

            //Act
            var actual = _viewModel.TotalDeadParticles;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RedMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.RedMin = 123;
            var actual = _viewModel.RedMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RedMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.RedMax = 123;
            var actual = _viewModel.RedMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GreenMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.GreenMin = 123;
            var actual = _viewModel.GreenMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GreenMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.GreenMax = 123;
            var actual = _viewModel.GreenMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BlueMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.BlueMin = 123;
            var actual = _viewModel.BlueMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BlueMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.BlueMax = 123;
            var actual = _viewModel.BlueMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SizeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SizeMin = 123;
            var actual = _viewModel.SizeMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SizeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SizeMax = 123;
            var actual = _viewModel.SizeMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngleMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngleMin = 123;
            var actual = _viewModel.AngleMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngleMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngleMax = 123;
            var actual = _viewModel.AngleMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngularVelocityMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngularVelocityMin = 123;
            var actual = _viewModel.AngularVelocityMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngularVelocityMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.AngularVelocityMax = 123;
            var actual = _viewModel.AngularVelocityMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityXMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityXMin = 123;
            var actual = _viewModel.VelocityXMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityXMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityXMax = 123;
            var actual = _viewModel.VelocityXMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityYMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityYMin = 123;
            var actual = _viewModel.VelocityYMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void VelocityYMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.VelocityYMax = 123;
            var actual = _viewModel.VelocityYMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LifetimeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.LifetimeMin = 123;
            var actual = _viewModel.LifetimeMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LifetimeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.LifetimeMax = 123;
            var actual = _viewModel.LifetimeMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SpawnRateMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SpawnRateMin = 123;
            var actual = _viewModel.SpawnRateMin;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SpawnRateMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 123;

            //Act
            _viewModel.SpawnRateMax = 123;
            var actual = _viewModel.SpawnRateMax;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void UseColorsFromList_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = true;

            //Act
            _viewModel.UseColorsFromList = true;
            var actual = _viewModel.UseColorsFromList;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
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
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void UIDispatcher_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = Dispatcher.CurrentDispatcher;

            //Act
            _viewModel.UIDispatcher = Dispatcher.CurrentDispatcher;
            var actual = _viewModel.UIDispatcher;

            //Assert    
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void WindowTitle_WhenGettingValueWithNullCurrentOpenProjectValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "Particle Maker";

            //Act
            var actual = _viewModel.WindowTitle;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void WindowTitle_WhenGettingValueWithSetCurrentOpenProjectValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "Particle Maker - TestProject";

            //Act
            _viewModel.CurrentOpenProject = "TestProject";
            var actual = _viewModel.WindowTitle;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void CurrentOpenProject_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "TestProject";

            //Act
            var actual = _viewModel.CurrentOpenProject = "TestProject";

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void OpenProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.OpenProject;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void SetupItemSelected_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.SetupItemSelected;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void AddSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.AddSetup != null;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RenameProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.RenameProject != null;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void DeleteProject_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.DeleteProject != null;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Play_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.Play != null;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Pause_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.Pause != null;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SaveSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = _viewModel.SaveSetup != null;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void CurrentLoadedSetup_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = "current-setup";

            //Act
            _viewModel.CurrentLoadedSetup = "current-setup";
            var actual = _viewModel.CurrentLoadedSetup;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void UpdateDeploymentPath_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.UpdateDeploymentPath;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void DeploySetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.DeploySetup;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void RenameSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.RenameSetup;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void DeleteSetup_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.DeleteSetup;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void AddParticle_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.AddParticle;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void RenameParticle_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.RenameParticle;

            //Assert
            Assert.NotNull(actual);
        }


        [Fact]
        public void DeleteParticle_WhenGettingValue_DoesNotReturnNull()
        {
            //Act
            var actual = _viewModel.DeleteParticle;

            //Assert
            Assert.NotNull(actual);
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _viewModel.ShutdownEngine();
            _engine = null;
        }
        #endregion
    }
}

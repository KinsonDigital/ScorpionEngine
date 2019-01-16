using KDParticleEngine;
using KDParticleEngine.Services;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using Moq;
using NUnit.Framework;
using ParticleMaker.UserControls;
using ParticleMaker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private ICoreEngine _coreEngine;
        private Mock<ICoreEngine> _mockCoreEngine;
        #endregion


        #region Prop Tests
        [Test]
        public void RenderSurface_WhenSettingGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = new Control("TEST-CONTROL");

            //Act
            model.RenderSurface = expected;
            var actual = model.RenderSurface;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UIDispatcher_WhenSettingGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = Dispatcher.CurrentDispatcher;

            //Act
            model.UIDispatcher = Dispatcher.CurrentDispatcher;
            var actual = model.UIDispatcher;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RenderSurfaceWidth_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 1234;

            //Act
            model.RenderSurfaceWidth = 1234;
            var actual = model.RenderSurfaceWidth;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RenderSurfaceHeight_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 1234;

            //Act
            model.RenderSurfaceHeight = 1234;
            var actual = model.RenderSurfaceHeight;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TotalParticles_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 1234;

            //Act
            model.TotalParticles = 1234;
            var actual = model.TotalParticles;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TotalLivingParticles_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 2;

            //Act
            var actual = model.TotalLivingParticles;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void TotalDeadParticles_WhenGettingValue_ValueIsCorrect()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 2;

            //Act
            var actual = model.TotalDeadParticles;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RedMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.RedMin = 123;
            var actual = model.RedMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RedMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.RedMax = 123;
            var actual = model.RedMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GreenMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.GreenMin = 123;
            var actual = model.GreenMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GreenMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.GreenMax = 123;
            var actual = model.GreenMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BlueMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.BlueMin = 123;
            var actual = model.BlueMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BlueMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.BlueMax = 123;
            var actual = model.BlueMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SizeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.SizeMin = 123;
            var actual = model.SizeMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }



        [Test]
        public void SizeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.SizeMax = 123;
            var actual = model.SizeMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngleMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.AngleMin = 123;
            var actual = model.AngleMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }



        [Test]
        public void AngleMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.AngleMax = 123;
            var actual = model.AngleMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularVelocityMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.AngularVelocityMin = 123;
            var actual = model.AngularVelocityMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularVelocityMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.AngularVelocityMax = 123;
            var actual = model.AngularVelocityMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityXMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.VelocityXMin = 123;
            var actual = model.VelocityXMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }



        [Test]
        public void VelocityXMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.VelocityXMax = 123;
            var actual = model.VelocityXMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityYMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.VelocityYMin = 123;
            var actual = model.VelocityYMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void VelocityYMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.VelocityYMax = 123;
            var actual = model.VelocityYMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LifetimeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.LifetimeMin = 123;
            var actual = model.LifetimeMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LifetimeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.LifetimeMax = 123;
            var actual = model.LifetimeMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SpawnRateMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.SpawnRateMin = 123;
            var actual = model.SpawnRateMin;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void SpawnRateMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = 123;

            //Act
            model.SpawnRateMax = 123;
            var actual = model.SpawnRateMax;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void UseColorsFromList_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var expected = true;

            //Act
            model.UseColorsFromList = true;
            var actual = model.UseColorsFromList;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Colors_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var model = new MainViewModel(_engine);
            var colors = new ObservableCollection<ColorItem>()
            {
                new ColorItem()
                {
                    ColorBrush = new SolidColorBrush(Color.FromArgb(11, 22, 33, 44))
                }
            };
            var expected = Color.FromArgb(11, 22, 33, 44);

            //Act
            model.Colors = colors;
            var actual = model.Colors[0].ColorBrush.Color;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void ShutdownEngine_WhenInvoked_InvokesCoreEngineExitMethod()
        {
            //Arrange
            var model = new MainViewModel(_engine);

            //Act
            model.ShutdownEngine();

            //Assert
            _mockCoreEngine.Verify(m => m.Exit(), Times.Once());
        }


        [Test]
        public void Testing()
        {
            var model = new MainViewModel(_engine);
            model.UIDispatcher = Dispatcher.CurrentDispatcher;

            Thread.Sleep(300);

            var stop = true;
        }
        #endregion


        #region Private Methods
        [SetUp]
        public void Setup()
        {
            _mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(_mockCoreEngine.Object);

            var mockTexture = new Mock<ITexture>();

            var getValueResult = 10f;

            var particleEngine = new ParticleEngine(null);

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

            particleEngine.Randomizer = mockRandomizer.Object;
            particleEngine.AddTexture(new Texture(mockTexture.Object));
            particleEngine.TotalParticlesAliveAtOnce = 4;
            particleEngine.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 11) });

            _engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);
        }


        [TearDown]
        public void TearDown()
        {
            _engine = null;
        }
        #endregion
    }
}

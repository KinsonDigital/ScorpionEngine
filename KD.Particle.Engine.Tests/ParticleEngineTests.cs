using System;
using System.Drawing;
using System.Linq;
using KDParticleEngine;
using KDParticleEngine.Services;
using KDScorpionCore.Graphics;
using Moq;
using Xunit;

namespace KDParticleEngineTests
{
    public class ParticleEngineTests
    {
        #region Private Fields
        private Mock<IRandomizerService> _mockRandomizerService;
        private ParticleEngine<ITexture> _engine;
        #endregion


        #region Constructors
        public ParticleEngineTests()
        {
            _mockRandomizerService = new Mock<IRandomizerService>();
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(10);

            _engine = new ParticleEngine<ITexture>(_mockRandomizerService.Object)
            {
                TotalParticlesAliveAtOnce = 2,
                SpawnRateMin = 10,
                SpawnRateMax = 10
            };
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoked__ProperlySetsParticleList()
        {
            //Assert
            Assert.NotNull(_engine.Particles);
        }


        [Fact]
        public void Ctor_WhenInvoked__ProperlySetsUpRandomizer()
        {
            //Assert
            Assert.NotNull(_engine.Randomizer);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Particles_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.Add(new Mock<ITexture>().Object);

            //Act
            var actual = _engine.Particles.Length;

            //Assert
            Assert.Equal(2, actual);
        }


        [Fact]
        public void Enabled_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.Enabled = false;

            //Assert
            Assert.False(_engine.Enabled);
        }


        [Fact]
        public void Enabled_WhenSettingValue_KillsAllParticles()
        {
            //Arrange
            _engine.Update(new TimeSpan(0, 0, 0, 0, 15));

            //Assert
            Assert.True(_engine.Particles.All(p => p.IsDead));
        }


        [Fact]
        public void SpawnLocation_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.SpawnLocation = new PointF(11, 22);

            //Assert
            Assert.Equal(new PointF(11, 22), _engine.SpawnLocation);
        }


        [Fact]
        public void AngleMin_WhenSettingNegativeValue_SetsTo360Degrees()
        {
            //Arrange
            _engine.AngleMin = -23;

            //Assert
            Assert.Equal(360, _engine.AngleMin);
        }


        [Fact]
        public void AngleMin_WhenSettingValueGreaterThan360_SetsToZero()
        {
            //Arrange
            _engine.AngleMin = 400;

            //Assert
            Assert.Equal(0, _engine.AngleMin);
        }


        [Fact]
        public void AngleMax_WhenSettingNegativeValue_SetsTo360Degrees()
        {
            //Arrange
            _engine.AngleMax = -23;

            //Assert
            Assert.Equal(360, _engine.AngleMax);
        }


        [Fact]
        public void AngleMax_WhenSettingValueGreaterThan360_SetsToZero()
        {
            //Arrange
            _engine.AngleMax = 400;

            //Assert
            Assert.Equal(0, _engine.AngleMax);
        }


        [Fact]
        public void LifeTimeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.LifeTimeMin = 100;

            //Assert
            Assert.Equal(100, _engine.LifeTimeMin);
        }


        [Fact]
        public void LifeTimeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.LifeTimeMax = 100;

            //Assert
            Assert.Equal(100, _engine.LifeTimeMax);
        }


        [Fact]
        public void SizeMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.SizeMin = 100;

            //Assert
            Assert.Equal(100, _engine.SizeMin);
        }


        [Fact]
        public void SizeMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.SizeMax = 100;

            //Assert
            Assert.Equal(100, _engine.SizeMax);
        }


        [Fact]
        public void AngularVelocityMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.AngularVelocityMin = 100;

            //Assert
            Assert.Equal(100, _engine.AngularVelocityMin);
        }


        [Fact]
        public void AngularVelocityMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.AngularVelocityMax = 100;

            //Assert
            Assert.Equal(100, _engine.AngularVelocityMax);
        }


        [Fact]
        public void SpawnRateMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.SpawnRateMin = 100;

            //Assert
            Assert.Equal(100, _engine.SpawnRateMin);
        }


        [Fact]
        public void SpawnRateMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.SpawnRateMax = 100;

            //Assert
            Assert.Equal(100, _engine.SpawnRateMax);
        }


        [Fact]
        public void TotalLivingParticles_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.Add(new Mock<ITexture>().Object);

            //Assert
            Assert.Equal(2, _engine.TotalLivingParticles);
        }


        [Fact]
        public void TotalDeadParticles_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.Add(new Mock<ITexture>().Object);
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.Equal(2, _engine.TotalDeadParticles);
        }


        [Fact]
        public void UseRandomVelocity_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.UseRandomVelocity = false;

            //Assert
            Assert.False(_engine.UseRandomVelocity);
        }


        [Fact]
        public void UserColorsFromList_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.UseColorsFromList = true;

            //Assert
            Assert.True(_engine.UseColorsFromList);
        }


        [Fact]
        public void TintColors_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.TintColors = new Color[1] { Color.FromArgb(11, 22, 33, 44) };

            //Assert
            Assert.Equal(new Color[1] { Color.FromArgb(11, 22, 33, 44) }, _engine.TintColors);
        }


        [Fact]
        public void RedMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.RedMin = 111;

            //Assert
            Assert.Equal(111, _engine.RedMin);
        }


        [Fact]
        public void RedMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.RedMax = 111;

            //Assert
            Assert.Equal(111, _engine.RedMax);
        }


        [Fact]
        public void GreenMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.GreenMin = 111;

            //Assert
            Assert.Equal(111, _engine.GreenMin);
        }


        [Fact]
        public void GreenMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.GreenMax = 111;

            //Assert
            Assert.Equal(111, _engine.GreenMax);
        }


        [Fact]
        public void BlueMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.BlueMin = 111;

            //Assert
            Assert.Equal(111, _engine.BlueMin);
        }


        [Fact]
        public void BlueMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.BlueMax = 111;

            //Assert
            Assert.Equal(111, _engine.BlueMax);
        }


        [Fact]
        public void VelocityXMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.VelocityXMin = 111;

            //Assert
            Assert.Equal(111, _engine.VelocityXMin);
        }


        [Fact]
        public void VelocityXMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.VelocityXMax = 111;

            //Assert
            Assert.Equal(111, _engine.VelocityXMax);
        }


        [Fact]
        public void VelocityYMin_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.VelocityYMin = 111;

            //Assert
            Assert.Equal(111, _engine.VelocityYMin);
        }


        [Fact]
        public void VelocityYMax_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.VelocityYMax = 111;

            //Assert
            Assert.Equal(111, _engine.VelocityYMax);
        }


        [Fact]
        public void IsReadOnly_WhenGettingValue_ReturnsCorrectValue()
        {
            //Assert
            Assert.False(_engine.IsReadOnly);
        }


        [Fact]
        public void IsFixedSize_WhenGettingValue_ReturnsCorrectValue()
        {
            //Assert
            Assert.False(_engine.IsFixedSize);
        }


        [Fact]
        public void Count_WhenGettingValue_ReturnsCorrectValue()
        {
            //Act
            _engine.Add(new Mock<ITexture>().Object);
            var actual = _engine.Count;

            //Assert
            Assert.Equal(1, actual);
        }


        [Fact]
        public void SyncRoot_WhenSettingValue_ReturnsCorrectValue()
        {
            //Act
            var testObj = new object();
            _engine.SyncRoot = testObj;

            //Assert
            Assert.Equal(testObj, _engine.SyncRoot);
        }


        [Fact]
        public void IsSyncrhonized_WhenGettingValue_ReturnsCorrectValue()
        {
            //Assert
            Assert.False(_engine.IsSynchronized);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvokedWhileDisabled_DoesNotGenerateParticles()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(1000);
            _engine.Add(new Mock<ITexture>().Object);
            _engine.Enabled = false;
            _engine.KillAllParticles();

            //Act
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.Equal(0, _engine.TotalLivingParticles);
        }


        [Fact]
        public void Update_WhenInvokedWithAllDeadParticles_CountChangedEventNeverFired()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(-10);
            _engine.Add(new Mock<ITexture>().Object);

            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(5000);

            //Act
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));
            _engine.KillAllParticles();
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));
            var eventInvoked = false;
            _engine.LivingParticlesCountChanged += (sender, e) => eventInvoked = true;

            //Assert
            Assert.False(eventInvoked);
        }


        [Fact]
        public void Add_WhenInvoked_AddsTextureToEngine()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            _engine.Add(mockTexture.Object);

            //Act
            var actual = _engine.Count;

            //Assert
            Assert.Equal(1, actual);
        }


        [Fact]
        public void Add_WhenInvokedWithPredicateReturningTrue_AddsTextureToEngine()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            _engine.Add(mockTexture.Object, (texture) => true);

            //Act
            var actual = _engine.Count;

            //Assert
            Assert.Equal(1, actual);
        }


        [Fact]
        public void Add_WhenInvokedWithPredicateReturningFalse_DoesNotAddTextureToEngine()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            _engine.Add(mockTexture.Object, (texture) => false);

            //Act
            var actual = _engine.Count;

            //Assert
            Assert.Equal(0, actual);
        }


        [Fact]
        public void Add_WhenInvokedWithTextureArray_AddsAllTexturesToEngine()
        {
            //Arrange
            var mockTextureA = new Mock<ITexture>();
            var mockTextureB = new Mock<ITexture>();
            _engine.Add(new ITexture[] { mockTextureA.Object, mockTextureB.Object });

            //Act
            var actual = _engine.Count;

            //Assert
            Assert.Equal(2, actual);
        }


        [Fact]
        public void Contains_WhenInvokedWithAddedTexture_ReturnsTrue()
        {
            //Arrange
            var mockTexture = new Mock<ITexture>();
            _engine.Add(mockTexture.Object);

            //Act
            var actual = _engine.Contains(mockTexture.Object);

            //Assert
            Assert.True(actual);
        }
        #endregion
    }
}

using System;
using System.Drawing;
using System.Linq;
using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using Xunit;

namespace KDParticleEngineTests
{
    /// <summary>
    /// Unit tests to test the <see cref="KDParticleEngine.ParticleEngine{Texture}"/> class.
    /// </summary>
    public class ParticleEngineTests : IDisposable
    {
        #region Private Fields
        private Mock<IRandomizerService> _mockRandomizerService;
        private ParticleEngine<IFakeTexture> _engine;
        #endregion


        #region Constructors
        public ParticleEngineTests()
        {
            _mockRandomizerService = new Mock<IRandomizerService>();

            _engine = new ParticleEngine<IFakeTexture>(_mockRandomizerService.Object)
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
            _engine.Add(new Mock<IFakeTexture>().Object);

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
            _engine.Add(new Mock<IFakeTexture>().Object);

            //Assert
            Assert.Equal(2, _engine.TotalLivingParticles);
        }


        [Fact]
        public void TotalDeadParticles_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.Add(new Mock<IFakeTexture>().Object);
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
        public void ParticleVelocity_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.ParticleVelocity = new PointF(11, 22);

            //Assert
            Assert.Equal(new PointF(11, 22), _engine.ParticleVelocity);
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
            _engine.Add(new Mock<IFakeTexture>().Object);
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


        [Fact]
        public void TotalParticlesAliveAtOnce_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _engine.TotalParticlesAliveAtOnce = 100;

            //Assert
            Assert.Equal(100, _engine.TotalParticlesAliveAtOnce);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Add_WhenInvokedWithUseRandomVelocityFalse_UsesParticleVelocity()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(10);
            var mockTexture = new Mock<IFakeTexture>();
            _engine.UseRandomVelocity = false;
            _engine.Add(mockTexture.Object);

            //Act
            var actual = _engine.Count;

            //Assert
            Assert.Equal(new PointF(0, 1), _engine.Particles[0].Velocity);
        }


        [Fact]
        public void Add_WhenInvokedWithUseRandomVelocityTrue_UsesRandomVelocity()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(10);
            var mockTexture = new Mock<IFakeTexture>();
            _engine.UseRandomVelocity = true;
            _engine.Add(mockTexture.Object);

            //Act
            var actual = _engine.Count;

            //Assert
            Assert.Equal(new PointF(10, 10), _engine.Particles[0].Velocity);
        }


        [Fact]
        public void Add_WhenInvoked_AddsTextureToEngine()
        {
            //Arrange
            var mockTexture = new Mock<IFakeTexture>();
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
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            var mockTexture = new Mock<IFakeTexture>();
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
            var mockTexture = new Mock<IFakeTexture>();
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
            var mockTextureA = new Mock<IFakeTexture>();
            var mockTextureB = new Mock<IFakeTexture>();
            _engine.Add(new IFakeTexture[] { mockTextureA.Object, mockTextureB.Object });

            //Act
            var actual = _engine.Count;

            //Assert
            Assert.Equal(2, actual);
        }


        [Fact]
        public void Add_WhenInvoked_CanGeneratePositiveAngularVelocity()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.FlipCoin()).Returns(true);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(10);
            _engine.Add(new Mock<IFakeTexture>().Object);

            //Assert
            Assert.Equal(10, _engine.Particles[0].AngularVelocity);
        }


        [Fact]
        public void Add_WhenInvoked_CanGenerateNegativeAngularVelocity()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.FlipCoin()).Returns(false);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(10);
            _engine.Add(new Mock<IFakeTexture>().Object);

            //Assert
            Assert.Equal(-10, _engine.Particles[0].AngularVelocity);
        }


        [Fact]
        public void Contains_WhenInvokedWithAddedTexture_ReturnsTrue()
        {
            //Arrange
            var mockTexture = new Mock<IFakeTexture>();
            _engine.Add(mockTexture.Object);

            //Act
            var actual = _engine.Contains(mockTexture.Object);

            //Assert
            Assert.True(actual);
        }


        [Fact]
        public void Update_WhenInvokedWhileDisabled_DoesNotGenerateParticles()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.Enabled = false;
            _engine.KillAllParticles();

            //Act
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.Equal(0, _engine.TotalLivingParticles);
        }


        [Fact]
        public void Update_WhenInvokedWhenSpawnMinIsLessOrEqualToMax_ProperlyRandomizesValue()
        {
            //Arrange
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.SpawnRateMin = 2;
            _engine.SpawnRateMax = 6;

            //Act
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            _mockRandomizerService.Verify(m => m.GetValue(2, 6), Times.Once());
        }


        [Fact]
        public void Update_WhenInvokedWhenSpawnMaxIsGreaterThanMin_ProperlyRandomizesValue()
        {
            //Arrange
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.SpawnRateMin = 6;
            _engine.SpawnRateMax = 2;

            //Act
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            _mockRandomizerService.Verify(m => m.GetValue(2, 6), Times.Once());
        }


        [Fact]
        public void Update_WhenInvokedWhenLifeTimeMinIsLessOrEqualToMax_ProperlyRandomizesValue()
        {
            //Arrange
            _engine.LifeTimeMin = 2;
            _engine.LifeTimeMax = 6;

            //Act
            _engine.Add(new Mock<IFakeTexture>().Object);

            //Assert
            _mockRandomizerService.Verify(m => m.GetValue(2, 6), Times.AtLeast(1));
        }


        [Fact]
        public void Update_WhenInvokedWhenLifeTimeMaxIsGreaterThanMin_ProperlyRandomizesValue()
        {
            //Arrange
            _engine.LifeTimeMin = 6;
            _engine.LifeTimeMax = 2;

            //Act
            _engine.Add(new Mock<IFakeTexture>().Object);

            //Assert
            _mockRandomizerService.Verify(m => m.GetValue(2, 6), Times.AtLeast(1));
        }


        [Fact]
        public void Update_WhenInvokingWhileExpiredParticleLifeTime_LivingParticlesCountChangeEventFires()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            _engine.Add(new Mock<IFakeTexture>().Object);

            //Act
            var eventInvoked = false;
            _engine.KillAllParticles();
            _engine.LivingParticlesCountChanged += (sender, e) => eventInvoked = true;
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.True(eventInvoked);
        }

        [Fact]
        public void Update_WhenInvokingWithUnexpiredParticleLifeTime_LivingParticlesCountChangeEventNeverFires()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(1000);
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.PreParticleUpdate = () => _engine.KillAllParticles();

            //Act
            var eventInvoked = false;
            _engine.LivingParticlesCountChanged += (sender, e) => eventInvoked = true;
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.False(eventInvoked);
        }


        [Fact]
        public void Update_WhenInvokingWithNullEvent_DoesNotThrowException()
        {
            //Arrange
            _engine.Add(new Mock<IFakeTexture>().Object);

            //Act/Assert
            DoesNotThrowNullReference(() => _engine.Update(new TimeSpan(0, 0, 0, 0, 30)));
        }


        [Fact]
        public void Update_WhenInvokingWithDeadParticles_SetsParticleProps()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.FlipCoin()).Returns(true);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            _engine.Add(new Mock<IFakeTexture>().Object);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(10);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<float>(), It.IsAny<float>())).Returns(10);
            _engine.KillAllParticles();

            //Act
            _engine.Particles[0].Texture = null;
            _engine.Particles[0].Position = PointF.Empty;
            _engine.Particles[0].Velocity = PointF.Empty;
            _engine.Particles[0].Angle = -10;
            _engine.Particles[0].AngularVelocity = -10;
            _engine.Particles[0].TintColor = Color.Empty;
            _engine.Particles[0].Size = -10;
            _engine.Particles[0].LifeTime = -10;
            _engine.Particles[0].IsAlive = false;

            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.NotNull(_engine.Particles[0].Texture);
            Assert.NotEqual(PointF.Empty, _engine.Particles[0].Position);
            Assert.NotEqual(PointF.Empty, _engine.Particles[0].Velocity);
            Assert.NotEqual(-10, _engine.Particles[0].Angle);
            Assert.NotEqual(-10, _engine.Particles[0].AngularVelocity);
            Assert.NotEqual(Color.Empty, _engine.Particles[0].TintColor);
            Assert.NotEqual(-10, _engine.Particles[0].Size);
            Assert.NotEqual(-10, _engine.Particles[0].LifeTime);
        }


        [Fact]
        public void Update_WhenInvokingUseColorsFromListFalse_CorrectlySetsColor()
        {
            //Arrange
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.RedMin = 20;
            _engine.RedMax = 10;
            _engine.GreenMin = 20;
            _engine.GreenMax = 10;
            _engine.BlueMin = 20;
            _engine.BlueMax = 10;

            _engine.KillAllParticles();

            //Act
            _engine.Particles[0].TintColor = Color.Empty;
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.NotEqual(Color.Empty, _engine.Particles[0].TintColor);
        }


        [Fact]
        public void Update_WhenInvokingWithUseColorsFromListTrueAndTintColorsNotNull_CorrectlySetsColor()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.FlipCoin()).Returns(true);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            _engine.UseColorsFromList = true;
            _engine.TintColors = new Color[] { Color.FromArgb(255, 11, 22, 33) };
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.KillAllParticles();

            //Act
            _engine.Particles[0].TintColor = Color.Empty;
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.NotEqual(Color.Empty, _engine.Particles[0].TintColor);
        }


        [Fact]
        public void Update_WhenInvokingWithUseColorsFromListTrueAndTintColorsNull_CorrectlySetsColor()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.FlipCoin()).Returns(true);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            _engine.UseColorsFromList = true;
            _engine.TintColors = null;
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.KillAllParticles();

            //Act
            _engine.Particles[0].TintColor = Color.Empty;
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.NotEqual(Color.Empty, _engine.Particles[0].TintColor);
        }


        [Fact]
        public void Update_WhenInvokingUseRandomVelocityFalse_CorrectlySetsParticleVelocity()
        {
            //Arrange
            _mockRandomizerService.Setup(m => m.FlipCoin()).Returns(true);
            _mockRandomizerService.Setup(m => m.GetValue(It.IsAny<int>(), It.IsAny<int>())).Returns(0);
            _engine.UseRandomVelocity = false;
            _engine.ParticleVelocity = new Point(11, 22);
            _engine.Add(new Mock<IFakeTexture>().Object);
            _engine.KillAllParticles();

            //Act
            _engine.Update(new TimeSpan(0, 0, 0, 0, 30));

            //Assert
            Assert.Equal(new PointF(11, 22), _engine.Particles[0].Velocity);
        }


        [Fact]
        public void ApplySetup_WhenInvoked_ProperlySetusUpEngine()
        {
            //Arrange
            var setupData = new ParticleSetup()
            {
                RedMin = 1,
                RedMax = 2,
                GreenMin = 3,
                GreenMax = 4,
                BlueMin = 5,
                BlueMax = 6,
                SizeMin = 7,
                SizeMax = 8,
                AngleMin = 9,
                AngleMax = 10,
                AngularVelocityMin = 11,
                AngularVelocityMax = 22,
                VelocityXMin = 33,
                VelocityXMax = 44,
                VelocityYMin = 55,
                VelocityYMax = 66,
                LifeTimeMin = 77,
                LifeTimeMax = 88,
                SpawnRateMin = 99,
                SpawnRateMax = 100,
                TotalParticlesAliveAtOnce = 111,
                UseColorsFromList = true,
                Colors = new Color[] { Color.FromArgb(11, 22, 33, 44) }
            };

            //Act
            _engine.ApplySetup(setupData);

            //Assert
            Assert.Equal(1, _engine.RedMin);
            Assert.Equal(2, _engine.RedMax);
            Assert.Equal(3, _engine.GreenMin);
            Assert.Equal(4, _engine.GreenMax);
            Assert.Equal(5, _engine.BlueMin);
            Assert.Equal(6, _engine.BlueMax);
            Assert.Equal(7, _engine.SizeMin);
            Assert.Equal(8, _engine.SizeMax);
            Assert.Equal(9, _engine.AngleMin);
            Assert.Equal(10, _engine.AngleMax);
            Assert.Equal(11, _engine.AngularVelocityMin);
            Assert.Equal(22, _engine.AngularVelocityMax);
            Assert.Equal(33, _engine.VelocityXMin);
            Assert.Equal(44, _engine.VelocityXMax);
            Assert.Equal(55, _engine.VelocityYMin);
            Assert.Equal(66, _engine.VelocityYMax);
            Assert.Equal(77, _engine.LifeTimeMin);
            Assert.Equal(88, _engine.LifeTimeMax);
            Assert.Equal(99, _engine.SpawnRateMin);
            Assert.Equal(100, _engine.SpawnRateMax);
            Assert.Equal(111, _engine.TotalParticlesAliveAtOnce);
            Assert.True(_engine.UseColorsFromList);
            Assert.Equal(new Color[] { Color.FromArgb(11, 22, 33, 44) }, _engine.TintColors);
        }


        [Fact]
        public void GenerateParticleSetup_WhenInvoked_ProperlySetusUpEngine()
        {
            //Arrange
            var setupData = new ParticleSetup()
            {
                RedMin = 1,
                RedMax = 2,
                GreenMin = 3,
                GreenMax = 4,
                BlueMin = 5,
                BlueMax = 6,
                SizeMin = 7,
                SizeMax = 8,
                AngleMin = 9,
                AngleMax = 10,
                AngularVelocityMin = 11,
                AngularVelocityMax = 22,
                VelocityXMin = 33,
                VelocityXMax = 44,
                VelocityYMin = 55,
                VelocityYMax = 66,
                LifeTimeMin = 77,
                LifeTimeMax = 88,
                SpawnRateMin = 99,
                SpawnRateMax = 100,
                TotalParticlesAliveAtOnce = 111,
                UseColorsFromList = true,
                Colors = new Color[] { Color.FromArgb(11, 22, 33, 44) }
            };

            //Act
            _engine.ApplySetup(setupData);
            var actual = _engine.GenerateParticleSetup();

            //Assert
            Assert.Equal(1, actual.RedMin);
            Assert.Equal(2, actual.RedMax);
            Assert.Equal(3, actual.GreenMin);
            Assert.Equal(4, actual.GreenMax);
            Assert.Equal(5, actual.BlueMin);
            Assert.Equal(6, actual.BlueMax);
            Assert.Equal(7, actual.SizeMin);
            Assert.Equal(8, actual.SizeMax);
            Assert.Equal(9, actual.AngleMin);
            Assert.Equal(10, actual.AngleMax);
            Assert.Equal(11, actual.AngularVelocityMin);
            Assert.Equal(22, actual.AngularVelocityMax);
            Assert.Equal(33, actual.VelocityXMin);
            Assert.Equal(44, actual.VelocityXMax);
            Assert.Equal(55, actual.VelocityYMin);
            Assert.Equal(66, actual.VelocityYMax);
            Assert.Equal(77, actual.LifeTimeMin);
            Assert.Equal(88, actual.LifeTimeMax);
            Assert.Equal(99, actual.SpawnRateMin);
            Assert.Equal(100, actual.SpawnRateMax);
            Assert.Equal(111, actual.TotalParticlesAliveAtOnce);
            Assert.True(actual.UseColorsFromList);
            Assert.Equal(new Color[] { Color.FromArgb(11, 22, 33, 44) }, actual.Colors);
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Asserts if an action does not throw a null reference exception.
        /// </summary>
        /// <param name="action">The action to catch the exception against.</param>
        private void DoesNotThrowNullReference(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NullReferenceException))
                {
                    Assert.True(false, $"Expected not to raise a {nameof(NullReferenceException)} exception.");
                }
                else
                {
                    Assert.True(true);
                }
            }
        }


        public void Dispose()
        {
            _mockRandomizerService = null;
            _engine = null;
        }
        #endregion
    }
}

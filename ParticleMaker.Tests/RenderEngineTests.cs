using System;
using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using ParticleMaker.Services;
using Xunit;

namespace ParticleMaker.Tests
{
    public class RenderEngineTests : IDisposable
    {
        #region Private Fields
        private ParticleEngine<ParticleTexture> _particleEngine;
        private Mock<IRenderer> _mockRenderer;
        private Mock<ITimingService> _mockTimingService;
        private RenderEngine _engine;
        #endregion


        #region Constructors
        public RenderEngineTests()
        {
            _particleEngine = new ParticleEngine<ParticleTexture>(new Mock<IRandomizerService>().Object);

            _mockRenderer = new Mock<IRenderer>();
            _mockTimingService = new Mock<ITimingService>();

            _engine = new RenderEngine(_mockRenderer.Object, _particleEngine, _mockTimingService.Object);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void ParticleEngine_WhenGettingValue_ReturnsParticleEngine()
        {
            //Assert
            Assert.Equal(_particleEngine, _engine.ParticleEngine);
        }


        [Fact]
        public void FPS_WhenSettingValue_ReturnsCorrectResult()
        {
            //Arrange
            _mockTimingService.SetupGet(p => p.FPS).Returns(1234);

            //Assert
            Assert.Equal(1234, _engine.FPS);
        }


        [Fact]
        public void TargetFrameRate_WhenSettingValue_ReturnsCorrectResult()
        {
            //Arrange
            _engine.TargetFrameRate = 30f;

            //Assert
            Assert.Equal(30.0000019f, _engine.TargetFrameRate);
        }


        [Fact]
        public void Pause_WhenInvoked_InvokesTimingServicePause()
        {
            //Arrange
            _engine.Pause();

            //Assert
            _mockTimingService.Verify(m => m.Pause(), Times.Once());
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Start_WhenInvokedWhilePaused_InvokesTimingServiceStart()
        {
            //Arrange
            _mockTimingService.SetupGet(p => p.IsPaused).Returns(true);

            //Act
            _engine.Start();

            //Assert
            _mockTimingService.Verify(m => m.Start(), Times.Once());
        }


        [Fact]
        public async void Start_WhenInvoked_LoadesTexture()
        {
            //Arrange
            _mockTimingService.SetupGet(p => p.TotalMilliseconds).Returns(1000);
            _mockTimingService.Setup(m => m.Record()).Callback(() => _engine.Stop());
            var texturePath = @"C:\temp\texture-A.png";
            _engine.TexturePaths = new[] { texturePath };

            //Act
            await _engine.Start();

            //Assert
            _mockRenderer.Verify(m => m.LoadTexture(texturePath), Times.Once());
        }


        [Fact]
        public void Render_WhenInvoked_InvokesRendererBeginAndEnd()
        {
            //Arrange
            _mockTimingService.SetupGet(p => p.TotalMilliseconds).Returns(1000);
            _mockTimingService.Setup(m => m.Record()).Callback(() => _engine.Stop());
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>());

            //Act
            _engine.ParticleEngine.Add(texture);
            var startTask = _engine.Start();

            startTask.Wait();

            //Assert
            _mockRenderer.Verify(m => m.Begin(), Times.Once());
            _mockRenderer.Verify(m => m.End(), Times.Once());
        }


        [Fact]
        public void Render_WhenInvokedWithZeroParticles_RendererBeginAndEndNotInvoked()
        {
            //Arrange
            _particleEngine.TotalParticlesAliveAtOnce = 0;
            _mockTimingService.SetupGet(p => p.TotalMilliseconds).Returns(1000);
            _mockTimingService.Setup(m => m.Record()).Callback(() => _engine.Stop());
            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>());

            //Act
            _engine.ParticleEngine.Add(texture);
            var startTask = _engine.Start();

            startTask.Wait();

            //Assert
            _mockRenderer.Verify(m => m.Begin(), Times.Never());
            _mockRenderer.Verify(m => m.End(), Times.Never());
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _engine.Stop();
            _engine.Dispose();
            _engine = null;
            _mockTimingService = null;
            _particleEngine = null;
            _mockRenderer = null;
        }
        #endregion
    }
}

using System;
using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using ParticleMaker.Services;
using Xunit;

namespace ParticleMaker.Tests
{
    /// <summary>
    /// Unit tests to test the <see cref="RenderEngine"/> class.
    /// </summary>
    public class RenderEngineTests : IDisposable
    {
        #region Private Fields
        private ParticleEngine<ParticleTexture> _particleEngine;
        private Mock<IRenderer> _mockRenderer;
        private Mock<ITimingService> _mockTimingService;
        private readonly Mock<ITaskManagerService> _mockTaskService;
        private RenderEngine _engine;
        #endregion


        #region Constructors
        public RenderEngineTests()
        {
            _particleEngine = new ParticleEngine<ParticleTexture>(new Mock<IRandomizerService>().Object);
            _mockRenderer = new Mock<IRenderer>();
            _mockTimingService = new Mock<ITimingService>();
            _mockTaskService = new Mock<ITaskManagerService>();

            _engine = new RenderEngine(_mockRenderer.Object, _particleEngine, _mockTimingService.Object, _mockTaskService.Object);
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
        public void Start_WhenInvoked_LoadesTexture()
        {
            //Arrange
            _mockTimingService.SetupGet(p => p.TotalMilliseconds).Returns(1000);
            _mockTimingService.Setup(m => m.Record()).Callback(() => _engine.Stop());
            var texturePath = @"C:\temp\texture-A.png";
            _engine.TexturePaths = new[] { texturePath };

            //Act
            _engine.Start();

            //Assert
            _mockRenderer.Verify(m => m.LoadTexture(texturePath), Times.Once());
        }


        [Fact]
        public void Start_WhenInvokedWhileRunning_UnPausesEngine()
        {
            //Arrange
            _mockTaskService.SetupGet(p => p.IsRunning).Returns(true);
            _mockTimingService.SetupGet(p => p.TotalMilliseconds).Returns(1000);
            _mockTimingService.SetupGet(m => m.IsPaused).Returns(true);
            var texturePath = @"C:\temp\texture-A.png";
            _engine.TexturePaths = new[] { texturePath };

            //Act
            _engine.Start();

            //Assert
            _mockTimingService.Verify(m => m.Start(), Times.Once());
        }


        [Fact]
        public void RenderEngine_WhenInvoked_InvokesRendererBeginAndEnd()
        {
            //Arrange
            var cancelPending = false;
            _mockTimingService.SetupGet(p => p.TotalMilliseconds).Returns(1000);
            _mockTimingService.Setup(m => m.Record()).Callback(() => _engine.Stop());
            _mockTaskService.SetupGet(p => p.CancelPending).Returns(() => cancelPending);
            _mockTaskService.Setup(m => m.Start(It.IsAny<Action>())).Callback(() => _engine.Run());
            _mockRenderer.Setup(m => m.End()).Callback(() => cancelPending = true);

            var texture = new ParticleTexture(It.IsAny<IntPtr>(), It.IsAny<int>(), It.IsAny<int>());

            //Act
            _engine.ParticleEngine.Add(texture);
            _engine.Start();

            //Assert
            _mockRenderer.Verify(m => m.Begin(), Times.Once());
            _mockRenderer.Verify(m => m.End(), Times.Once());
        }


        [Fact]
        public void RenderEngine_WhenInvokedWithZeroParticles_RendererBeginAndEndNotInvoked()
        {
            //Arrange
            var cancelPending = false;
            _mockTaskService.SetupGet(p => p.CancelPending).Returns(() => cancelPending);
            _mockTaskService.Setup(m => m.Start(It.IsAny<Action>())).Callback(() => _engine.Run());
            _mockTimingService.SetupGet(p => p.TotalMilliseconds).Returns(1000);
            _mockTimingService.Setup(m => m.Record()).Callback(() => cancelPending = true);
            _particleEngine.TotalParticlesAliveAtOnce = 0;

            //Act
            _engine.Start();

            //Assert
            _mockRenderer.Verify(m => m.Begin(), Times.Never());
            _mockRenderer.Verify(m => m.End(), Times.Never());
        }


        [Fact]
        public void RenderEngine_WhenPaused_InvokesTimingServiceWait()
        {
            //Arrange
            var cancelPending = false;
            _mockTaskService.SetupGet(p => p.CancelPending).Returns(() => cancelPending);
            _mockTaskService.Setup(m => m.Start(It.IsAny<Action>())).Callback(() => _engine.Run());
            _mockTimingService.SetupGet(p => p.IsPaused).Returns(true);
            _mockTimingService.Setup(m => m.Wait()).Callback(() => cancelPending = true);

            //Act
            _engine.Start();

            //Assert
            _mockTimingService.Verify(m => m.Wait(), Times.Once());
        }


        [Fact]
        public void SetRenderWindow_WhenInvoked_InvokesRendererInit()
        {
            //Arrange
            _engine.SetRenderWindowHandle(It.IsAny<IntPtr>());

            //Assert
            _mockRenderer.Verify(m => m.Init(It.IsAny<IntPtr>()), Times.Once());
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

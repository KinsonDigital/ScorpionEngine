using System;
using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using ParticleMaker.Services;
using Xunit;

namespace ParticleMaker.Tests
{
    public class GraphicsEngineTests : IDisposable
    {
        #region Private Fields
        private ParticleEngine<ParticleTexture> _particleEngine;
        private Mock<IRenderer> _mockRenderer;
        private GraphicsEngine _engine;
        #endregion


        #region Constructors
        public GraphicsEngineTests()
        {
            _particleEngine = new ParticleEngine<ParticleTexture>(new Mock<IRandomizerService>().Object);

            _mockRenderer = new Mock<IRenderer>();

            _engine = new GraphicsEngine(_mockRenderer.Object, _particleEngine, new Mock<IStopWatchService>().Object);
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
        public void DesiredFPS_WhenSettingValue_ReturnsCorrectResult()
        {
            GraphicsEngine.TargetFrameRate = 30f;

            //Assert
            Assert.Equal(30.0000019f, GraphicsEngine.TargetFrameRate);
        }


        [Fact]
        public void CurrentFSP_WhenGettingValue_ReturnsCorrectResult()
        {
            //Arrange
            var gameTime = new TimeSpan(0, 0, 0, 0, 10);

            //_engine.Start(IntPtr.Zero);

            //Assert
            Assert.True(false, "NOT IMPLEMENTED");
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Start_WhenInvoked_InvokesRendererInit()
        {
            _engine.Start(IntPtr.Zero);

            _mockRenderer.Verify(m => m.Init(It.IsAny<IntPtr>()), Times.Once());
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _engine.Stop();
            _engine.Dispose();
            _engine = null;
            _particleEngine = null;
            _mockRenderer = null;
        }
        #endregion
    }
}

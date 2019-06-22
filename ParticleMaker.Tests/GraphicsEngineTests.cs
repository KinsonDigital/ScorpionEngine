using System;
using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using Xunit;

namespace ParticleMaker.Tests
{
    public class GraphicsEngineTests : IDisposable
    {
        #region Private Fields
        private ParticleEngine<ParticleTexture> _particleEngine;
        private GraphicsEngine _engine;
        #endregion


        #region Constructors
        public GraphicsEngineTests()
        {
            _particleEngine = new ParticleEngine<ParticleTexture>(new Mock<IRandomizerService>().Object);

            _engine = new GraphicsEngine(new Mock<IRenderer>().Object, _particleEngine);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void ParticleEngine_WhenGettingValue_ReturnsParticleEngine()
        {
            //Assert
            Assert.Equal(_particleEngine, _engine.ParticleEngine);
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _particleEngine = null;
            _engine = null;
        }
        #endregion
    }
}

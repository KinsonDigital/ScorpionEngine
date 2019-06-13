using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using NUnit.Framework;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class GraphicsEngineTests
    {
        private ParticleEngine<ParticleTexture> _particleEngine;
        private GraphicsEngine _engine;


        #region Prop Tests
        [Test]
        public void ParticleEngine_WhenGettingValue_ReturnsParticleEngine()
        {
            //Assert
            Assert.AreEqual(_particleEngine, _engine.ParticleEngine);
        }
        #endregion


        #region Method Tests
        #endregion


        #region Private Methods
        [SetUp]
        public void Setup()
        {
            _particleEngine = new ParticleEngine<ParticleTexture>(new Mock<IRandomizerService>().Object);

            _engine = new GraphicsEngine(new Mock<IRenderer>().Object, _particleEngine);
        }


        [TearDown]
        public void TearDown()
        {
            _particleEngine = null;
            _engine = null;
        }
        #endregion
    }
}

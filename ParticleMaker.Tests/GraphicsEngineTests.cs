using KDParticleEngine;
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


        [Test]
        public void Width_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 1234;

            //Act
            _engine.Width = 1234;
            var actual = _engine.Width;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Height_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var expected = 1234;

            //Act
            _engine.Height = 1234;
            var actual = _engine.Height;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        #endregion


        #region Private Methods
        [SetUp]
        public void Setup()
        {
        }


        [TearDown]
        public void TearDown()
        {
        }
        #endregion
    }
}

using KDParticleEngine;
using Moq;
using NUnit.Framework;
using System;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class GraphicsEngineTests
    {
        #region Prop Tests
        [Test]
        public void ParticleEngine_WhenInvoking_ReturnsParticleEngine()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine();

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);
            var expected = particleEngine;

            //Act
            var actual = engine.ParticleEngine;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RenderSurfaceHandle_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();
            mockCoreEngine.SetupGet(p => p.RenderSurfaceHandle).Returns(new IntPtr(1234));

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine();

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);
            var expected = new IntPtr(1234);

            //Act
            var actual = engine.RenderSurfaceHandle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RenderSurfaceHandle_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();
            mockCoreEngine.SetupProperty(p => p.RenderSurfaceHandle);

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine();

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);
            var expected = new IntPtr(5678);

            //Act
            engine.RenderSurfaceHandle = new IntPtr(5678);

            var actual = engine.RenderSurfaceHandle;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

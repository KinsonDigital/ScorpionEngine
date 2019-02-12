using KDParticleEngine;
using KDParticleEngine.Services;
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
        public void IsRunning_WhenGettingValueWhileEngineIsRunning_ReturnsTrue()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();
            mockCoreEngine.SetupGet(p => p.IsRunning).Returns(true);

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var engine = new GraphicsEngine(mockEngineFactory.Object, It.IsAny<ParticleEngine>());

            //Act
            engine.Play();

            //Assert
            Assert.IsTrue(engine.IsRunning);
        }


        [Test]
        public void ParticleEngine_WhenGettingValue_ReturnsParticleEngine()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine(new RandomizerService());

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

            var particleEngine = new ParticleEngine(new RandomizerService());

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

            var particleEngine = new ParticleEngine(new RandomizerService());

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);
            var expected = new IntPtr(5678);

            //Act
            engine.RenderSurfaceHandle = new IntPtr(5678);

            var actual = engine.RenderSurfaceHandle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Width_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine(new RandomizerService());

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);
            var expected = 1234;

            //Act
            engine.Width = 1234;
            var actual = engine.Width;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Height_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine(new RandomizerService());

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);
            var expected = 1234;

            //Act
            engine.Height = 1234;
            var actual = engine.Height;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Run_WhenInvoked_InvokesCoreEngineRunMethod()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();
            mockCoreEngine.SetupGet(p => p.RenderSurfaceHandle).Returns(new IntPtr(1234));

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine(new RandomizerService());

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);

            //Act
            engine.Start();

            //Assert
            mockCoreEngine.Verify(m => m.Run(), Times.Once());
        }


        [Test]
        public void Run_WhenInvokedWithoutSurfaceHandle_ThrowsException()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine(new RandomizerService());

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);

            //Act & Assert
            Assert.Throws(typeof(Exception), () =>
            {
                engine.Start();
            });
        }


        [Test]
        public void Stop_WhenInvoked_InvokesCoreEngineExitMethod()
        {
            //Arrange
            var mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(mockCoreEngine.Object);

            var particleEngine = new ParticleEngine(new RandomizerService());

            var engine = new GraphicsEngine(mockEngineFactory.Object, particleEngine);

            //Act
            engine.Stop();

            //Assert
            mockCoreEngine.Verify(m => m.Exit(), Times.Once());
        }
        #endregion
    }
}

﻿using KDParticleEngine;
using KDParticleEngine.Services;
using Moq;
using NUnit.Framework;
using System;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class GraphicsEngineTests
    {
        private Mock<ICoreEngine> _mockCoreEngine;
        private ParticleEngine _particleEngine;
        private GraphicsEngine _engine;
        #region Prop Tests
        [Test]
        public void IsRunning_WhenGettingValueWhileEngineIsRunning_ReturnsTrue()
        {
            //Arrange
            _mockCoreEngine.SetupGet(p => p.IsRunning).Returns(true);

            //Act
            _engine.Play();

            //Assert
            Assert.IsTrue(_engine.IsRunning);
        }


        [Test]
        public void ParticleEngine_WhenGettingValue_ReturnsParticleEngine()
        {
            //Assert
            Assert.AreEqual(_particleEngine, _engine.ParticleEngine);
        }


        [Test]
        public void RenderSurfaceHandle_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _mockCoreEngine.SetupProperty(p => p.RenderSurfaceHandle);
            var expected = new IntPtr(5678);

            //Act
            _engine.RenderSurfaceHandle = new IntPtr(5678);

            var actual = _engine.RenderSurfaceHandle;

            //Assert
            Assert.AreEqual(expected, actual);
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
        [Test]
        public void Run_WhenInvoked_InvokesCoreEngineRunMethod()
        {
            //Arrange
            _mockCoreEngine.SetupGet(p => p.RenderSurfaceHandle).Returns(new IntPtr(1234));

            //Act
            _engine.Start();

            //Assert
            _mockCoreEngine.Verify(m => m.Run(), Times.Once());
        }


        [Test]
        public void Run_WhenInvokedWithoutSurfaceHandle_ThrowsException()
        {
            //Arrange
            _mockCoreEngine.SetupGet(p => p.RenderSurfaceHandle).Returns(IntPtr.Zero);

            //Assert
            Assert.Throws(typeof(Exception), () =>
            {
                _engine.Start();
            });
        }


        [Test]
        public void Stop_WhenInvoked_InvokesCoreEngineExitMethod()
        {
            //Act
            _engine.Stop();

            //Assert
            _mockCoreEngine.Verify(m => m.Exit(), Times.Once());
        }


        [Test]
        public void Pause_WhenInvoked_InvokesCoreEnginePauseMethod()
        {
            //Act
            _engine.Pause();

            //Assert
            _mockCoreEngine.Verify(m => m.Pause(), Times.Once());
        }
        #endregion


        #region Private Methods
        [SetUp]
        public void Setup()
        {
            _mockCoreEngine = new Mock<ICoreEngine>();

            var mockEngineFactory = new Mock<IGraphicsEngineFactory>();
            mockEngineFactory.SetupGet(p => p.CoreEngine).Returns(_mockCoreEngine.Object);

            _particleEngine = new ParticleEngine(new RandomizerService());

            _engine = new GraphicsEngine(mockEngineFactory.Object, _particleEngine);
        }


        [TearDown]
        public void TearDown()
        {
            _mockCoreEngine = null;
            _engine = null;
        }
        #endregion
    }
}

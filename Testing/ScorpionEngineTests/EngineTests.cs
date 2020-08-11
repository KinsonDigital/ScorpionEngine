// <copyright file="EngineTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="Engine"/> class.
    /// </summary>
    public class EngineTests : IDisposable
    {
        #region Private Fields
        private readonly Mock<IContentLoader> _mockContentLoader;
        private Mock<IEngineCore> _mockEngineCore;
        private readonly Mock<IKeyboard> _mockKeyboard;
        #endregion

        #region Constructors
        public EngineTests()
        {
            _mockKeyboard = new Mock<IKeyboard>();
            _mockContentLoader = new Mock<IContentLoader>();

            _mockEngineCore = new Mock<IEngineCore>();
            _mockEngineCore.Setup(m => m.IsRunning()).Returns(true);
            _mockEngineCore.SetupProperty(m => m.WindowWidth);
            _mockEngineCore.SetupProperty(m => m.WindowHeight);
        }
        #endregion

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_SetsContentLoader()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            // Act
            var actual = engine.ContentLoader;

            // Assert
            Assert.NotNull(actual);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void SceneManager_WhenGettingValue_IsNotNull()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            // Assert
            Assert.NotNull(engine.SceneManager);
        }

        [Fact]
        public void ContentLoader_WhenGettingValue_IsNotNull()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            // Assert
            Assert.NotNull(engine.ContentLoader);
        }

        [Fact]
        public void Running_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);
            var expected = true;

            // Act
            var actual = Engine.Running;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentFPS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);
            var expected = 62.5f;

            // Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            Engine.Start();
            engine.Update(engineTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WindowWidth_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var expected = 123;

            // Act
            Engine.WindowWidth = 123;
            var actual = Engine.WindowWidth;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WindowHeight_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var expected = 123;

            // Act
            Engine.WindowHeight = 123;
            var actual = Engine.WindowHeight;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Start_WhenInvoked_InvokesEngineCoreStart()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            // Act
            Engine.Start();

            // Assert
            _mockEngineCore.Verify(m => m.StartEngine(), Times.Once());
        }

        [Fact]
        public void Start_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            engine.SetFieldNull("engineCore");

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => Engine.Start());
        }

        [Fact]
        public void Stop_WhenInvoked_InvokesEngineCoreStop()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            // Act
            Engine.Stop();

            // Assert
            _mockEngineCore.Verify(m => m.StopEngine(), Times.Once());
        }

        [Fact]
        public void Stop_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            engine.SetFieldNull("engineCore");

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => Engine.Stop());
        }

        [Fact]
        public void Update_WhenInvokingWhileRunning_SetsCurrentFPSProp()
        {
            // Arrange
            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);
            var expected = 62.5f;

            // Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            engine.Update(engineTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWhileNotRunning_DoesNotSetCurrentFPSProp()
        {
            // Arrange
            _mockEngineCore.Setup(m => m.IsRunning()).Returns(false);

            var engine = new Engine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);
            var expected = 0f;
            Engine.CurrentFPS = 0;

            // Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            engine.Update(engineTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Dispose_WhenInvoking_DisposesEngineCore()
        {
            // Arrange
            var engine = new FakeEngine(_mockContentLoader.Object, _mockEngineCore.Object, _mockKeyboard.Object);

            // Act
            engine.Dispose();

            // Assert
            _mockEngineCore.Verify(m => m.Dispose(), Times.Once());
        }
        #endregion

        #region Public Methods       
        public void Dispose() => _mockEngineCore = null;
        #endregion
    }
}

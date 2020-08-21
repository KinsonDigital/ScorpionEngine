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
        private readonly Mock<IContentLoader> mockContentLoader;
        private readonly Mock<IKeyboard> mockKeyboard;
        private Mock<IEngineCore> mockEngineCore;
        #endregion

        public EngineTests()
        {
            this.mockKeyboard = new Mock<IKeyboard>();
            this.mockContentLoader = new Mock<IContentLoader>();

            this.mockEngineCore = new Mock<IEngineCore>();
            this.mockEngineCore.Setup(m => m.IsRunning()).Returns(true);
            this.mockEngineCore.SetupProperty(m => m.WindowWidth);
            this.mockEngineCore.SetupProperty(m => m.WindowHeight);
        }

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_SetsContentLoader()
        {
            // Arrange
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

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
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

            // Assert
            Assert.NotNull(engine.SceneManager);
        }

        [Fact]
        public void ContentLoader_WhenGettingValue_IsNotNull()
        {
            // Arrange
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

            // Assert
            Assert.NotNull(engine.ContentLoader);
        }

        [Fact]
        public void Running_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            _ = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);
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
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);
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
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

            // Act
            Engine.Start();

            // Assert
            this.mockEngineCore.Verify(m => m.StartEngine(), Times.Once());
        }

        [Fact]
        public void Start_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            // Arrange
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

            engine.SetFieldNull("engineCore");

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => Engine.Start());
        }

        [Fact]
        public void Stop_WhenInvoked_InvokesEngineCoreStop()
        {
            // Arrange
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

            // Act
            Engine.Stop();

            // Assert
            this.mockEngineCore.Verify(m => m.StopEngine(), Times.Once());
        }

        [Fact]
        public void Stop_WhenInvokedWithNullEngineCore_DoesNotThrowException()
        {
            // Arrange
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

            engine.SetFieldNull("engineCore");

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() => Engine.Stop());
        }

        [Fact]
        public void Update_WhenInvokingWhileRunning_SetsCurrentFPSProp()
        {
            // Arrange
            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);
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
            this.mockEngineCore.Setup(m => m.IsRunning()).Returns(false);

            var engine = new Engine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);
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
            var engine = new FakeEngine(this.mockContentLoader.Object, this.mockEngineCore.Object, this.mockKeyboard.Object);

            // Act
            engine.Dispose();

            // Assert
            this.mockEngineCore.Verify(m => m.Dispose(), Times.Once());
        }
        #endregion

        /// <inheritdoc/>
        public void Dispose() => this.mockEngineCore = null;
    }
}

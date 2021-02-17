// <copyright file="EngineTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Scene;
    using Moq;
    using Raptor.Content;
    using Raptor.Desktop;
    using Raptor.Input;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="Engine"/> class.
    /// </summary>
    public class EngineTests : IDisposable
    {
        private readonly Engine engine;
        private readonly Mock<IContentLoader> mockContentLoader;
        private readonly Mock<IWindow> mockGameWindow;
        private readonly Mock<IKeyboard> mockKeyBoard;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineTests"/> class.
        /// </summary>
        public EngineTests()
        {
            this.mockContentLoader = new Mock<IContentLoader>();

            this.mockGameWindow = new Mock<IWindow>();
            this.mockGameWindow.SetupGet(p => p.ContentLoader).Returns(this.mockContentLoader.Object);

            this.mockKeyBoard = new Mock<IKeyboard>();

            var manager = new SceneManager(this.mockContentLoader.Object, this.mockKeyBoard.Object);

            this.engine = new Engine(this.mockGameWindow.Object, manager);
        }

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_SetsContentLoader()
        {
            // Act
            var actual = this.engine.ContentLoader;

            // Assert
            Assert.NotNull(actual);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void SceneManager_WhenGettingValue_IsNotNull()
        {
            // Assert
            Assert.NotNull(this.engine.SceneManager);
        }

        [Fact]
        public void ContentLoader_WhenGettingValue_IsNotNull()
        {
            // Assert
            Assert.NotNull(this.engine.ContentLoader);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWhileRunning_SetsCurrentFPSProp()
        {
            // Arrange
            var expected = 62.5f;

            this.engine.RunAsync();

            var gameTime = new GameTime();
            gameTime.UpdateTotalGameTime(16);

            // Act
            this.engine.Update(gameTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWhileNotRunning_DoesNotSetCurrentFPSProp()
        {
            // Arrange
            var expected = 0f;

            //engine.RunAsync();

            var gameTime = new GameTime();
            gameTime.UpdateTotalGameTime(16);

            // Act
            this.engine.Update(gameTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        /// <inheritdoc/>
        public void Dispose()
        {
            Engine.CurrentFPS = 0;
        }
    }
}

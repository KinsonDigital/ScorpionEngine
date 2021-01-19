// <copyright file="EngineTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using KDScorpionEngine;
    using Moq;
    using Raptor.Content;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="Engine"/> class.
    /// </summary>
    public class EngineTests
    {
        #region Private Fields
        private readonly Mock<IContentLoader> mockContentLoader;
        #endregion

        public EngineTests()
        {
            this.mockContentLoader = new Mock<IContentLoader>();
        }

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_SetsContentLoader()
        {
            // Arrange
            var engine = new Engine(10, 20);

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
            var engine = new Engine(10, 20);

            // Assert
            Assert.NotNull(engine.SceneManager);
        }

        [Fact]
        public void ContentLoader_WhenGettingValue_IsNotNull()
        {
            // Arrange
            var engine = new Engine(10, 20);

            // Assert
            Assert.NotNull(engine.ContentLoader);
        }

        [Fact]
        public void CurrentFPS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var engine = new Engine(10, 20);
            var expected = 62.5f;

            // Act
            var gameTime = new GameTime();
            gameTime.UpdateTotalGameTime(16);
            engine.Update(gameTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWhileRunning_SetsCurrentFPSProp()
        {
            // Arrange
            var engine = new Engine(10, 20);
            var expected = 62.5f;

            // Act
            var gameTime = new GameTime();
            gameTime.UpdateTotalGameTime(16);
            engine.Update(gameTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWhileNotRunning_DoesNotSetCurrentFPSProp()
        {
            // Arrange
            var engine = new Engine(10, 20);
            var expected = 0f;
            Engine.CurrentFPS = 0;

            // Act
            var gameTime = new GameTime();
            gameTime.UpdateTotalGameTime(16);
            engine.Update(gameTime);
            var actual = Engine.CurrentFPS;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

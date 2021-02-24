// <copyright file="GameWindowTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using Moq;
    using Raptor;
    using Raptor.Content;
    using Raptor.Desktop;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="GameWindow"/> class.
    /// </summary>
    public class GameWindowTests
    {
        private readonly Mock<IContentLoader> mockContentLoader;
        private readonly Mock<IWindow> mockWindow;
        private readonly Mock<IRenderer> mockRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowTests"/> class.
        /// </summary>
        public GameWindowTests()
        {
            this.mockContentLoader = new Mock<IContentLoader>();

            this.mockWindow = new Mock<IWindow>();
            this.mockWindow.SetupGet(p => p.ContentLoader).Returns(this.mockContentLoader.Object);

            this.mockRenderer = new Mock<IRenderer>();
        }

        #region Method tests
        [Fact]
        public void OnLoad_WhenInvoked_PerformsLoad()
        {
            // Arrange
            var initActionInvoked = false;
            var loadActionInvoked = false;
            var window = CreateWindow();
            window.InitAction = () => initActionInvoked = true;
            window.LoadAction = () => loadActionInvoked = true;

            // Act
            window.OnLoad();

            // Assert
            Assert.True(initActionInvoked);
            Assert.True(loadActionInvoked);
        }

        [Fact]
        public void OnLoad_WithNullInitAndLoadActions_DoesNotThrowException()
        {
            // Arrange
            var window = CreateWindow();

            // Act & Assert
            AssertHelpers.DoesNotThrow<NullReferenceException>(() =>
            {
                window.OnLoad();
            });
        }

        [Fact]
        public void OnUpdate_WhenInvoked_PerformsUpdate()
        {
            // Arrange
            var window = CreateWindow();
            var updateActionInvoked = false;
            var millisecondsPassed = 0;

            window.UpdateAction = (gameTime) =>
            {
                millisecondsPassed = gameTime.CurrentFrameElapsed;
                updateActionInvoked = true;
            };

            // Act
            window.OnUpdate(new FrameTime() { ElapsedTime = new TimeSpan(0, 0, 0, 0, 16) });

            // Assert
            Assert.True(updateActionInvoked);
            Assert.Equal(16, millisecondsPassed);
        }

        [Fact]
        public void OnUpdate_WithNullUpdateAction_DoesNotThrowException()
        {
            // Arrange
            var window = CreateWindow();

            // Act & Assert
            AssertHelpers.DoesNotThrow<NullReferenceException>(() =>
            {
                window.OnUpdate(new FrameTime() { ElapsedTime = new TimeSpan(0, 0, 0, 0, 16) });
            });
        }

        [Fact]
        public void OnDraw_WhenInvoked_PerformsDraw()
        {
            // Arrange
            var window = CreateWindow();
            var renderActionInvoked = false;
            window.RenderAction = (renderer) => renderActionInvoked = true;

            // Act
            window.OnDraw(new FrameTime() { ElapsedTime = new TimeSpan(0, 0, 0, 0, 16) });

            // Assert
            Assert.True(renderActionInvoked);
        }

        [Fact]
        public void OnUpdate_WithNullRenderAction_DoesNotThrowException()
        {
            // Arrange
            var window = CreateWindow();

            // Act & Assert
            AssertHelpers.DoesNotThrow<NullReferenceException>(() =>
            {
                window.OnDraw(new FrameTime() { ElapsedTime = new TimeSpan(0, 0, 0, 0, 16) });
            });
        }

        [Fact]
        public void Dispose_WhenInvoked_DisposesOfWindow()
        {
            // Arrange
            var window = CreateWindow();

            // Act
            window.Dispose();
            window.Dispose();

            // Assert
            this.mockWindow.Verify(m => m.Dispose(), Times.Once());
        }
        #endregion

        /// <summary>
        /// Creats a new instance of <see cref="GameWindow"/> for the purpose of testing.
        /// </summary>
        /// <returns>The instance to test.</returns>
        private GameWindow CreateWindow() => new GameWindow(this.mockWindow.Object, this.mockRenderer.Object);
    }
}

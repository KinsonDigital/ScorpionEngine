// <copyright file="EngineTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Moq;
    using Raptor;
    using Raptor.Content;
    using Raptor.Input;
    using Raptor.UI;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="Engine"/> class.
    /// </summary>
    public class EngineTests
    {
        private readonly Mock<IContentLoader> mockContentLoader;
        private readonly Mock<IWindow> mockGameWindow;
        private readonly Mock<IGameInput<KeyCode, KeyboardState>> mockKeyboard;
        private readonly Mock<IRenderer> mockRenderer;
        private readonly SceneManager manager;
        private readonly GameWindow gameWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineTests"/> class.
        /// </summary>
        public EngineTests()
        {
            this.mockContentLoader = new Mock<IContentLoader>();

            this.mockKeyboard = new Mock<IGameInput<KeyCode, KeyboardState>>();
            this.manager = new SceneManager(this.mockContentLoader.Object, this.mockKeyboard.Object);

            this.mockGameWindow = new Mock<IWindow>();
            this.mockGameWindow.SetupGet(p => p.ContentLoader).Returns(this.mockContentLoader.Object);
            this.mockGameWindow.SetupProperty(p => p.Width);
            this.mockGameWindow.SetupProperty(p => p.Height);

            this.mockRenderer = new Mock<IRenderer>();

            this.gameWindow = new GameWindow(this.mockGameWindow.Object, this.mockRenderer.Object);
        }

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_SetsContentLoader()
        {
            // Arrange
            var engine = CreateEngine();

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
            var engine = CreateEngine();

            // Assert
            Assert.NotNull(engine.SceneManager);
        }

        [Fact]
        public void ContentLoader_WhenGettingValue_IsNotNull()
        {
            // Arrange
            var engine = CreateEngine();

            // Assert
            Assert.NotNull(engine.ContentLoader);
        }

        [Fact]
        public void WindowWidth_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var engine = CreateEngine();

            // Act
            engine.WindowWidth = 100;
            var actual = engine.WindowWidth;

            // Assert
            Assert.Equal(100, actual);
        }

        [Fact]
        public void WindowHeight_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var engine = CreateEngine();

            // Act
            engine.WindowHeight = 200;
            var actual = engine.WindowHeight;

            // Assert
            Assert.Equal(200, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWhileRunning_SetsCurrentFPSProp()
        {
            // Arrange
            this.mockGameWindow.SetupGet(p => p.Update)
                .Returns((frameTime) =>
                {
                    var gameTime = new GameTime();
                    gameTime.AddTime(frameTime.ElapsedTime.Milliseconds);
                    this.gameWindow.UpdateAction(gameTime);
                });

            var engine = CreateEngine();
            var expected = 62.5f;

            engine.RunAsync();

            // Act
            // This simulates that the underlying system calls update.
            this.mockGameWindow.Object.Update(new FrameTime() { ElapsedTime = new TimeSpan(0, 0, 0, 0, 16) });

            // Assert
            Assert.Equal(expected, Engine.CurrentFPS);
        }

        [Fact]
        public void Update_WhenInvokedWhileNotRunning_DoesNotSetCurrentFPSProp()
        {
            // Arrange
            this.mockGameWindow.SetupGet(p => p.Update)
                .Returns((frameTime) =>
                {
                    var gameTime = new GameTime();
                    gameTime.AddTime(frameTime.ElapsedTime.Milliseconds);
                    this.gameWindow.UpdateAction(gameTime);
                });

            var engine = CreateEngine();
            Engine.CurrentFPS = 1234;

            var expected = 1234f;
            engine.Play();
            engine.Pause();

            // Act
            this.mockGameWindow.Object.Update(new FrameTime() { ElapsedTime = new TimeSpan(0, 0, 0, 0, 16) });

            // Assert
            Assert.Equal(expected, Engine.CurrentFPS);
        }

        [Fact]
        public void RunAsync_WhenInvoked_AsynchronouslyShowsInternalWindowImplementation()
        {
            // Arrange
            var engine = CreateEngine();

            // Act
            engine.RunAsync();

            // Assert
            this.mockGameWindow.Verify(m => m.ShowAsync(It.IsAny<Action>()), Times.Once());
        }

        [Fact]
        public void RunAsync_WhenEngineIsDisposed_DisposesOfInternalWindowImplementation()
        {
            // Arrange
            var engine = CreateEngine();

            this.mockGameWindow.Setup(m => m.ShowAsync(It.IsAny<Action>()))
                .Callback<Action>((dispose) => dispose());

            engine.RunAsync();

            // Act
            this.mockGameWindow.Object.ShowAsync(() => { });

            // Assert
            this.mockGameWindow.Verify(m => m.Dispose(), Times.Once());
        }

        [Fact]
        public void Play_WhenInvoked_SetsRunningToTrue()
        {
            // Arrange
            var engine = CreateEngine();

            // Act
            engine.Play();

            // Assert
            Assert.True(engine.IsRunning);
        }

        [Fact]
        public void Pause_WhenInvoked_SetsRunningToFalse()
        {
            // Arrange
            var engine = CreateEngine();
            engine.Play();

            // Act
            engine.Pause();

            // Assert
            Assert.False(engine.IsRunning);
        }

        [Fact]
        public void Init_WhenInvoked_InitializesScenes()
        {
            // Arrange
            var engine = CreateEngine();

            var mockScene = new Mock<IScene>();
            this.manager.Add(mockScene.Object);

            // Act
            // This simulates that the underlying system calls initialize.
            engine.Init();

            // Assert
            mockScene.Verify(m => m.Initialize(), Times.Once());
        }

        [Fact]
        public void LoadContent_WhenInvoked_LoadsContent()
        {
            // Arrange
            var engine = CreateEngine();

            var mockScene = new Mock<IScene>();
            mockScene.SetupGet(p => p.Active).Returns(true);

            this.manager.Add(mockScene.Object);

            // Act
            // NOTE: The internal IWindow implementation is what actually calls the LoadAction
            // after the IniAction has been called.
            engine.LoadContent(this.mockContentLoader.Object);

            // Assert
            mockScene.Verify(m => m.LoadContent(this.mockContentLoader.Object), Times.Once());
        }

        [Fact]
        public void Render_WhenInvoked_RendersTheScene()
        {
            // Arrange
            this.mockGameWindow.SetupGet(p => p.Draw)
                .Returns((frameTime) =>
                {
                    this.gameWindow.RenderAction(this.mockRenderer.Object);
                });

            var mockScene = new Mock<IScene>();
            var engine = CreateEngine();

            this.manager.Add(mockScene.Object);

            // Act
            this.mockGameWindow.Object.Draw(new FrameTime() { ElapsedTime = new TimeSpan(0, 0, 0, 0, 16) });

            // Assert
            this.mockRenderer.Verify(m => m.Clear(), Times.Once());
            this.mockRenderer.Verify(m => m.Begin(), Times.Once());
            mockScene.Verify(m => m.Render(this.mockRenderer.Object), Times.Once());
            this.mockRenderer.Verify(m => m.End(), Times.Once());
        }

        [Fact]
        public void Dispose_WhenInvoked_DisposesOfEngine()
        {
            // Arrange
            var engine = CreateEngine();

            // Act
            engine.Dispose();
            engine.Dispose();

            // Assert
            this.mockGameWindow.Verify(m => m.Dispose(), Times.Once());
        }
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="Engine"/> for the purpose of testing.
        /// </summary>
        /// <returns>The instance to test.</returns>
        private Engine CreateEngine() => new Engine(this.gameWindow, this.manager);
    }
}

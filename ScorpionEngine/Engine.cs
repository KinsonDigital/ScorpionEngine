// <copyright file="Engine.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Raptor.Content;
    using Raptor.Desktop;
    using Raptor.Factories;
    using Raptor.Graphics;
    using Raptor.Input;

    /// <summary>
    /// Drives and manages the game.
    /// </summary>
    public class Engine : IDisposable
    {
        private readonly GameWindow gameWindow;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="gameWindow">The game window that provides user interativity and rendering of the game.</param>
        /// <param name="sceneManager">The scene manager.</param>
        public Engine(GameWindow gameWindow, SceneManager sceneManager)
        {
            this.gameWindow = gameWindow;

            gameWindow.InitAction = InitAction;
            gameWindow.LoadAction = LoadAction;
            gameWindow.UpdateAction = UpdateAction;
            gameWindow.RenderAction = RenderAction;

            SceneManager = sceneManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="windowWidth">The width of the game window.</param>
        /// <param name="windowHeight">The height of the game window.</param>
        [ExcludeFromCodeCoverage]
        public Engine(int windowWidth, int windowHeight)
        {
            IoC.Init();

            SceneManager = new SceneManager(ContentLoaderFactory.CreateContentLoader(), new Keyboard());

            this.gameWindow = SetupWindow(
                WindowFactory.CreateWindow(windowWidth, windowHeight),
                new Renderer(windowWidth, windowHeight));
        }

        /// <summary>
        /// Gets the FPS that the engine is currently running at.
        /// </summary>
        public static float CurrentFPS { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the engine is running or paused.
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public int WindowWidth
        {
            get => this.gameWindow.Width;
            set => this.gameWindow.Width = value;
        }

        /// <summary>
        /// Gets or sets the height of the game window.
        /// </summary>
        public int WindowHeight
        {
            get => this.gameWindow.Height;
            set => this.gameWindow.Height = value;
        }

        /// <summary>
        /// Gets the <see cref="SceneManager"/> used to manage a game's scenes.
        /// </summary>
        public SceneManager SceneManager { get; private set; }

        /// <summary>
        /// Gets the <see cref="ContentLoader"/> used to load and unload the games content.
        /// </summary>
        public IContentLoader ContentLoader => this.gameWindow.ContentLoader;

        /// <summary>
        /// Runs the engine on another thread.
        /// </summary>
        /// <returns>The task/thread that the engine is running on.</returns>
        public Task RunAsync()
        {
            IsRunning = true;
            return this.gameWindow.ShowAsync(() =>
            {
                this.gameWindow.Dispose();
            });
        }

        /// <summary>
        /// Plays the engine.
        /// </summary>
        public void Play() => IsRunning = true;

        /// <summary>
        /// Pauses the engine.
        /// </summary>
        public void Pause() => IsRunning = false;

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public virtual void Init()
        {
            foreach (var scene in SceneManager)
            {
                scene.Initialize();
            }
        }

        /// <summary>
        /// Loads all of the content.
        /// </summary>
        /// <param name="contentLoader">Loads content.</param>
        public virtual void LoadContent(IContentLoader contentLoader)
        {
            foreach (var scene in SceneManager)
            {
                if (scene.Active)
                {
                    scene.LoadContent(contentLoader);

                    foreach (var entity in scene.Entities)
                    {
                        entity.LoadContent(contentLoader);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!IsRunning)
            {
                return;
            }

            CurrentFPS = 1000f / gameTime.CurrentFrameElapsed;
            SceneManager.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        /// <param name="renderer">The renderer used to render the graphics.</param>
        public virtual void Render(IRenderer renderer) => SceneManager.Render(renderer);

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.gameWindow.Dispose();
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Sets up the window.
        /// </summary>
        /// <param name="raptorWindow">The Raptor window object.</param>
        [ExcludeFromCodeCoverage]
        private GameWindow SetupWindow(IWindow raptorWindow, IRenderer renderer)
            => new GameWindow(raptorWindow, renderer)
            {
                InitAction = InitAction,
                LoadAction = LoadAction,
                UpdateAction = UpdateAction,
                RenderAction = RenderAction,
            };

        /// <summary>
        /// Occurs one time during game initialization. This event is fired before the <see cref="OnLoadContent"/> event is fired. Add initialization code here.
        /// </summary>
        private void InitAction() => Init();

        /// <summary>
        /// Occurs one time during game initialization after the <see cref="OnInitialize"/> event is fired.
        /// </summary>
        private void LoadAction() => LoadContent(ContentLoader);

        /// <summary>
        /// Occurs once every frame before the OnDraw event before the <see cref="OnRender"/> event is invoked.
        /// </summary>
        private void UpdateAction(GameTime gameTime) => Update(gameTime);

        /// <summary>
        /// Occurs once every frame after the <see cref="OnUpdate"/> event has been invoked.
        /// </summary>
        private void RenderAction(IRenderer renderer)
        {
            renderer.Clear();

            renderer.Begin();

            Render(renderer);

            renderer.End();
        }
    }
}

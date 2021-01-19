// <copyright file="Engine.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Raptor.Content;
    using Raptor.Factories;
    using Raptor.Graphics;

    /// <summary>
    /// Drives and manages the game.
    /// </summary>
    public class Engine : IDisposable
    {
        private GameWindow gameWindow;
        private ISpriteBatch spriteBatch;
        private static int prevElapsedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public Engine(int windowWidth, int windowHeight)
        {
            ContentLoader = ContentLoaderFactory.CreateContentLoader();
            SceneManager = new SceneManager(ContentLoader);

            SetupWindow(windowWidth, windowHeight);
        }

        /// <summary>
        /// Gets the FPS that the engine is currently running at.
        /// </summary>
        public static float CurrentFPS { get; internal set; }

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
        public IContentLoader ContentLoader { get; private set; }

        public void Run()
        {
            Init();

            this.gameWindow.ShowAsync(() =>
            {
                this.gameWindow.Dispose();
            }).Wait();
        }

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        public virtual void LoadContent(IContentLoader contentLoader)
        {
            foreach (var scene in SceneManager)
            {
                if (scene.Active)
                {
                    scene.LoadContent(contentLoader);
                }
            }
        }

        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public virtual void Update(GameTime gameTime) => SceneManager.Update(gameTime);

        /// <summary>
        /// Draws the game world.
        /// </summary>
        /// <param name="renderer">The renderer used to render the graphics.</param>
        [ExcludeFromCodeCoverage]
        public virtual void Render(Renderer renderer) => SceneManager.Render(renderer);

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
        protected virtual void Dispose(bool disposing) => this.gameWindow.Dispose();

        /// <summary>
        /// Sets up the window.
        /// </summary>
        private void SetupWindow(int windowWidth, int windowHeight)
            => this.gameWindow = new GameWindow(WindowFactory.CreateWindow(windowWidth, windowHeight))
            {
                InitAction = InitAction,
                LoadAction = LoadAction,
                UpdateAction = UpdateAction,
                RenderAction = RenderAction,
            };

        /// <summary>
        /// Occurs one time during game initialization. This event is fired before the <see cref="OnLoadContent"/> event is fired. Add initialization code here.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void InitAction() => Init();
            // TODO: Get this working
            // _renderer = new GameRenderer()
            // {
            //    InternalRenderer = _engineCore.Renderer
            // };

        /// <summary>
        /// Occurs one time during game initialization after the <see cref="OnInitialize"/> event is fired.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void LoadAction() => LoadContent(ContentLoader);

        /// <summary>
        /// Occurs once every frame before the OnDraw event before the <see cref="OnRender"/> event is invoked.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void UpdateAction(GameTime gameTime)
        {
            Update(gameTime);
        }

        /// <summary>
        /// Occurs once every frame after the <see cref="OnUpdate"/> event has been invoked.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void RenderAction(Renderer renderer)
        {
            renderer.Clear();

            renderer.Begin();

            Render(renderer);

            renderer.End();
        }
    }
}

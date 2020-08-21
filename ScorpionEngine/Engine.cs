// <copyright file="Engine.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Raptor;
    using Raptor.Content;
    using Raptor.Plugins;

    /// <summary>
    /// Drives and manages the game.
    /// </summary>
    public class Engine : IDisposable
    {
        private readonly GameRenderer renderer;
        private static IEngineCore engineCore;
        private static int prevElapsedTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public Engine()
        {
            // TODO: This needs to be dealt with because Core does not exist anymore
            // SetupEngineCore(Core.Start());
            ContentLoader = new ContentLoader();
            SceneManager = new SceneManager(ContentLoader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="contentLoader">The content loader to inject.</param>
        /// <param name="engineCore">The engine core to inject.</param>
        /// <param name="keyboard">The keyboard to inject.</param>.
        internal Engine(IContentLoader contentLoader, IEngineCore engineCore, IKeyboard keyboard)
        {
            ContentLoader = new ContentLoader(contentLoader);
            SceneManager = new SceneManager(contentLoader, keyboard);

            SetupEngineCore(engineCore);
        }

        /// <summary>
        /// Gets a value indicating whether the game engine is currently running.
        /// </summary>
        public static bool Running => engineCore.IsRunning();

        /// <summary>
        /// Gets the FPS that the engine is currently running at.
        /// </summary>
        public static float CurrentFPS { get; internal set; }

        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public static int WindowWidth
        {
            get => engineCore.WindowWidth;
            set => engineCore.WindowWidth = value;
        }

        /// <summary>
        /// Gets or sets the height of the game window.
        /// </summary>
        public static int WindowHeight
        {
            get => engineCore.WindowHeight;
            set => engineCore.WindowHeight = value;
        }

        /// <summary>
        /// Gets the <see cref="SceneManager"/> used to manage a game's scenes.
        /// </summary>
        public SceneManager SceneManager { get; private set; }

        /// <summary>
        /// Gets the <see cref="ContentLoader"/> used to load and unload the games content.
        /// </summary>
        public ContentLoader ContentLoader { get; private set; }

        /// <summary>
        /// Starts the game engine.
        /// </summary>
        public static void Start() => engineCore?.StartEngine();

        /// <summary>
        /// Stops the game engine.
        /// </summary>
        public static void Stop() => engineCore?.StopEngine();

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual void Init()
        {
        }

        /// <summary>
        /// Loads all of the content.
        /// </summary>
        /// <param name="contentLoader">Loads content.</param>
        [ExcludeFromCodeCoverage]
        public virtual void LoadContent(ContentLoader contentLoader)
        {
        }

        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public virtual void Update(EngineTime engineTime)
        {
            var currentTime = engineTime.ElapsedEngineTime.Milliseconds;

            if (!Running)
            {
                return; // If the engine has not been started, exit
            }

            prevElapsedTime = currentTime;

            CurrentFPS = 1000f / prevElapsedTime;

            SceneManager.Update(engineTime);
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        /// <param name="renderer">The renderer used to render the graphics.</param>
        [ExcludeFromCodeCoverage]
        public virtual void Render(GameRenderer renderer) => SceneManager.Render(renderer);

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing">True dispose of managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (engineCore != null)
            {
                engineCore.Dispose();
            }
        }

        /// <summary>
        /// Sets up the core of the engine.
        /// </summary>
        private void SetupEngineCore(IEngineCore engineCore)
        {
            Engine.engineCore = engineCore;
            Engine.engineCore.SetFPS(60);
            Engine.engineCore.OnInitialize += EngineCore_OnInitialize;
            Engine.engineCore.OnLoadContent += EngineCore_OnLoadContent;
            Engine.engineCore.OnUpdate += EngineCore_OnUpdate;
            Engine.engineCore.OnRender += EngineCore_OnRender;
        }

        /// <summary>
        /// Occurs one time during game initialization. This event is fired before the <see cref="OnLoadContent"/> event is fired. Add initialization code here.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void EngineCore_OnInitialize(object sender, EventArgs e) =>
            // TODO: Get this working
            // _renderer = new GameRenderer()
            // {
            //    InternalRenderer = _engineCore.Renderer
            // };
            Init();

        /// <summary>
        /// Occurs one time during game initialization after the <see cref="OnInitialize"/> event is fired.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void EngineCore_OnLoadContent(object sender, EventArgs e) => LoadContent(ContentLoader);

        /// <summary>
        /// Occurs once every frame before the OnDraw event before the <see cref="OnRender"/> event is invoked.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void EngineCore_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            var engineTime = new EngineTime()
            {
                ElapsedEngineTime = e.EngineTime.ElapsedEngineTime,
                TotalEngineTime = e.EngineTime.TotalEngineTime,
            };

            Update(engineTime);
        }

        /// <summary>
        /// Occurs once every frame after the <see cref="OnUpdate"/> event has been invoked.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void EngineCore_OnRender(object sender, OnRenderEventArgs e)
        {
            this.renderer.Clear(255, 50, 50, 50);

            this.renderer.Begin();

            Render(this.renderer);

            this.renderer.End();
        }
    }
}

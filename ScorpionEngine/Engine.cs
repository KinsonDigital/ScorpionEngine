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
        private static IEngineCore engineCore;
        private static int prevElapsedTime;
        private GameRenderer renderer;

        /// <summary>
        /// Creates a new instance of <see cref="Engine"/>.
        /// <paramref name="contentLoader">The content loader to inject.</paramref>
        /// <paramref name="engineCore">The engine core to inject.</paramref>
        /// <paramref name="keyboard">The keyboard to inject.</paramref>
        /// USED FOR UNIT TESTING.
        /// </summary>
        internal Engine(IContentLoader contentLoader, IEngineCore engineCore, IKeyboard keyboard)
        {
            ContentLoader = new ContentLoader(contentLoader);
            SceneManager = new SceneManager(contentLoader, keyboard);

            SetupEngineCore(engineCore);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Engine"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public Engine()
        {
            //TODO: This needs to be dealt with because Core does not exist anymore
            //SetupEngineCore(Core.Start());
            ContentLoader = new ContentLoader();
            SceneManager = new SceneManager(ContentLoader);
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
        /// Gets a value indicating that the game engine is currently running.
        /// </summary>
        public bool Running => engineCore.IsRunning();

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
        /// Starts the game engine.
        /// </summary>
        public void Start() => engineCore?.StartEngine();

        /// <summary>
        /// Stops the game engine.
        /// </summary>
        public void Stop() => engineCore?.StopEngine();

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual void Init() { }

        /// <summary>
        /// Loads all of the content.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual void LoadContent(ContentLoader contentLoader) { }

        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public virtual void Update(EngineTime engineTime)
        {
            var currentTime = engineTime.ElapsedEngineTime.Milliseconds;

            if (!Running) return;//If the engine has not been started, exit

            prevElapsedTime = currentTime;

            CurrentFPS = 1000f / prevElapsedTime;

            SceneManager.Update(engineTime);
        }

        /// <summary>
        /// Draws the game world.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual void Render(GameRenderer renderer) => SceneManager.Render(renderer);

        /// <summary>
        /// Disposes of the engine.
        /// </summary>
        public void Dispose() => Dispose(true);

        /// <summary>
        /// Disposes of the internal engine components.
        /// </summary>
        /// <param name="disposing">True if the internal engine components should be disposed of.</param>
        private static void Dispose(bool _)
        {
            if (engineCore != null)
                engineCore.Dispose();
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
        private void EngineCore_OnInitialize(object sender, EventArgs e)
        {
            //TODO: Get this working
            //_renderer = new GameRenderer()
            //{
            //    InternalRenderer = _engineCore.Renderer
            //};
            Init();
        }

        /// <summary>
        /// Occurs one time during game intialization after the <see cref="OnInitialize"/> event is fired.
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
                TotalEngineTime = e.EngineTime.TotalEngineTime
            };

            Update(engineTime);
        }

        /// <summary>
        /// Occurs once every frame after the <see cref="OnUpdate"/> event has been been invoked.
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

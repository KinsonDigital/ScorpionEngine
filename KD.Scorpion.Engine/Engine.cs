﻿using System;
using KDScorpionCore;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Scene;
using System.Diagnostics.CodeAnalysis;
using KDScorpionCore.Content;
using KDScorpionEngine.Graphics;


namespace KDScorpionEngine
{
    /// <summary>
    /// Drives and manages the game.
    /// </summary>
    public class Engine : IDisposable
    {
        #region Fields
        private static IEngineCore _engineCore;
        private static int _prevElapsedTime;
        private GameRenderer _renderer;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Engine"/> for the purpose of unit testing.
        /// <paramref name="contentLoader">The content loader.</paramref>
        /// <paramref name="engineCore">The engine core.</paramref>
        /// <paramref name="keyboard">The keyboard.</paramref>
        /// <paramref name="loadPhysicsLibrary">True if the physics library should be loaded.</paramref>
        /// </summary>
        internal Engine(IContentLoader contentLoader, IEngineCore engineCore, IKeyboard keyboard, bool loadPhysicsLibrary = true)
        {
            ContentLoader = new ContentLoader(contentLoader);
            SceneManager = new SceneManager(contentLoader, keyboard);

            SetupEngineCore(engineCore);
        }


        /// <summary>
        /// Creates an instance of engine.
        /// </summary>
        /// <paramref name="loadPhysicsLibrary">True if the physics library should be loaded.</paramref>
        [ExcludeFromCodeCoverage]
        public Engine(bool loadPhysicsLibrary = true)
        {
            SetupEngineCore(Core.Start());
            ContentLoader = new ContentLoader();
            SceneManager = new SceneManager(ContentLoader);
        }
        #endregion


        #region Props
        public SceneManager SceneManager { get; private set; }

        public ContentLoader ContentLoader { get; private set; }

        /// <summary>
        /// Gets a value indicating that the game engine is currently running.
        /// </summary>
        public bool Running => _engineCore.IsRunning();

        /// <summary>
        /// Gets the FPS that the engine is currently running at.
        /// </summary>
        public static float CurrentFPS { get; internal set; }

        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public static int WindowWidth
        {
            get => _engineCore.WindowWidth;
            set => _engineCore.WindowWidth = value;
        }

        /// <summary>
        /// Gets or sets the height of the game window.
        /// </summary>
        public static int WindowHeight
        {
            get => _engineCore.WindowHeight;
            set => _engineCore.WindowHeight = value;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the game engine.
        /// </summary>
        public void Start()
        {
            _engineCore?.Start();
        }


        public void Stop()
        {
            _engineCore?.Stop();
        }


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
        [ExcludeFromCodeCoverage]
        public virtual void LoadContent(ContentLoader contentLoader)
        {
        }


        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="engineTime">The time passed since last frame and game start.</param>
        public virtual void Update(EngineTime engineTime)
        {
            var currentTime = engineTime.ElapsedEngineTime.Milliseconds;

            if (!Running) return;//If the engine has not been started, exit

            _prevElapsedTime = currentTime;

            CurrentFPS = 1000f / _prevElapsedTime;

            SceneManager.Update(engineTime);
        }


        /// <summary>
        /// Draws the game world.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public virtual void Render(GameRenderer renderer)
        {
            SceneManager.Render(renderer);
        }


        /// <summary>
        /// Disposes of the engine.
        /// </summary>
        public void Dispose() => Dispose(true);
        #endregion


        #region Protected Methods
        /// <summary>
        /// Disposes of the internal engine components.
        /// </summary>
        /// <param name="disposing">True if the internal engine components should be disposed of.</param>
        private static void Dispose(bool _)
        {
            if (_engineCore != null)
                _engineCore.Dispose();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the engine.
        /// </summary>
        private void SetupEngineCore(IEngineCore engineCore)
        {
            _engineCore = engineCore;
            _engineCore.SetFPS(60);
            _engineCore.OnInitialize += EngineCore_OnInitialize;
            _engineCore.OnLoadContent += EngineCore_OnLoadContent;
            _engineCore.OnUpdate += EngineCore_OnUpdate;
            _engineCore.OnRender += EngineCore_OnRender;
        }
        

        [ExcludeFromCodeCoverage]
        private void EngineCore_OnInitialize(object sender, EventArgs e)
        {
            _renderer = new GameRenderer
            {
                InternalRenderer = _engineCore.Renderer
            };
            Init();
        }


        [ExcludeFromCodeCoverage]
        private void EngineCore_OnLoadContent(object sender, EventArgs e)
        {
            LoadContent(ContentLoader);
        }


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


        [ExcludeFromCodeCoverage]
        private void EngineCore_OnRender(object sender, OnRenderEventArgs e)
        {
            _renderer.Clear(255, 50, 50, 50);

            _renderer.Begin();

            Render(_renderer);

            _renderer.End();
        }
        #endregion
    }
}
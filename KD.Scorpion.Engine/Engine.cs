﻿using System;
using KDScorpionCore;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Scene;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using KDScorpionCore.Graphics;
using KDScorpionCore.Content;

[assembly: InternalsVisibleTo(assemblyName: "ScorpionEngine.Tests", AllInternalsVisible = true)]

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
        private Renderer _renderer;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Engine"/> for the purpose of unit testing.
        /// <paramref name="enginePluginLib">The plugin library used to mock engine plugins.</paramref>
        /// <paramref name="physicsPluginLib">The plugin library used to mock physics plugins.</paramref>
        /// </summary>
        internal Engine(IPluginLibrary enginePluginLib, IPluginLibrary physicsPluginLib, bool loadPhysicsLibrary = true)
        {
            PluginSystem.LoadEnginePluginLibrary(enginePluginLib);

            if (loadPhysicsLibrary)
                PluginSystem.LoadPhysicsPluginLibrary(physicsPluginLib);

            //Make sure that the Setup() method is called
            //This is to make sure that this class is testable for unit testing purposes.
            Setup();
        }


        /// <summary>
        /// Creates an instance of engine.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public Engine(bool loadPhysicsLibrary = true)
        {
            PluginSystem.LoadEnginePluginLibrary(new PluginLibrary("MonoScorpPlugin"));

            if(loadPhysicsLibrary)
                PluginSystem.LoadPhysicsPluginLibrary(new PluginLibrary("VelcroPhysicsPlugin"));

            Setup();
        }
        #endregion


        #region Properties
        public SceneManager SceneManager { get; set; }

        public ContentLoader ContentLoader { get; set; }

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
        public virtual void Render(Renderer renderer)
        {
            SceneManager.Render(renderer);
        }


        /// <summary>
        /// Disposes of the engine.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Disposes of the internal engine components.
        /// </summary>
        /// <param name="disposing">True if the internal engine components should be disposed of.</param>
        private static void Dispose(bool disposing)
        {
            if (_engineCore != null)
                _engineCore.Dispose();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the engine.
        /// </summary>
        private void Setup()
        {
            ContentLoader = new ContentLoader(PluginSystem.EnginePlugins.LoadPlugin<IContentLoader>());
            _engineCore = PluginSystem.EnginePlugins.LoadPlugin<IEngineCore>();

            _engineCore.SetFPS(60);

            _engineCore.OnInitialize += _engineCore_OnInitialize;
            _engineCore.OnLoadContent += _engineCore_OnLoadContent;
            _engineCore.OnUpdate += _engineCore_OnUpdate;
            _engineCore.OnRender += _engineCore_OnRender;

            SceneManager = new SceneManager(ContentLoader);
        }


        private void _engineCore_OnInitialize(object sender, EventArgs e)
        {
            Init();
        }


        private void _engineCore_OnLoadContent(object sender, EventArgs e)
        {
            LoadContent(ContentLoader);
        }


        private void _engineCore_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            var engineTime = new EngineTime()
            {
                ElapsedEngineTime = e.EngineTime.ElapsedEngineTime,
                TotalEngineTime = e.EngineTime.TotalEngineTime
            };

            Update(engineTime);
        }


        private void _engineCore_OnRender(object sender, OnRenderEventArgs e)
        {
            //TODO: Look into this.  This should not be created every single time
            //the render method is called. This is not performant.
            _renderer = new Renderer(e.Renderer);

            _renderer.Clear(50, 50, 50, 255);

            _renderer.Start();

            Render(_renderer);

            _renderer.End();
        }
        #endregion
    }
}
using System;
using ScorpionEngine.Content;
using ScorpionEngine.Events;
using ScorpionEngine.GameSound;
using ScorpionEngine.Config;
using ScorpionCore;

namespace ScorpionEngine
{
    /// <summary>
    /// Drives and manages the game.
    /// </summary>
    public class Engine : IDisposable
    {
        #region Fields
        private static IEngineCore _engineCore;
        private static int _prevElapsedTime;
        private IRenderer _renderer;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of engine.
        /// </summary>
        public Engine()
        {
            PluginLoader.Init();
            ContentLoader = PluginLoader.GetContentLoader();
            _renderer = PluginLoader.GetRenderer();

            _engineCore = PluginLoader.GetEngineCore();
            _engineCore.SetFPS(60);

            //_engineCore.Content = DriverLoader.Container.GetInstance<IContentLoader>();
            _engineCore.OnInitialize += _engineCore_OnInitialize;
            _engineCore.OnLoadContent += _engineCore_OnLoadContent;
            _engineCore.OnUpdate += _engineCore_OnUpdate;
            _engineCore.OnRender += _engineCore_OnRender;
        }
        #endregion


        #region Properties
        public IContentLoader ContentLoader { get; set; }

        public IScene Scene { get; private set; }

        /// <summary>
        /// Gets a value indicating that the game engine is currently running.
        /// </summary>
        public bool Running { get; private set; }


        #region Static Properties
        /// <summary>
        /// Gets the FPS that the engine is currently running at.
        /// </summary>
        public static float CurrentFPS { get; private set; }

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
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the game engine.
        /// </summary>
        public void Start()
        {
            //Check if the engine core has been set.  If not throw an exception
            if (_engineCore == null)
                //TODO: Create engine core not set exception.
                throw new Exception("The engine core has not been set.");

            _engineCore.Start();
        }


        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public virtual void Init()
        {
        }


        /// <summary>
        /// Loads all of the content.
        /// </summary>
        public virtual void LoadContent()
        {
        }


        /// <summary>
        /// Updates the game world.
        /// </summary>
        /// <param name="engineTime">The time passed since last frame and game start.</param>
        public virtual void Update(IEngineTiming engineTime)
        {
            var currentTime = engineTime.ElapsedEngineTime.Milliseconds;

            if (!Running) return;//If the engine has not been started, exit

            _prevElapsedTime = currentTime;

            CurrentFPS = (1000f / _prevElapsedTime);
        }


        /// <summary>
        /// Draws the game world.
        /// </summary>
        public virtual void Render(IRenderer renderer)
        {

        }
        

        /// <summary>
        /// Stops the game engine.
        /// </summary>
        public void Stop()
        {
            Running = false;
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
            if (! disposing) return;

            if (_engineCore != null)
                _engineCore.Dispose();
        }
        #endregion


        #region Private Methods
        private void _engineCore_OnInitialize(object sender, EventArgs e)
        {
            Scene?.Initialize();
            Init();
        }


        private void _engineCore_OnLoadContent(object sender, EventArgs e)
        {
            Scene?.LoadContent(ContentLoader);
            LoadContent();
        }


        private void _engineCore_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            Update(e.EngineTime);
        }


        private void _engineCore_OnRender(object sender, EventArgs e)
        {
            Render(_renderer);
        }
        #endregion
    }
}
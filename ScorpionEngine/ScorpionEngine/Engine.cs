using System;
using ScorpionEngine.Content;
using ScorpionEngine.Events;
using ScorpionEngine.GameSound;
using ScorpionEngine.Core;

namespace ScorpionEngine
{
    /// <summary>
    /// Drives and manages the game.
    /// </summary>
    public class Engine : IDisposable
    {
        #region Fields
        private static IEngineCore _engineCore;
        //private static int _framesPreSecondToMaintain = 60;//The set elapsed time for the engine to run at.  This is 60fps
        public static int _prevElapsedTime;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of engine.
        /// </summary>
        public Engine()
        {
        }
        #endregion


        #region Properties
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
        //TODO: Add method docs
        public void SetEngineCore(IEngineCore engineCore, IContentLoader contentLoader)
        {
            _engineCore = engineCore;

            _engineCore.SetFPS(60);

            _engineCore.Content = contentLoader;
        }


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
        public  virtual void Render()
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
        /// Loads a song with the given name.
        /// </summary>
        /// <param name="songName">The name of the song to load.</param>
        /// <returns></returns>
        public static Song LoadSong(string songName)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Loads a sound effect with the given name.
        /// </summary>
        /// <param name="effectName">The name of the sound effect.</param>
        /// <returns></returns>
        public static SoundEffect LoadSoundEffect(string effectName)
        {
            throw new NotImplementedException();
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
        /// <summary>
        /// Calculates a new vector that is relative to the given source vector from the given destination vector.
        /// </summary>
        /// <param name="src">The source vector to set the destination vector relative to.</param>
        /// <param name="objToAdjust">The destination vector that will be relative to the source vector.</param>
        /// <returns></returns>
        private static Vector CalcRelativeVector(Vector src, Vector objToAdjust)
        {
            objToAdjust = new Vector(src.X + objToAdjust.X, objToAdjust.Y);
            objToAdjust = new Vector(objToAdjust.X, src.Y + objToAdjust.Y);

            return objToAdjust;
        }


        private void SetupDebugView()
        {
//            if (World.PhysicsWorld == null || _debugView != null) return;
//
//            // create and configure the debug view
//            _debugView = new DebugViewXNA(World.PhysicsWorld);
//            _debugView.DefaultShapeColor = MonoColor.White;
//            _debugView.SleepingShapeColor = MonoColor.LightGray;
//            _debugView.LoadContent(_graphicsDeviceManager.GraphicsDevice, _engineCore.Content);
        }
        #endregion
    }
}
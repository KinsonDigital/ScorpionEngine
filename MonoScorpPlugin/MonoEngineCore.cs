using System;
using KDScorpionCore;
using KDScorpionCore.Plugins;

namespace MonoScorpPlugin
{
    /// <summary>
    /// Provides the core of a game engine which facilitates how the engine starts, stops,
    /// manages time and how the game loop runs.
    /// </summary>
    public class MonoEngineCore : IEngineCore
    {
        #region Private Fields
        private readonly MonoGame _monoGame;
        private bool _isRunning;
        #endregion


        #region Public Events
        public event EventHandler<OnUpdateEventArgs> OnUpdate;
        public event EventHandler<OnRenderEventArgs> OnRender;
        public event EventHandler OnInitialize;
        public event EventHandler OnLoadContent;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MonoEngineCore"/>.
        /// </summary>
        public MonoEngineCore()
        {
            _monoGame = new MonoGame
            {
                IsMouseVisible = true
            };

            _monoGame.OnInitialize += MonoGame_OnInitialize;
            _monoGame.OnLoadContent += MonoGame_OnLoadContent;
            _monoGame.OnUpdate += MonoGame_OnUpdate;
            _monoGame.OnRender += MonoGame_OnRender;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public int WindowWidth
        {
            get => 1;
            set
            {
                //TODO: Look into if this should change the window width
            }
        }

        /// <summary>
        /// Gets or sets the height of the game window.
        /// </summary>
        public int WindowHeight
        {
            get => 2;
            set
            {
                //TODO: Look into if this should change the window width
            }
        }

        /// <summary>
        /// Gets or sets the renderer that renders graphics to the window.
        /// </summary>
        public IRenderer Renderer
        {
            get => _monoGame.Renderer;
            set { }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the engine.
        /// </summary>
        public void Start()
        {
            _isRunning = true;
            _monoGame.Start();
        }


        /// <summary>
        /// Stops the engine.
        /// </summary>
        public void Stop()
        {
            _monoGame.Dispose();
            _monoGame.Exit();
            _isRunning = false;
        }


        /// <summary>
        /// Sets how many frames the engine will process per second.
        /// </summary>
        /// <param name="value">The total number of frames.</param>
        public void SetFPS(float value)
        {
            _monoGame.SetFPS(value);
        }


        /// <summary>
        /// Returns true if the engine is running.
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return _isRunning;
        }


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Disposes of the <see cref="MonoEngineCore"/>.
        /// </summary>
        public void Dispose() => _monoGame.Dispose();
        #endregion


        #region Private Methods
        /// <summary>
        /// Invokes the <see cref="OnInitialize"/> event.
        /// </summary>
        private void MonoGame_OnInitialize(object sender, EventArgs e)
        {
            //Inject the graphics device into the renderer
            _monoGame.Renderer.InjectData(_monoGame.GraphicsDevice);

            OnInitialize?.Invoke(sender, e);
        }


        /// <summary>
        /// Invokes the <see cref="OnLoadContent"/> event.
        /// </summary>
        private void MonoGame_OnLoadContent(object sender, EventArgs e) => OnLoadContent?.Invoke(sender, e);


        /// <summary>
        /// Invokes the <see cref="OnUpdate"/> event.
        /// </summary>
        private void MonoGame_OnUpdate(object sender, OnUpdateEventArgs e) => OnUpdate?.Invoke(sender, e);


        /// <summary>
        /// Invokes the <see cref="OnRender"/> event.
        /// </summary>
        private void MonoGame_OnRender(object sender, OnRenderEventArgs e) => OnRender?.Invoke(sender, e);
        #endregion
    }
}

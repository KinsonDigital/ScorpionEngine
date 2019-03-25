using Microsoft.Xna.Framework;
using ParticleMaker.CustomEventArgs;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker
{
    /// <summary>
    /// Used to drive and interact with the graphics engine for rendering and updating purposes.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CoreEngine : Game, ICoreEngine
    {
        #region Fields
        private GraphicsDeviceManager _graphics;
        private int _renderWidth = 400;
        private int _renderHeight = 400;
        #endregion


        #region Events
        /// <summary>
        /// Invoked when the engine is initializing.
        /// </summary>
        public event EventHandler<EventArgs> OnInitialize;

        /// <summary>
        /// Invoked when the engine is loading content.
        /// </summary>
        public event EventHandler<EventArgs> OnLoadContent;

        /// <summary>
        /// Invoked when the engine is updating.
        /// </summary>
        public event EventHandler<UpdateEventArgs> OnUpdate;

        /// <summary>
        /// Invoked when the engine is drawing content.
        /// </summary>
        public event EventHandler<DrawEventArgs> OnDraw;

        /// <summary>
        /// Invoked when the engine is unloading content.
        /// </summary>
        public event EventHandler<EventArgs> OnUnLoadContent;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="CoreEngine"/>.
        /// </summary>
        public CoreEngine()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreparingDeviceSettings += _graphics_PreparingDeviceSettings;

            Window.Position = new Point(1000, 0);

            OriginalWindow = Window;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets a value indicating if the engine is running.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// The original render window.
        /// </summary>
        public GameWindow OriginalWindow { get; set; }

        /// <summary>
        /// Gets or sets the handle to the surface to render the graphics to.
        /// </summary>
        public IntPtr RenderSurfaceHandle { get; set; }

        /// <summary>
        /// Gets or sets the width of the render surface.
        /// </summary>
        public int RenderWidth
        {
            get => _renderWidth;
            set
            {
                _renderWidth = value;
                _graphics.PreferredBackBufferWidth = value;
                _graphics.ApplyChanges();
            }
        }

        /// <summary>
        /// Gets or sets the height of the render surface.
        /// </summary>
        public int RenderHeight
        {
            get => _renderHeight;
            set
            {
                _renderHeight = value;
                _graphics.PreferredBackBufferHeight = value;
                _graphics.ApplyChanges();
            }
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the engine.
        /// </summary>
        public void Start()
        {
            Run();
        }


        /// <summary>
        /// Stops the engine.
        /// </summary>
        public void Stop()
        {
            Exit();
        }


        /// <summary>
        /// Unpauses the <see cref="CoreEngine"/>.
        /// </summary>
        public void Play()
        {
            IsRunning = true;
            LoadContent();
        }


        /// <summary>
        /// Pauses the <see cref="CoreEngine"/>.
        /// </summary>
        /// /// <param name="clearSurface">If true, will clear the rendering surface before pausing.</param>
        public void Pause(bool clearSurface)
        {
            if (clearSurface)
                GraphicsDevice?.Clear(new Color(40, 40, 40, 255));

            IsRunning = false;
            UnloadContent();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the graphics device.
        /// </summary>
        private void _graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = RenderSurfaceHandle;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = _renderWidth;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = _renderHeight;
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Initializes the <see cref="CoreEngine"/>.
        /// </summary>
        protected override void Initialize()
        {
            OnInitialize?.Invoke(this, new EventArgs());

            base.Initialize();
        }


        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            OnLoadContent?.Invoke(this, new EventArgs());

            base.LoadContent();
        }


        /// <summary>
        /// Updates the <see cref="CoreEngine"/>.
        /// </summary>
        /// <param name="gameTime">The amount of time that has passed since the last frame.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!IsRunning)
                return;

            OnUpdate?.Invoke(this, new UpdateEventArgs(gameTime));

            base.Update(gameTime);
        }


        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="gameTime">The amount of time that has passed since the last frame.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!IsRunning)
                return;

            OnDraw?.Invoke(this, new DrawEventArgs(gameTime));

            base.Draw(gameTime);
        }


        /// <summary>
        /// Unloads the content.
        /// </summary>
        protected override void UnloadContent()
        {
            OnUnLoadContent?.Invoke(this, new EventArgs());

            base.UnloadContent();
        }


        /// <summary>
        /// Properly disposes of the <see cref="CoreEngine"/>.
        /// </summary>
        /// <param name="disposing">true if currently being disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}

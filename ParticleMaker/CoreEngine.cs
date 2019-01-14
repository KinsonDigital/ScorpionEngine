using Microsoft.Xna.Framework;
using ParticleMaker.CustomEventArgs;
using System;

namespace ParticleMaker
{
    /// <summary>
    /// Used to drive and interact with the graphics engine for rendering and updating purposes.
    /// </summary>
    public class CoreEngine : Game, ICoreEngine
    {
        #region Fields
        private GraphicsDeviceManager _graphics;
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
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the handle to the surface to render the graphics to.
        /// </summary>
        public IntPtr RenderSurfaceHandle { get; set; }
        #endregion


        #region Event Methods
        /// <summary>
        /// Sets up the graphics device.
        /// </summary>
        private void _graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = RenderSurfaceHandle;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = 400;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = 400;
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
            OnUpdate?.Invoke(this, new UpdateEventArgs(gameTime));

            base.Update(gameTime);
        }


        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="gameTime">The amount of time that has passed since the last frame.</param>
        protected override void Draw(GameTime gameTime)
        {
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
        #endregion
    }
}

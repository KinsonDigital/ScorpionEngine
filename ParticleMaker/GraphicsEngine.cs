using KDParticleEngine;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;

namespace ParticleMaker
{
    /// <summary>
    /// Drives the graphics of the particles being rendered to the screen.
    /// </summary>
    public class GraphicsEngine : Game
    {
        #region Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IntPtr _windowsHandle;
        private Control _window;
        private ParticleEngine _particleEngine;
        private ContentLoader _contentLoader;
        private ParticleRenderer _particleRenderer;
        private Renderer _renderer;
        private ParticleTextureLoader _particleTextureLoader;
        private bool _shuttingDown = false;
        private int _width;
        private int _height;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="GraphicsEngine"/>.
        /// </summary>
        /// <param name="windowHandle">The handle that points to the window of where to render the graphics.</param>
        /// <param name="particleEngine">The particle engine that manages the particles.</param>
        public GraphicsEngine(IntPtr windowHandle, ParticleEngine particleEngine, int width, int height)
        {
            _width = width;
            _height = height;

            _particleEngine = particleEngine;

            _windowsHandle = windowHandle;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreparingDeviceSettings += _graphics_PreparingDeviceSettings;
        }
        #endregion


        #region Public Methods
        //TODO: Change this to directly use the BackBufferWidth in the
        //_graphics_PreparingDeviceSettings event below.
        /// <summary>
        /// Gets or sets the width of the render surface that the graphics are rendering to.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        //TODO: Change this to directly use the BackBufferHeight in the
        //_graphics_PreparingDeviceSettings event below.
        /// <summary>
        /// Gets or sets the height of the render surface that the graphics are rendering to.
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Stops the graphics engine.
        /// </summary>
        public void Stop()
        {
            _shuttingDown = true;

            Exit();
        }
        #endregion


        #region Event Methods
        /// <summary>
        /// Sets up the graphics device.
        /// </summary>
        private void _graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            //TODO: Use the width and height below directly in the Width and Height props
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = _windowsHandle;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = _width;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = _height;
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Initializes the <see cref="GraphicsEngine"/>.
        /// </summary>
        protected override void Initialize()
        {
            Window.Position = new Point(-30000, Window.Position.Y);
            _window = Control.FromHandle(Window.Handle);

            _particleTextureLoader = new ParticleTextureLoader(_graphics.GraphicsDevice);
            _contentLoader = new ContentLoader(_particleTextureLoader);

            base.Initialize();
        }


        /// <summary>
        /// Loads the content for the <see cref="GraphicsEngine"/> to render.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: Use SimpleInjector to inject the ParticleRender when creating an instance of Renderer.
            //TODO: This renderer class can be injected into GraphicsEngine class via constructor.
            _particleRenderer = new ParticleRenderer(_spriteBatch);
            _renderer = new Renderer(_particleRenderer);

            var texture = _contentLoader.LoadTexture("Arrow");
            
            _particleEngine.AddTexture(texture);

            base.LoadContent();
        }


        /// <summary>
        /// Updates the <see cref="GraphicsEngine"/>.
        /// </summary>
        /// <param name="gameTime">The amount of time that has passed since the last frame.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_shuttingDown)
                return;

            if (_window.Visible)
                _window.Hide();

            var engineTime = new EngineTime() { ElapsedEngineTime = gameTime.ElapsedGameTime };

            _particleEngine.Update(engineTime);

            base.Update(gameTime);
        }


        /// <summary>
        /// Renders the partical graphics to the screen.
        /// </summary>
        /// <param name="gameTime">The amount of time that has passed since the last frame.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (_shuttingDown)
                return;

            GraphicsDevice.Clear(new Color(40, 40, 40, 255));

            _spriteBatch.Begin();

            _particleEngine.Render(_renderer);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}

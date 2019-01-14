using KDParticleEngine;
using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleMaker.CustomEventArgs;
using ParticleMaker.Services;
using System;
using System.Windows.Forms;

namespace ParticleMaker
{
    /// <summary>
    /// Drives the graphics of the particles being rendered to the screen.
    /// </summary>
    public class GraphicsEngine
    {
        #region Fields
        private ICoreEngine _coreEngine;
        private SpriteBatch _spriteBatch;
        private ParticleEngine _particleEngine;
        private ContentLoader _contentLoader;
        private ParticleRenderer _particleRenderer;
        private Renderer _renderer;
        private ParticleTextureLoader _particleTextureLoader;
        private IContentDirectoryService _contentDirService;
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
        public GraphicsEngine(ParticleEngine particleEngine, int width, int height)
        {
            _coreEngine = new CoreEngine();
            _coreEngine.OnInitialize += _coreEngine_OnInitialize;
            _coreEngine.OnLoadContent += _coreEngine_OnLoadContent;
            _coreEngine.OnUpdate += _coreEngine_OnUpdate;
            _coreEngine.OnDraw += _coreEngine_OnDraw;
            _coreEngine.OnUnLoadContent += _coreEngine_OnUnLoadContent;

            _width = width;
            _height = height;

            _particleEngine = particleEngine;
        }
        #endregion


        #region Props
        public IntPtr RenderSurfaceHandle
        {
            get => _coreEngine.RenderSurfaceHandle;
            set => _coreEngine.RenderSurfaceHandle = value;
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
        /// Starts the <see cref="GraphicsEngine"/>.
        /// </summary>
        public void Run()
        {
            if (RenderSurfaceHandle == IntPtr.Zero)
                throw new Exception($"You must set the rendering surface handle before starting the {nameof(GraphicsEngine)}");

            _coreEngine.Run();
        }


        /// <summary>
        /// Stops the graphics engine.
        /// </summary>
        public void Stop()
        {
            _shuttingDown = true;

            _coreEngine.Exit();
        }
        #endregion


        #region Event Methods
        /// <summary>
        /// Initializes the <see cref="GraphicsEngine"/>.
        /// </summary>
        private void _coreEngine_OnInitialize(object sender, EventArgs e)
        {
            _coreEngine.Window.Position = new Point(-30000, _coreEngine.Window.Position.Y);
            
            _contentDirService = new ContentDirectoryService();

            _particleTextureLoader = App.DIContainer.GetInstance<ParticleTextureLoader>();
            _particleTextureLoader.InjectData(_coreEngine.GraphicsDevice);

            _contentLoader = new ContentLoader(_particleTextureLoader);
        }


        /// <summary>
        /// Loads the content for the <see cref="GraphicsEngine"/> to render.
        /// </summary>
        private void _coreEngine_OnLoadContent(object sender, EventArgs e)
        {
            _spriteBatch = new SpriteBatch(_coreEngine.GraphicsDevice);

            //TODO: Use SimpleInjector to inject the ParticleRender when creating an instance of Renderer.
            //TODO: This renderer class can be injected into GraphicsEngine class via constructor.
            _particleRenderer = new ParticleRenderer(_spriteBatch);
            _renderer = new Renderer(_particleRenderer);

            var texture = _contentLoader.LoadTexture("Arrow");

            _particleEngine.AddTexture(texture);
        }


        /// <summary>
        /// Updates the <see cref="GraphicsEngine"/>.
        /// </summary>
        private void _coreEngine_OnUpdate(object sender, UpdateEventArgs e)
        {
            if (_shuttingDown)
                return;

            _particleEngine.Update(e.GameTime.ToEngineTime());
        }


        /// <summary>
        /// Renders the partical graphics to the screen.
        /// </summary>
        private void _coreEngine_OnDraw(object sender, DrawEventArgs e)
        {
            if (_shuttingDown)
                return;

            _coreEngine.GraphicsDevice.Clear(new Color(40, 40, 40, 255));

            _spriteBatch.Begin();

            _particleEngine.Render(_renderer);

            _spriteBatch.End();
        }


        /// <summary>
        /// Unloads the content.
        /// </summary>
        private void _coreEngine_OnUnLoadContent(object sender, EventArgs e)
        {
            //TODO: Add code here to unload content before shutting down app
        }
        #endregion
    }
}

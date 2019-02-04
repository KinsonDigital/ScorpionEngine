using KDParticleEngine;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleMaker.CustomEventArgs;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker
{
    /// <summary>
    /// Drives the graphics of the particles being rendered to the screen.
    /// </summary>
    public class GraphicsEngine
    {
        #region Fields
        private IGraphicsEngineFactory _factory;
        private ICoreEngine _coreEngine;
        private SpriteBatch _spriteBatch;
        private ContentLoader _contentLoader;
        private Renderer _renderer;
        private bool _shuttingDown = false;
        private int _width;
        private int _height;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="GraphicsEngine"/>.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="particleEngine">The particle engine that manages the particles.</param>
        public GraphicsEngine(IGraphicsEngineFactory factory, ParticleEngine particleEngine)
        {
            _factory = factory;

            _coreEngine = _factory.CoreEngine;

            _coreEngine.OnInitialize += _coreEngine_OnInitialize;
            _coreEngine.OnLoadContent += _coreEngine_OnLoadContent;
            _coreEngine.OnUpdate += _coreEngine_OnUpdate;
            _coreEngine.OnDraw += _coreEngine_OnDraw;
            _coreEngine.OnUnLoadContent += _coreEngine_OnUnLoadContent;

            _width = 400;
            _height = 400;

            ParticleEngine = particleEngine;
        }
        #endregion


        #region Props
        public IntPtr RenderSurfaceHandle
        {
            get => _coreEngine.RenderSurfaceHandle;
            set => _coreEngine.RenderSurfaceHandle = value;
        }

        public ParticleEngine ParticleEngine { get; set; }

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
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the <see cref="GraphicsEngine"/>.
        /// </summary>
        public void Run()
        {
            if (RenderSurfaceHandle == IntPtr.Zero)
                throw new Exception($"You must set the rendering surface handle before starting the {nameof(GraphicsEngine)}");

            _coreEngine.OriginalWindow.Hide();
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
        [ExcludeFromCodeCoverage]
        private void _coreEngine_OnInitialize(object sender, EventArgs e)
        {
            _coreEngine.WindowPosition = new Point(-30000, _coreEngine.WindowPosition.Y);

            _contentLoader = _factory.NewContentLoader();
        }


        /// <summary>
        /// Loads the content for the <see cref="GraphicsEngine"/> to render.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void _coreEngine_OnLoadContent(object sender, EventArgs e)
        {

            //_particleRenderer = new ParticleRenderer(_spriteBatch);
            //_renderer = new Renderer(_particleRenderer);

            _renderer = _factory.NewRenderer();

            _spriteBatch = _factory.SpriteBatch;

            var texture = _contentLoader.LoadTexture("Arrow");

            ParticleEngine.AddTexture(texture);
        }


        /// <summary>
        /// Updates the <see cref="GraphicsEngine"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void _coreEngine_OnUpdate(object sender, UpdateEventArgs e)
        {
            if (_shuttingDown)
                return;

            ParticleEngine.Update(e.GameTime.ToEngineTime());
        }


        /// <summary>
        /// Renders the partical graphics to the screen.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void _coreEngine_OnDraw(object sender, DrawEventArgs e)
        {
            if (_shuttingDown)
                return;

            _coreEngine.GraphicsDevice.Clear(new Color(40, 40, 40, 255));

            _spriteBatch.Begin();

            ParticleEngine.Render(_renderer);

            _spriteBatch.End();
        }


        /// <summary>
        /// Unloads the content.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void _coreEngine_OnUnLoadContent(object sender, EventArgs e)
        {
            //TODO: Add code here to unload content before shutting down app
        }
        #endregion
    }
}

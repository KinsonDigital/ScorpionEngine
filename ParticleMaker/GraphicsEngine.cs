using KDParticleEngine;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleMaker.CustomEventArgs;
using ParticleMaker.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using CoreTexture = KDScorpionCore.Graphics.Texture;

namespace ParticleMaker
{
    /// <summary>
    /// Drives the graphics of the particles being rendered to the screen.
    /// </summary>
    public class GraphicsEngine
    {
        #region Fields
        private IFileService _fileService;
        private IGraphicsEngineFactory _factory;
        private ICoreEngine _coreEngine;
        private SpriteBatch _spriteBatch;
        private Renderer _renderer;
        private bool _shuttingDown = false;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="GraphicsEngine"/>.
        /// </summary>
        /// <param name="factory">The factory used to get <see cref="GraphicsEngine"/> dependencies and services.</param>
        /// <param name="particleEngine">The particle engine that manages the particles behaviors.</param>
        /// <param name="fileService">The file service used to manage particle texture files.</param>
        public GraphicsEngine(IGraphicsEngineFactory factory, ParticleEngine particleEngine, IFileService fileService)
        {
            _fileService = fileService;

            _factory = factory;
            
            _coreEngine = _factory.CoreEngine;

            _coreEngine.OnLoadContent += _coreEngine_OnLoadContent;
            _coreEngine.OnUpdate += _coreEngine_OnUpdate;
            _coreEngine.OnDraw += _coreEngine_OnDraw;

            Width = 400;
            Height = 400;

            ParticleEngine = particleEngine;
        }
        #endregion


        #region Props
        /// <summary>
        /// The handle to the render sturface of where to render the particle graphics.
        /// </summary>
        public IntPtr RenderSurfaceHandle
        {
            get => _coreEngine.RenderSurfaceHandle;
            set => _coreEngine.RenderSurfaceHandle = value;
        }

        /// <summary>
        /// Gets or sets the particle engine managing the particles.
        /// </summary>
        public ParticleEngine ParticleEngine { get; set; }

        /// <summary>
        /// Gets or sets the width of the render surface that the graphics are rendering to.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the render surface that the graphics are rendering to.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets a value indicating if the engine is running or paused.
        /// </summary>
        public bool IsRunning => _coreEngine.IsRunning;

        /// <summary>
        /// This list of paths to all of the texture to load and render.
        /// </summary>
        public string[] TexturePaths { get; set; } = new string[] 
        {
            $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Content\Arrow.png",
            $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Content\Star.png"
        };
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the <see cref="GraphicsEngine"/>.
        /// </summary>
        public void Start()
        {
            //NOTE: This method will not exit until the monogame object has exited
            if (RenderSurfaceHandle == IntPtr.Zero)
                throw new Exception($"You must set the rendering surface handle before starting the {nameof(GraphicsEngine)}");

            _coreEngine.OriginalWindow.Hide();
            _coreEngine.Start();
        }


        /// <summary>
        /// Stops the graphics engine.
        /// </summary>
        public void Stop()
        {
            _shuttingDown = true;

            _coreEngine.Dispose();
            _coreEngine.Stop();
        }


        /// <summary>
        /// Unpauses the graphics engine.
        /// </summary>
        public void Play()
        {
            _coreEngine.Play();
        }


        /// <summary>
        /// Pauses the engine.
        /// </summary>
        public void Pause(bool clearSurface = false)
        {
            _coreEngine?.Pause(clearSurface);
        }
        #endregion


        #region Event Methods
        /// <summary>
        /// Loads the content for the <see cref="GraphicsEngine"/> to render.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void _coreEngine_OnLoadContent(object sender, EventArgs e)
        {
            _renderer = _factory.NewRenderer();

            _spriteBatch = _factory.SpriteBatch;

            foreach (var path in TexturePaths)
            {
                var texture = _fileService.Load(path, _coreEngine.GraphicsDevice);

                ParticleEngine.AddTexture(new CoreTexture(new ParticleTexture(texture.GetTexture<Texture2D>())));
            }
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
        #endregion
    }
}

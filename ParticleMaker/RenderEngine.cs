using KDParticleEngine;
using ParticleMaker.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParticleMaker
{
    public class RenderEngine : IDisposable
    {
        #region Fields
        private Task _loopTask;
        private CancellationTokenSource _tokenSrc;
        private float _targetFrameRate = 1000f / 60f;
        private readonly IRenderer _renderer;
        private ITimingService _timingService;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="RenderEngine"/>.
        /// </summary>
        /// <param name="renderer">The renderer used to render textures to the screen.</param>
        /// <param name="particleEngine">The particle engine that manages the particles.</param>
        public RenderEngine(IRenderer renderer, ParticleEngine<ParticleTexture> particleEngine, ITimingService timingService)
        {
            _renderer = renderer;
            ParticleEngine = particleEngine;
            _timingService = timingService;
        }
        #endregion


        #region Props
        /// <summary>
        /// The particle engine that manages the particles.
        /// </summary>
        public ParticleEngine<ParticleTexture> ParticleEngine { get; set; }

        /// <summary>
        /// The current frames per second that the engine is running at.
        /// </summary>
        public float FPS => _timingService.FPS;

        /// <summary>
        /// The desired frames per second the engine should run at.
        /// </summary>
        public float TargetFrameRate
        {
            get => 1000f / _targetFrameRate;
            set => _targetFrameRate = 1000f / value;
        }

        /// <summary>
        /// This list of paths to all of the texture to load and render.
        /// </summary>
        public string[] TexturePaths { get; set; } = new string[0];

        /// <summary>
        /// Returns a value of true indicating if the engine is currently paused.
        /// </summary>
        public bool IsPaused => _timingService.IsPaused;
        #endregion


        #region Public Methods
        public void SetRenderWindow(IntPtr windowHandler) => _renderer.Init(windowHandler);


        /// <summary>
        /// Stars the engine.
        /// </summary>
        /// <param name="windowHandle"></param>
        public Task Start()
        {
            if (TaskIsRunning())
            {
                //If the engine is currently paused, just start it back up again
                if (_timingService.IsPaused)
                    _timingService.Start();
            }
            else
            {
                LoadTextures();

                _tokenSrc = new CancellationTokenSource();

                _loopTask = new Task(() =>
                {
                    Run();
                }, _tokenSrc.Token);

                _loopTask.Start();
            }


            return _loopTask;
        }


        private bool TaskIsRunning()
        {
            if (_loopTask != null && _loopTask.Status == TaskStatus.Running)
                return true;

            if (_tokenSrc != null && !_tokenSrc.IsCancellationRequested)
                return true;


            return false;
        }


        /// <summary>
        /// Stops the engine.
        /// </summary>
        public void Stop()
        {
            ParticleEngine.Clear();
            _tokenSrc?.Cancel();
        }


        /// <summary>
        /// Pauses the engine.
        /// </summary>
        /// <param name="clearSurface">If true, the surface will be cleared on pause.</param>
        public void Pause(bool clearSurface = false)
        {
            //TODO: Add code to clear the surface before pausing
            _timingService.Pause();
        }
        

        /// <summary>
        /// Frees renderer resources and shuts down the engine.
        /// </summary>
        public void Dispose()
        {
            _renderer.ShutDown();
            _renderer.Dispose();

            _timingService = null;

            _tokenSrc?.Cancel();
            _tokenSrc?.Dispose();
            _tokenSrc = null;

            _loopTask?.Dispose();
            _loopTask = null;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Loads all of the textures.
        /// </summary>
        private void LoadTextures()
        {
            //If any of the textures do not already exist in the engine, add the texture
            foreach (var path in TexturePaths)
            {
                ParticleEngine.Add(_renderer.LoadTexture(path), (texture) => ParticleEngine.Any(p =>
                {
                    return p.Name != texture.Name;
                }));
            }
        }


        /// <summary>
        /// Updates the particle engine.
        /// </summary>
        /// <param name="elapsedTime">The amount of time that passed for this frame.</param>
        private void Update(TimeSpan elapsedTime) => ParticleEngine.Update(elapsedTime);


        /// <summary>
        /// Renders the particles to the screen.
        /// </summary>
        private void Render()
        {
            if (ParticleEngine.Particles.Length <= 0)
                return;

            _renderer.Begin();

            //Render all of the particles
            foreach (var particle in ParticleEngine.Particles)
            {
                _renderer.Render(particle);
            }

            _renderer.End();
        }
        

        //[ExcludeFromCodeCoverage]
        private void Run()
        {
            _timingService.Start();

            while (!_tokenSrc.IsCancellationRequested)
            {
                if (IsPaused)
                {
                    _timingService.Wait();
                    continue;
                }

                if (_timingService.TotalMilliseconds >= _targetFrameRate)
                {
                    Update(_timingService.Elapsed);
                    Render();

                    _timingService.Record();
                }
            }
        }
        #endregion
    }
}

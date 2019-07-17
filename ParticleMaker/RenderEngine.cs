﻿using KDParticleEngine;
using ParticleMaker.Services;
using System;
using System.Linq;

namespace ParticleMaker
{
    /// <summary>
    /// Renders particle graphics to a render surface.
    /// </summary>
    public class RenderEngine : IDisposable
    {
        #region Private Fields
        private readonly IRenderer _renderer;
        private ITimingService _timingService;
        private readonly ITaskManagerService _taskService;
        private float _targetFrameRate = 1000f / 60f;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="RenderEngine"/>.
        /// </summary>
        /// <param name="renderer">The renderer used to render textures to the screen.</param>
        /// <param name="particleEngine">The particle engine that manages the particles.</param>
        /// <param name="timingService">The service used to manage the timing of the engine.</param>
        /// <param name="taskService">The service used to manage asynchronous tasks.</param>
        public RenderEngine(IRenderer renderer, ParticleEngine<ParticleTexture> particleEngine, ITimingService timingService, ITaskManagerService taskService)
        {
            _renderer = renderer;
            ParticleEngine = particleEngine;
            _timingService = timingService;
            _taskService = taskService;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the pointer/window handle to the rendering surface.
        /// </summary>
        public IntPtr WindowHandle => _renderer.WindowHandle;

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
        /// <summary>
        /// Sets the pointer/window handle to the window that the <see cref="RenderEngine"/> will
        /// render the particle graphics to.
        /// </summary>
        /// <param name="surfaceHandle">The pointer/handle to the rendering surface.</param>
        public void SetRenderWindowHandle(IntPtr surfaceHandle) => _renderer.Init(surfaceHandle);


        /// <summary>
        /// Starts the engine.
        /// </summary>
        public void Start()
        {
            //if (TaskIsRunning())
            if (_taskService.IsRunning)
            {
                //If the engine is currently paused, just start it back up again
                if (_timingService.IsPaused)
                    _timingService.Start();
            }
            else
            {
                LoadTextures();
                _taskService.Start(Run);
            }
        }


        /// <summary>
        /// Stops the engine.
        /// </summary>
        public void Stop()
        {
            ParticleEngine.Clear();
            _taskService.Cancel();
        }


        /// <summary>
        /// Pauses the engine.
        /// </summary>
        public void Pause() => _timingService.Pause();
        

        /// <summary>
        /// Frees renderer resources and shuts down the engine.
        /// </summary>
        public void Dispose()
        {
            _renderer.ShutDown();
            _renderer.Dispose();

            _timingService = null;
            _taskService.Dispose();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Loads all of the textures.
        /// </summary>
        private void LoadTextures() =>
            //If any of the textures do not already exist in the engine, add the texture
            TexturePaths.ToList().ForEach(p =>
            {
                ParticleEngine.Add(_renderer.LoadTexture(p), (texture) =>
                    ParticleEngine.Count <= 0 || ParticleEngine.Any(p => p.Name != texture.Name));
            });


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
            ParticleEngine.Particles.ToList().ForEach(p => _renderer.Render(p));

            _renderer.End();
        }
        
        /// <summary>
        /// The internal run method that is constantly rendering the graphics
        /// to the rendering surface.
        /// </summary>
        internal void Run()
        {
            _timingService.Start();

            while (!_taskService.CancelPending)
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

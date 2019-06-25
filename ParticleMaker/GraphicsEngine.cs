using KDParticleEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParticleMaker
{
    public class GraphicsEngine : IDisposable
    {
        #region Fields
        private Stopwatch _timer;
        private Task _loopTask;
        private CancellationTokenSource _tokenSrc;
        private TimeSpan _lastFrameTime;
        private bool _isRunning;
        private bool _isPaused;
        private static float _targetFrameRate = 1000f / 60f;
        private Queue<float> _frameTimes = new Queue<float>();
        private IRenderer _renderer;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="GraphicsEngine"/>.
        /// </summary>
        /// <param name="renderer">The renderer used to render textures to the screen.</param>
        /// <param name="particleEngine">The particle engine that manages the particles.</param>
        public GraphicsEngine(IRenderer renderer, ParticleEngine<ParticleTexture> particleEngine)
        {
            _renderer = renderer;
            ParticleEngine = particleEngine;
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
        public static float CurrentFPS { get; private set; }

        /// <summary>
        /// The desired frames per second the engine should run at.
        /// </summary>
        public static float DesiredFPS
        {
            get => 1000f / _targetFrameRate;
            set => _targetFrameRate = 1000f / value;
        }

        public static TimeStepType TimeStep { get; set; } = TimeStepType.Fixed;

        /// <summary>
        /// This list of paths to all of the texture to load and render.
        /// </summary>
        public string[] TexturePaths { get; set; } = new string[0];
        #endregion


        #region Public Methods
        /// <summary>
        /// Stars the engine.
        /// </summary>
        /// <param name="windowHandle"></param>
        public void Start(IntPtr windowHandle)
        {
            _renderer.Init(windowHandle);

            Initialize();

            _tokenSrc = new CancellationTokenSource();

            _loopTask = new Task(() =>
            {
                Run();
            }, _tokenSrc.Token);

            _loopTask.Start();
        }


        /// <summary>
        /// Stops the engine.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _renderer.ShutDown();
        }


        /// <summary>
        /// Plays the engine.
        /// </summary>
        public void Play()
        {
            _isPaused = false;
            Initialize();
        }


        /// <summary>
        /// Pauses the engine.
        /// </summary>
        /// <param name="clearSurface">If true, the surface will be cleared on pause.</param>
        public void Pause(bool clearSurface = false)
        {
            //TODO: Add code to clear the surface before pausing
            _isPaused = true;
        }


        /// <summary>
        /// Initializes the <see cref="GraphicsEngine"/>.
        /// </summary>
        public virtual void Initialize()
        {
            //If any of the textures do not already exist in the engine, add the texture
            foreach (var path in TexturePaths)
            {
                ParticleEngine.Add(_renderer.LoadTexture(path), (texture) => ParticleEngine.Any(p => p.Name != texture.Name));
            }
        }


        /// <summary>
        /// Frees renderer resources and shuts down the engine.
        /// </summary>
        public void Dispose()
        {
            _timer?.Stop();
            _tokenSrc?.Cancel();
            _tokenSrc?.Dispose();
            _loopTask?.Dispose();
            _tokenSrc = null;
            _loopTask = null;

            _renderer.Dispose();
        }
        #endregion


        #region Private Methods
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

            foreach (var particle in ParticleEngine.Particles)
            {
                _renderer.Render(particle);
            }

            _renderer.End();
        }


        private void Run()
        {
            _isRunning = true;

            _timer = new Stopwatch();
            _timer.Start();

            while (_isRunning)
            {
                if (_isPaused)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                if (TimeStep == TimeStepType.Fixed)
                {
                    if (_timer.Elapsed.TotalMilliseconds >= _targetFrameRate)
                    {
                        Update(_timer.Elapsed);
                        Render();

                        //Add the frame time to the list of previous frame times
                        _frameTimes.Enqueue((float)_timer.Elapsed.TotalMilliseconds);

                        //If the list is full, dequeue the oldest item
                        if (_frameTimes.Count >= 100)
                            _frameTimes.Dequeue();

                        //Calculate the average frames per second
                        CurrentFPS = (float)Math.Round(1000f / _frameTimes.Average(), 2);

                        _timer.Restart();
                    }
                }
                else if (TimeStep == TimeStepType.Variable)
                {
                    var currentFrameTime = _timer.Elapsed;
                    var elapsed = currentFrameTime - _lastFrameTime;

                    _lastFrameTime = currentFrameTime;

                    Update(elapsed);
                    Render();

                    _timer.Stop();

                    //Add the frame time to the list of previous frame times
                    _frameTimes.Enqueue((float)elapsed.TotalMilliseconds);

                    //If the list is full, dequeue the oldest item
                    if (_frameTimes.Count >= 100)
                        _frameTimes.Dequeue();

                    //Calculate the average frames per second
                    CurrentFPS = (float)Math.Round(1000f / _frameTimes.Average(), 2);

                    _timer.Start();
                }
            }

            _renderer.ShutDown();
        }
        #endregion
    }
}

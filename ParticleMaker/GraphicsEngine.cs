using KDParticleEngine;
using ParticleMaker.Services;
using SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParticleMaker
{
    public class GraphicsEngine
    {
        #region Fields
        private static IntPtr _windowPtr;
        private Stopwatch _timer;
        private Task _loopTask;
        private CancellationTokenSource _tokenSrc;
        private TimeSpan _lastFrameTime;
        private bool _isRunning;
        private bool _isPaused;
        private static float _targetFrameRate = 1000f / 60f;
        private Queue<float> _frameTimes = new Queue<float>();
        private IFileService _fileService;
        private IntPtr _renderSurfaceHandle;
        private ICoreEngine _engineCore;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="GraphicsEngine"/>.
        /// </summary>
        public GraphicsEngine(ICoreEngine engineCore, ParticleEngine<ParticleTexture> particleEngine, IFileService fileService)
        {
            _engineCore = engineCore;
            ParticleEngine = particleEngine;
            _fileService = fileService;
        }
        #endregion


        #region Props
        public ParticleEngine<ParticleTexture> ParticleEngine { get; set; }

        public static IntPtr RenderPointer { get; private set; }

        public static float CurrentFPS { get; private set; }

        public static float DesiredFPS
        {
            get => 1000f / _targetFrameRate;
            set => _targetFrameRate = 1000f / value;
        }

        public static TimeStepType TimeStep { get; set; } = TimeStepType.Fixed;

        public int Width
        {
            get
            {
                SDL.SDL_GetWindowSize(_windowPtr, out int w, out _);

                return w;
            }
            set
            {
                SDL.SDL_GetWindowSize(_windowPtr, out _, out int h);
                SDL.SDL_SetWindowSize(_windowPtr, value, h);
            }
        }

        public int Height
        {
            get
            {
                SDL.SDL_GetWindowSize(_windowPtr, out _, out int h);

                return h;
            }
            set
            {
                SDL.SDL_GetWindowSize(_windowPtr, out int w, out _);
                SDL.SDL_SetWindowSize(_windowPtr, w, value);
            }
        }

        /// <summary>
        /// This list of paths to all of the texture to load and render.
        /// </summary>
        public string[] TexturePaths { get; set; } = new string[0];
        #endregion


        #region Public Methods
        public void Start()
        {
            if (_renderSurfaceHandle == IntPtr.Zero)
                throw new Exception($"No render surface handle has been set.  Use the {nameof(SetRenderSurface)}() method to set the surface handle.");

            InitEngine();
            Initialize();

            _tokenSrc = new CancellationTokenSource();

            _loopTask = new Task(() =>
            {
                Run();
            }, _tokenSrc.Token);

            _loopTask.Start();
        }


        public void Stop()
        {
            _timer.Stop();
            _isRunning = false;
            _tokenSrc.Cancel();
            _tokenSrc.Dispose();
            _loopTask.Dispose();
            _tokenSrc = null;
            _loopTask = null;
        }


        public void Play()
        {
            _isPaused = false;
            Initialize();
        }


        public void Pause(bool clearSurface = false)
        {
            //TODO: Add code to clear the surface before pausing
            _isPaused = true;
        }


        public void SetRenderSurface(IntPtr surfaceHandle) => _renderSurfaceHandle = surfaceHandle;


        public virtual void Initialize()
        {
            foreach (var path in TexturePaths)
            {
                var texture = _fileService.Load(path);

                ParticleEngine.AddTexture(texture);
            }
        }
        #endregion


        #region Private Methods
        //TODO: Add method docs
        private void Update(TimeSpan elapsedTime)
        {
            ParticleEngine.Update(elapsedTime);
        }


        //TODO: Add method docs
        private void Render()
        {
            SDL.SDL_SetRenderDrawColor(RenderPointer, 48, 48, 48, 255);
            SDL.SDL_RenderClear(RenderPointer);

            foreach (var particle in ParticleEngine.Particles)
            {
                var textureOrigin = new SDL.SDL_Point()
                {
                    x = particle.Texture.Width / 2,
                    y = particle.Texture.Height / 2
                };

                var srcRect = new SDL.SDL_Rect()
                {
                    x = 0,
                    y = 0,
                    w = particle.Texture.Width,
                    h = particle.Texture.Height
                };

                var destRect = new SDL.SDL_Rect()
                {
                    x = (int)(particle.Position.X - particle.Texture.Width / 2),//Texture X on screen
                    y = (int)(particle.Position.Y - particle.Texture.Height / 2),//Texture Y on screen
                    w = (int)(particle.Texture.Width * particle.Size),//Scaled occurding to size
                    h = (int)(particle.Texture.Height * particle.Size)
                };

                SDL.SDL_SetTextureBlendMode(particle.Texture.TexturePointer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                SDL.SDL_SetTextureColorMod(particle.Texture.TexturePointer, particle.TintColor.R, particle.TintColor.G, particle.TintColor.B);
                SDL.SDL_SetTextureAlphaMod(particle.Texture.TexturePointer, particle.TintColor.A);
                SDL.SDL_RenderCopyEx(RenderPointer, particle.Texture.TexturePointer, ref srcRect, ref destRect, particle.Angle, ref textureOrigin, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }

            SDL.SDL_RenderPresent(RenderPointer);
        }


        private void Run()
        {
            _isRunning = true;

            _timer = new Stopwatch();
            _timer.Start();

            while (_isRunning)
            {
                CheckSDLEvents();

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

            ShutDown();
        }


        private void CheckSDLEvents()
        {
            //Check if the game has a signal to end
            while (SDL.SDL_PollEvent(out var e) != 0)
            {
                //TODO: Add code here for quiting SDL2
            }
        }


        private void InitEngine()
        {
            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                throw new Exception($"SDL could not initialize! SDL_Error: {SDL.SDL_GetError()}");
            }
            else
            {
                //Set texture filtering to linear
                if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1") == SDL.SDL_bool.SDL_FALSE)
                    throw new Exception("Warning: Linear texture filtering not enabled!");

                //Create window
                _windowPtr = SDL.SDL_CreateWindowFrom(_renderSurfaceHandle);

                //TODO: Remove this
                //_windowPtr = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                //    640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

                if (_windowPtr == IntPtr.Zero)
                {
                    throw new Exception($"Window could not be created! SDL_Error: {SDL.SDL_GetError()}");
                }
                else
                {
                    //Create vsynced renderer for window
                    var renderFlags = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
                    RenderPointer = SDL.SDL_CreateRenderer(_windowPtr, -1, renderFlags);

                    if (RenderPointer == IntPtr.Zero)
                    {
                        throw new Exception($"Renderer could not be created! SDL Error: {SDL.SDL_GetError()}");
                    }
                    else
                    {
                        //Initialize renderer color
                        SDL.SDL_SetRenderDrawColor(RenderPointer, 48, 48, 48, 255);

                        //Initialize PNG loading
                        var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;

                        if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                            throw new Exception($"SDL_image could not initialize! SDL_image Error: {SDL.SDL_GetError()}");
                    }
                }
            }
        }


        private void ShutDown()
        {
            SDL.SDL_DestroyRenderer(RenderPointer);
            SDL.SDL_DestroyWindow(_windowPtr);
            SDL.SDL_Quit();
        }
        #endregion
    }
}

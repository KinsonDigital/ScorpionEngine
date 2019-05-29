using KDScorpionCore;
using KDScorpionCore.Plugins;
using SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SDLScorpPlugin
{
    public class SDLEngineCore : IEngineCore
    {
        #region Public Events
        public event EventHandler<OnUpdateEventArgs> OnUpdate;
        public event EventHandler<OnRenderEventArgs> OnRender;
        public event EventHandler OnInitialize;
        public event EventHandler OnLoadContent;
        #endregion


        #region Private Vars
        private static IntPtr _windowPtr = IntPtr.Zero;
        private static IntPtr _rendererPtr = IntPtr.Zero;
        private Stopwatch _timer;
        private TimeSpan _lastFrameTime;
        private bool _isRunning;
        private float _targetFrameRate = 1000f / 60f;
        private Queue<float> _frameTimes = new Queue<float>();
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SDLEngineCore"/>.
        /// </summary>
        public SDLEngineCore() => InitEngine();
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public int WindowWidth
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

        /// <summary>
        /// Gets or sts the height of the game window.
        /// </summary>
        public int WindowHeight
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

        //TODO: Need to implement/create the SDLRenderer and use it here in this prop
        public IRenderer Renderer { get; set; }

        public TimeStepType TimeStep { get; set; } = TimeStepType.Fixed;

        /// <summary>
        /// Gets the current FPS for the current running frame.
        /// </summary>
        public float CurrentFPS { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts up the game engine.
        /// </summary>
        public void Start()
        {
            InitEngine();
            OnInitialize?.Invoke(this, new EventArgs());
            Run();
        }


        /// <summary>
        /// Stops shuts down the game engine.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            _isRunning = false;
        }


        /// <summary>
        /// Gets a value indicating if the engine is currently running.
        /// </summary>
        /// <returns></returns>
        public bool IsRunning() => _isRunning;


        /// <summary>
        /// Sets the FPS of the engine to run at the given <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to set the FPS at.</param>
        public void SetFPS(float value) => _targetFrameRate = 1000f / value;


        /// <summary>
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Injects a pointer into the plugin for use.
        /// </summary>
        public void InjectPointer(IntPtr pointer)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Properly disposes of the engine by disposing of any
        /// SDL resources.
        /// </summary>
        public void Dispose()
        {
            SDL.SDL_DestroyRenderer(_rendererPtr);
            SDL.SDL_DestroyWindow(_windowPtr);
            SDL.SDL_Quit();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Initializes the engine by setting up SDL.
        /// </summary>
        private void InitEngine()
        {
            //Initialization flag
            var success = true;

            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                //TODO: Convert to exception
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                //Set texture filtering to linear
                if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1") == SDL.SDL_bool.SDL_FALSE)
                {
                    //TODO: Convert to exception
                    Console.WriteLine("Warning: Linear texture filtering not enabled!");
                }

                //Create window
                _windowPtr = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                    640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
                if (_windowPtr == IntPtr.Zero)
                {
                    //TODO: Convert to exception
                    Console.WriteLine("Window could not be created! SDL_Error: {0}", SDL.SDL_GetError());
                    success = false;
                }
                else
                {
                    //Create vsynced renderer for window
                    var renderFlags = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
                    _rendererPtr = SDL.SDL_CreateRenderer(_windowPtr, -1, renderFlags);
                    if (_rendererPtr == IntPtr.Zero)
                    {
                        //TODO: Convert to exception
                        Console.WriteLine("Renderer could not be created! SDL Error: {0}", SDL.SDL_GetError());
                        success = false;
                    }
                    else
                    {
                        //Initialize renderer color
                        SDL.SDL_SetRenderDrawColor(_rendererPtr, 48, 48, 48, 255);

                        //Initialize PNG loading
                        var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;
                        if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                        {
                            //TODO: Convert to exception
                            Console.WriteLine("SDL_image could not initialize! SDL_image Error: {0}", SDL.SDL_GetError());
                            success = false;
                        }

                        //Initialize SDL_ttf
                        if (SDL_ttf.TTF_Init() == -1)
                        {
                            //TODO: Convert to exception
                            Console.WriteLine("SDL_ttf could not initialize! SDL_ttf Error: {0}", SDL.SDL_GetError());
                            success = false;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Runs the engine.
        /// </summary>
        private void Run()
        {
            _isRunning = true;
            _timer = new Stopwatch();
            _timer.Start();

            while (_isRunning)
            {
                if (TimeStep == TimeStepType.Fixed)
                {
                    if (_timer.Elapsed.TotalMilliseconds >= _targetFrameRate)
                    {
                        var engineTime = new EngineTime()
                        {
                            ElapsedEngineTime = _timer.Elapsed,
                            TotalEngineTime = _timer.Elapsed
                        };

                        OnUpdate?.Invoke(this, new OnUpdateEventArgs(engineTime));

                        //TODO: NEED TO IMPLEMENT THE SDLRenderer CLASS FOR THE PARAM BELOW
                        OnRender?.Invoke(this, new OnRenderEventArgs(null));

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

                    var engineTime = new EngineTime()
                    {
                        ElapsedEngineTime = _timer.Elapsed,
                        TotalEngineTime = _timer.Elapsed
                    };

                    OnUpdate?.Invoke(this, new OnUpdateEventArgs(engineTime));

                    //TODO: NEED TO IMPLEMENT THE SDLRenderer CLASS FOR THE PARAM BELOW
                    OnRender?.Invoke(this, new OnRenderEventArgs(null));

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
        }


        /// <summary>
        /// Properly shuts down the engine and releases SDL resources.
        /// </summary>
        private void ShutDown() => Dispose();
        #endregion
    }
}

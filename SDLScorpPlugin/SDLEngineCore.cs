﻿using KDScorpionCore;
using KDScorpionCore.Plugins;
using SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        private Stopwatch _timer;
        private TimeSpan _lastFrameTime;
        private bool _isRunning;
        private float _targetFrameRate = 1000f / 120f;
        private readonly Queue<float> _frameTimes = new Queue<float>();
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SDLEngineCore"/>.
        /// </summary>
        public SDLEngineCore() { }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public int WindowWidth
        {
            get
            {
                SDL.SDL_GetWindowSize(WindowPtr, out int w, out _);

                return w;
            }
            set
            {
                SDL.SDL_GetWindowSize(WindowPtr, out _, out int h);
                SDL.SDL_SetWindowSize(WindowPtr, value, h);
            }
        }

        /// <summary>
        /// Gets or sts the height of the game window.
        /// </summary>
        public int WindowHeight
        {
            get
            {
                SDL.SDL_GetWindowSize(WindowPtr, out _, out int h);

                return h;
            }
            set
            {
                SDL.SDL_GetWindowSize(WindowPtr, out int w, out _);
                SDL.SDL_SetWindowSize(WindowPtr, w, value);
            }
        }

        public IRenderer Renderer { get; private set; }

        public TimeStepType TimeStep { get; set; } = TimeStepType.Fixed;

        /// <summary>
        /// Gets the current FPS for the current running frame.
        /// </summary>
        public float CurrentFPS { get; private set; }

        /// <summary>
        /// Gets the pointer to the window.
        /// </summary>
        internal static IntPtr WindowPtr { get; private set; } = IntPtr.Zero;

        /// <summary>
        /// Gets the renderer pointer.
        /// </summary>
        internal static IntPtr RendererPointer { get; private set; } = IntPtr.Zero;

        /// <summary>
        /// Gets or sets the current state of the keyboard for the current frame.
        /// </summary>
        internal static List<SDL.SDL_Keycode> CurrentKeyboardState { get; set; } = new List<SDL.SDL_Keycode>();

        /// <summary>
        /// Gets or sets the previous state of the keyboard for the previous frame.
        /// </summary>
        internal static List<SDL.SDL_Keycode> PreviousKeyboardState { get; set; } = new List<SDL.SDL_Keycode>();

        /// <summary>
        /// Gets the current state of the left mouse button.
        /// </summary>
        internal static bool CurrentLeftMouseButtonState { get; private set; }

        /// <summary>
        /// Gets the current state of the right mouse button.
        /// </summary>
        internal static bool CurrentRightMouseButtonState { get; private set; }

        /// <summary>
        /// Gets the current state of the middle mouse button.
        /// </summary>
        internal static bool CurrentMiddleMouseButtonState { get; private set; }

        /// <summary>
        /// Gets the current position of the mouse cursur in the game window.
        /// </summary>
        internal static Vector MousePosition { get; private set; }
        #endregion


        #region Public Methods
        public void Start()
        {
            InitEngine();
            OnInitialize?.Invoke(this, new EventArgs());
            Run();
        }


        public void Stop()
        {
            _timer.Stop();
            _isRunning = false;
        }


        public bool IsRunning() => _isRunning;


        public void SetFPS(float value) => _targetFrameRate = 1000f / value;


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public T GetData<T>(int option) where T : class
        {
            var ptrContainer = new PointerContainer();

            if (option == 1)
            {
                ptrContainer.PackPointer(RendererPointer);
            }
            else if (option == 2)
            {
                ptrContainer.PackPointer(WindowPtr);
            }
            else
            {
                throw new Exception($"Incorrect {nameof(option)} parameter in '{nameof(SDLEngineCore)}.{nameof(GetData)}()'");
            }


            return ptrContainer as T;
        }


        /// <summary>
        /// Properly disposes of the engine by disposing of any
        /// SDL resources.
        /// </summary>
        public void Dispose()
        {
            _isRunning = false;
            SDL.SDL_DestroyRenderer(RendererPointer);
            SDL.SDL_DestroyWindow(WindowPtr);

            //Quit SDL sub systems
            SDL_ttf.TTF_Quit();
            SDL_image.IMG_Quit();

            SDL.SDL_Quit();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Initializes the engine by setting up SDL.
        /// </summary>
        private void InitEngine()
        {
            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                throw new Exception($"SDL could not initialize! \n\nSDL_Error: {SDL.SDL_GetError()}");
            }
            else
            {
                //Set texture filtering to linear
                if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "0") == SDL.SDL_bool.SDL_FALSE)
                    throw new Exception("Warning: Linear texture filtering not enabled!");

                //Create window
                WindowPtr = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                    640, 480, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

                if (WindowPtr == IntPtr.Zero)
                {
                    throw new Exception($"Window could not be created! \n\nSDL_Error: {SDL.SDL_GetError()}");
                }
                else
                {
                    //Create vsynced renderer for window
                    var renderFlags = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;

                    Renderer = new SDLRenderer();
                    RendererPointer = SDL.SDL_CreateRenderer(WindowPtr, -1, renderFlags);

                    var ptrContainer = new PointerContainer();
                    ptrContainer.PackPointer(RendererPointer);

                    Renderer.InjectData(ptrContainer);

                    if (RendererPointer == IntPtr.Zero)
                    {
                        throw new Exception($"Renderer could not be created! \n\nSDL_Error: {SDL.SDL_GetError()}");
                    }
                    else
                    {
                        //Initialize renderer color
                        SDL.SDL_SetRenderDrawColor(RendererPointer, 48, 48, 48, 255);

                        //Initialize PNG loading
                        var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;

                        if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                            throw new Exception($"SDL_image could not initialize! \n\nSDL_image Error: {SDL.SDL_GetError()}");

                        //Initialize SDL_ttf
                        if (SDL_ttf.TTF_Init() == -1)
                            throw new Exception($"SDL_ttf could not initialize! \n\nSDL_ttf Error: {SDL.SDL_GetError()}");
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
                UpdateInputStates();

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

                        OnRender?.Invoke(this, new OnRenderEventArgs(Renderer));

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

                    OnRender?.Invoke(this, new OnRenderEventArgs(Renderer));

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

                //Update the previous state of the keyboard
                UpdatePreviousKeyboardState();
            }
        }


        private void UpdatePreviousKeyboardState()
        {
            PreviousKeyboardState.Clear();
            PreviousKeyboardState.AddRange(CurrentKeyboardState);
        }


        /// <summary>
        /// Properly shuts down the engine and releases SDL resources.
        /// </summary>
        private void ShutDown() => Dispose();


        /// <summary>
        /// Updates the state of all the keyboard keys.
        /// </summary>
        private void UpdateInputStates()
        {
            while (SDL.SDL_PollEvent(out var e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    ShutDown();
                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    if (!CurrentKeyboardState.Contains(e.key.keysym.sym))
                        CurrentKeyboardState.Add(e.key.keysym.sym);
                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    CurrentKeyboardState.Remove(e.key.keysym.sym);
                }
                else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    CurrentLeftMouseButtonState = e.button.button == 1 && e.button.state == 1;
                    CurrentMiddleMouseButtonState = e.button.button == 2 && e.button.state == 1;
                    CurrentRightMouseButtonState = e.button.button == 3 && e.button.state == 1;
                }
                else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    CurrentLeftMouseButtonState = e.button.button == 1 && e.button.state == 1;
                    CurrentMiddleMouseButtonState = e.button.button == 2 && e.button.state == 1;
                    CurrentRightMouseButtonState = e.button.button == 3 && e.button.state == 1;
                }
                else if (e.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                {
                    MousePosition = new Vector(e.button.x, e.button.y);
                }
            }
        }
        #endregion
    }
}

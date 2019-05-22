using KDScorpionCore;
using KDScorpionCore.Plugins;
using SDL2;
using System;
using System.Collections.Generic;
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
        private const int SCREEN_WIDTH = 640;
        private const int SCREEN_HEIGHT = 480;
        private static IntPtr _windowPtr = IntPtr.Zero;
        private static IntPtr _rendererPtr = IntPtr.Zero;
        #endregion


        #region Props
        public int WindowWidth { get => SCREEN_WIDTH; set => throw new NotImplementedException(); }

        public int WindowHeight { get => SCREEN_HEIGHT; set => throw new NotImplementedException(); }

        public IRenderer Renderer { get; set; }
        #endregion


        #region Constructors
        public SDLEngineCore()
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
                    Console.WriteLine("Warning: Linear texture filtering not enabled!");

                //Create window
                _windowPtr = SDL.SDL_CreateWindow("TODO: Add title feature",
                    SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
                    SCREEN_WIDTH, SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

                if (_windowPtr == IntPtr.Zero)
                {
                    throw new Exception($"Window could not be created! SDL_Error: {SDL.SDL_GetError()}");
                }
                else
                {
                    //Create renderer for window
                    _rendererPtr = SDL.SDL_CreateRenderer(_windowPtr, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

                    if (_rendererPtr == IntPtr.Zero)
                    {
                        throw new Exception($"Renderer could not be created! SDL Error: {SDL.SDL_GetError()}");
                    }
                    else
                    {
                        //Initialize renderer color
                        SDL.SDL_SetRenderDrawColor(_rendererPtr, 0xFF, 0xFF, 0xFF, 0xFF);

                        //Initialize PNG loading
                        var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;

                        if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                            throw new Exception($"SDL_image could not initialize! SDL_image Error: {SDL.SDL_GetError()}");
                    }
                }

                Renderer = new SDLRenderer();
                Renderer.InjectPointer(_rendererPtr);
            }
        }
        #endregion


        #region Public Methods
        public void Start()
        {
            throw new NotImplementedException();
        }


        public void Stop()
        {
            throw new NotImplementedException();
        }


        public bool IsRunning()
        {
            throw new NotImplementedException();
        }


        public void SetFPS(float value)
        {
            throw new NotImplementedException();
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectPointer(IntPtr pointer)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

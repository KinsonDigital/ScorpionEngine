using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using KDParticleEngine;
using SDL2;

namespace ParticleMaker
{
    /// <summary>
    /// Renders graphics to a render target using <see cref="SDL2"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SDLRenderer : IRenderer
    {
        #region Private Fields
        private IntPtr _sdlWindowPtr;//The pointer to the SDL render target window
        private IntPtr _renderPtr;//The pointer to the SDL renderer
        private bool _beginInvokedFirst;//Keeps track if the Begin() method has been invoked
        #endregion


        #region Props
        public IntPtr WindowHandle { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Initializes the renderer.
        /// </summary>
        /// <param name="windowHandle">The window handle of the window to render to.</param>
        public void Init(IntPtr windowHandle)
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

                if (windowHandle == IntPtr.Zero)
                {
                    throw new Exception("The window handle must not be zero.");
                }
                else
                {
                    //Create window
                    _sdlWindowPtr = SDL.SDL_CreateWindowFrom(windowHandle);

                    if (_sdlWindowPtr == IntPtr.Zero)
                    {
                        throw new Exception($"Window could not be created! SDL_Error: {SDL.SDL_GetError()}");
                    }
                    else
                    {
                        WindowHandle = windowHandle;

                        //Create vsynced renderer for window
                        var renderFlags = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;

                        _renderPtr = SDL.SDL_CreateRenderer(_sdlWindowPtr, -1, renderFlags);

                        if (_renderPtr == IntPtr.Zero)
                        {
                            throw new Exception($"Renderer could not be created! SDL Error: {SDL.SDL_GetError()}");
                        }
                        else
                        {
                            //Initialize renderer color
                            SDL.SDL_SetRenderDrawColor(_renderPtr, 48, 48, 48, 255);

                            //Initialize PNG loading
                            var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;

                            if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                                throw new Exception($"SDL_image could not initialize! SDL_image Error: {SDL.SDL_GetError()}");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Loads texture file from disk and returns it as a <see cref="Particle{ITexture}"/>.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns></returns>
        public ParticleTexture LoadTexture(string path)
        {
            if (_renderPtr == IntPtr.Zero)
                throw new Exception($"The renderer has not been initialized.  Use the {nameof(Init)}() method.");

            if (!File.Exists(path))
                throw new FileNotFoundException("The particle file at the given path was not found.", path);

            //Load image at specified path
            var loadedSurface = SDL_image.IMG_Load(path);

            if (loadedSurface == IntPtr.Zero)
            {
                throw new Exception($"Unable to load image {path}! SDL Error: {SDL.SDL_GetError()}");
            }
            else
            {
                //Create texture from surface pixels
                var texturePtr = SDL.SDL_CreateTextureFromSurface(_renderPtr, loadedSurface);

                if (texturePtr == IntPtr.Zero)
                    throw new Exception($"Unable to create texture from {path}! SDL Error: {SDL.SDL_GetError()}");

                SDL.SDL_QueryTexture(texturePtr, out uint _, out int _, out int width, out int height);

                //Get rid of old loaded surface
                SDL.SDL_FreeSurface(loadedSurface);

                return new ParticleTexture(texturePtr, width, height)
                {
                    Name = Path.GetFileNameWithoutExtension(path)
                };
            }
        }


        /// <summary>
        /// Begins the rendering process.
        /// </summary>
        /// <remarks>This method must be invoked first before the <see cref="Render(Particle{ParticleTexture})"/> method.</remarks>
        public void Begin()
        {
            SDL.SDL_SetRenderDrawColor(_renderPtr, 48, 48, 48, 255);
            SDL.SDL_RenderClear(_renderPtr);
            _beginInvokedFirst = true;
        }


        /// <summary>
        /// Rendeers the given <paramref name="particle"/> to the render target.
        /// Each render call will be batched.
        /// </summary>
        /// <param name="particle">The particle to render.</param>
        public void Render(Particle<ParticleTexture> particle)
        {
            if (!_beginInvokedFirst)
                throw new Exception($"The {nameof(Begin)}() method must be invoked first.");

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
            SDL.SDL_RenderCopyEx(_renderPtr, particle.Texture.TexturePointer, ref srcRect, ref destRect, particle.Angle, ref textureOrigin, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }


        /// <summary>
        /// Ends the rendering process.  All of the graphics batched in the <see cref="Render(Particle{ParticleTexture})"/>
        /// calls will be rendered to the render target when this method is invoked.
        /// </summary>
        public void End()
        {
            SDL.SDL_RenderPresent(_renderPtr);
            _beginInvokedFirst = false;
        }


        /// <summary>
        /// Shuts down the renderer.
        /// </summary>
        public void ShutDown() => Dispose();


        /// <summary>
        /// Disposes of the <see cref="SDLRenderer"/>.  Invoking <see cref="ShutDown"/> will
        /// also call this method.
        /// </summary>
        public void Dispose()
        {
            SDL.SDL_DestroyRenderer(_renderPtr);
            SDL.SDL_DestroyWindow(_sdlWindowPtr);
            _renderPtr = IntPtr.Zero;
            _sdlWindowPtr = IntPtr.Zero;

            SDL_image.IMG_Quit();
            SDL.SDL_Quit();
        }
        #endregion
    }
}

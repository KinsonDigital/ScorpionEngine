using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDLScorpPlugin
{
    public class SDLContentLoader : IContentLoader
    {
        #region Props
        public string GamePath { get; } = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

        public string ContentRootDirectory { get; set; }
        #endregion


        #region Constructors
        public SDLContentLoader() => ContentRootDirectory = $@"{GamePath}\Content\";
        #endregion


        #region Public Methods
        T IContentLoader.LoadTexture<T>(string name)
        {
            var texturePath = $@"{ContentRootDirectory}\Graphics\{name}.png";

            //The final optimized image
            var newTexturePtr = IntPtr.Zero;

            //Load image at specified path
            var loadedSurface = SDL_image.IMG_Load(texturePath);

            if (loadedSurface == IntPtr.Zero)
            {
                throw new Exception($"Unable to load image {texturePath}! \n\nSDL Error: {SDL.SDL_GetError()}");
            }
            else
            {
                //Create texture from surface pixels
                newTexturePtr = SDL.SDL_CreateTextureFromSurface(SDLEngineCore.RendererPointer, loadedSurface);

                if (newTexturePtr == IntPtr.Zero)
                    throw new Exception($"Unable to create texture from {texturePath}! \n\nSDL Error: {SDL.SDL_GetError()}");

                //Get rid of old loaded surface
                SDL.SDL_FreeSurface(loadedSurface);
            }

            ITexture newTexture = new SDLTexture(newTexturePtr);


            return newTexture as T;
        }


        public T GetData<T>(int option) where T : class => throw new NotImplementedException();


        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();


        T IContentLoader.LoadText<T>(string name) => throw new NotImplementedException();
        #endregion
    }
}

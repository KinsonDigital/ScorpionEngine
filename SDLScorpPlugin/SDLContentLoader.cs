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


        #region Public Methods
        T IContentLoader.LoadTexture<T>(string name)
        {
            var texturePath = $@"{GamePath}\Content\{name}.png";

            //The final optimized image
            var newTexturePtr = IntPtr.Zero;

            //Load image at specified path
            var loadedSurface = SDL_image.IMG_Load(texturePath);

            if (loadedSurface == IntPtr.Zero)
            {
                throw new Exception($"Unable to load image {texturePath}! SDL Error: {SDL.SDL_GetError()}");
            }
            else
            {
                //TODO: Need to get a real renderer pointer from the Renderer
                var tempRendererPtr = IntPtr.Zero;

                //Create texture from surface pixels
                newTexturePtr = SDL.SDL_CreateTextureFromSurface(tempRendererPtr, loadedSurface);

                if (newTexturePtr == IntPtr.Zero)
                    throw new Exception($"Unable to create texture from {texturePath}! SDL Error: {SDL.SDL_GetError()}");

                //Get rid of old loaded surface
                SDL.SDL_FreeSurface(loadedSurface);
            }

            ITexture newTexture = new SDLTexture(newTexturePtr);

            //TODO: This is probably not needed due to injecting the pointer via the constructor
            //Remove this
            //newTexture.InjectData();


            return newTexture as T;
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        T IContentLoader.LoadText<T>(string name)
        {
            throw new NotImplementedException();
        }


        public void InjectPointer(IntPtr pointer)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

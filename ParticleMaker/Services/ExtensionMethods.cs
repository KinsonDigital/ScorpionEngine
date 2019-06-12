using Microsoft.Xna.Framework.Graphics;
using ParticleMaker.Exceptions;
using SDL2;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides extension methods for the classes in the <see cref="Services"/> namespace.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ExtensionMethods
    {
        //TODO: Remove this
        /// <summary>
        /// Loads a file at the given <paramref name="path"/> using the given <paramref name="grfxDevice"/>.
        /// </summary>
        /// <param name="service">The service to extend.</param>
        /// <param name="path">The directory path to the file.</param>
        /// <param name="grfxDevice">Used to load the particle texture from disk.</param>
        /// <returns></returns>
        public static ParticleTexture Load(this IFileService service, string path, GraphicsDevice grfxDevice)
        {
            if (service.Exists(path))
            {
                using (var file = File.OpenRead(path))
                {
                    return new ParticleTexture(Texture2D.FromStream(grfxDevice, file));
                }
            }

            throw new ParticleDoesNotExistException();
        }


        /// <summary>
        /// Loads a <see cref="ParticleRenderer_NEW"/> at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="service">The service used to load the particle texture.</param>
        /// <param name="path">The path to the texture file.</param>
        /// <returns></returns>
        public static ParticleTexture_NEW Load(this IFileService service, string path)
        {
            if (GraphicsEngine_NEW.Renderer == IntPtr.Zero)
                throw new Exception($"No render surface handle has been set.  Use the {nameof(GraphicsEngine_NEW.SetRenderSurface)}() method to set the surface handle.");

            if (!service.Exists(path))
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
                var texturePtr = SDL.SDL_CreateTextureFromSurface(GraphicsEngine_NEW.Renderer, loadedSurface);

                if (texturePtr == IntPtr.Zero)
                    throw new Exception($"Unable to create texture from {path}! SDL Error: {SDL.SDL_GetError()}");

                SDL.SDL_QueryTexture(texturePtr, out uint _, out int _, out int width, out int height);

                //Get rid of old loaded surface
                SDL.SDL_FreeSurface(loadedSurface);

                return new ParticleTexture_NEW(texturePtr, width, height);
            }
        }
    }
}

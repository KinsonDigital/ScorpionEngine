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
        /// <summary>
        /// Loads a <see cref="ParticleRenderer"/> at the given <paramref name="path"/>.
        /// </summary>
        /// <param name="service">The service used to load the particle texture.</param>
        /// <param name="path">The path to the texture file.</param>
        /// <returns></returns>
        public static ParticleTexture Load(this IFileService service, string path)
        {
            if (GraphicsEngine.Renderer == IntPtr.Zero)
                throw new Exception($"No render surface handle has been set.  Use the {nameof(GraphicsEngine.SetRenderSurface)}() method to set the surface handle.");

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
                var texturePtr = SDL.SDL_CreateTextureFromSurface(GraphicsEngine.Renderer, loadedSurface);

                if (texturePtr == IntPtr.Zero)
                    throw new Exception($"Unable to create texture from {path}! SDL Error: {SDL.SDL_GetError()}");

                SDL.SDL_QueryTexture(texturePtr, out uint _, out int _, out int width, out int height);

                //Get rid of old loaded surface
                SDL.SDL_FreeSurface(loadedSurface);

                return new ParticleTexture(texturePtr, width, height);
            }
        }
    }
}

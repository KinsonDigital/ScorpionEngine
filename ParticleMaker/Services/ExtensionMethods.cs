using Microsoft.Xna.Framework.Graphics;
using ParticleMaker.Exceptions;
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
    }
}

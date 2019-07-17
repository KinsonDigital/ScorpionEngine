using SDL2;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace ParticleMaker
{
    /// <summary>
    /// A particle texture to be rendering to a graphics surface.
    /// </summary>
    public class ParticleTexture : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ParticleTexture"/>.
        /// </summary>
        /// <param name="texturePtr">The pointer to the texture.</param>
        /// <param name="width">The width of the texture.</param>
        /// <param name="height">The height of the texture.</param>
        public ParticleTexture(IntPtr texturePtr, int width, int height)
        {
            TexturePointer = texturePtr;
            Width = width;
            Height = height;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the texture.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The pointer to the texture to render.
        /// </summary>
        public IntPtr TexturePointer { get; set; }

        /// <summary>
        /// Gets or sets the X coordinate of where to render the <see cref="ParticleTexture"/> on the graphics surface.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of where to render the <see cref="ParticleTexture"/> on the graphics surface.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the color of the <see cref="ParticleTexture"/>.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the width of the <see cref="ParticleTexture"/>.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the <see cref="ParticleTexture"/>.
        /// </summary>
        public int Height { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns the hash code of the <see cref="ParticleTexture"/> that makes this unique.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Name.GetHashCode();


        /// <summary>
        /// Properly disposes of the <see cref="ParticleTexture"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose() => SDL.SDL_DestroyTexture(TexturePointer);
        #endregion
    }
}

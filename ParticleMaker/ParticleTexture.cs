using SDL2;
using System;
using System.Drawing;

namespace ParticleMaker
{
    public class ParticleTexture : IDisposable
    {
        #region Constructors
        public ParticleTexture(IntPtr texturePtr, int width, int height)
        {
            TexturePointer = texturePtr;
            Width = width;
            Height = height;
        }
        #endregion


        #region Props
        public string Name { get; set; }

        public IntPtr TexturePointer { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Color Color { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns the hash code of the <see cref="ParticleTexture"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Name.GetHashCode();


        /// <summary>
        /// Properly disposes of the <see cref="ParticleTexture"/>.
        /// </summary>
        public void Dispose() => SDL.SDL_DestroyTexture(TexturePointer);
        #endregion
    }
}

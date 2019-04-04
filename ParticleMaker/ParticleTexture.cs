using KDScorpionCore.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker
{
    /// <summary>
    /// Represents a single texture particle for a particle engine.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ParticleTexture : ITexture
    {
        #region Fields
        private readonly Texture2D _texture;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new <see cref="ParticleTexture"/>.
        /// </summary>
        public ParticleTexture(Texture2D texture)
        {
            _texture = texture;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the width of the <see cref="ParticleTexture"/>.
        /// </summary>
        public int Width => _texture.Width;

        /// <summary>
        /// Gets the height of the <see cref="ParticleTexture"/>.
        /// </summary>
        public int Height => _texture.Height;
        #endregion


        #region Public Methods
        /// <summary>
        /// Gets data.  WARNING!! NOT IMPLEMENTED.
        /// </summary>
        /// <param name="dataType">The type of data to get.</param>
        /// <returns></returns>
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns the particle texture as a <see cref="Texture2D"/>.
        /// </summary>
        /// <typeparam name="T">The type of texture to return.</typeparam>
        /// <returns></returns>
        public T GetTexture<T>() where T : class
        {
#pragma warning disable IDE0019 // Use pattern matching
            var result = _texture as T;
#pragma warning restore IDE0019 // Use pattern matching

            if (result == null)
                throw new Exception($"Generic param T must be of type {nameof(Texture2D)}");


            return result;
        }


        /// <summary>
        /// Injects data.  WARNING!! NOT IMPLEMENTED.
        /// </summary>
        /// <param name="data">The data to inject.</param>
        /// <returns></returns>
        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

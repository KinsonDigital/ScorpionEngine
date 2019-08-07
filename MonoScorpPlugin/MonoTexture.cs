using Microsoft.Xna.Framework.Graphics;
using System;
using KDScorpionCore.Graphics;

namespace MonoScorpPlugin
{
    /// <summary>
    /// Represents a <see cref="Texture2D"/> texture that can be renderered to a graphics surface.
    /// </summary>
    public class MonoTexture : ITexture
    {
        #region Props
        /// <summary>
        /// The internal mono <see cref="Texture2D"/> implementation.
        /// </summary>
        internal Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the width of the texture.
        /// </summary>
        public int Width => Texture.Width;

        /// <summary>
        /// Gets the height of the texture.
        /// </summary>
        public int Height => Texture.Height;
        #endregion


        #region Public Methods
        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        /// <exception cref="Exceptoin">Thrown if the <paramref name="data"/> is not of type <see cref="Texture2D"/>.</exception>
        public void InjectData<T>(T data) where T : class
        {
            //If the incoming data is not a monogame texture, throw an exception
            if (data.GetType() != typeof(Texture2D))
                throw new Exception($"Data getting injected into {nameof(MonoTexture)} is not of type {nameof(Texture2D)}.  Incorrect type is '{data.GetType().ToString()}'");

            Texture = data as Texture2D;
        }


        /// <summary>
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <exception cref="Exception">Thrown if the <paramref name="option"/> value is not the value of
        /// type '1' for the type <see cref="Texture2D"/>.</exception>
        public T GetData<T>(int option) where T : class
        {
            if (option == 1)
                return Texture as T;


            throw new Exception($"The option '{option}' is not valid. \n\nValid options are 1.");
        }
        #endregion
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using KDScorpionCore.Graphics;

namespace MonoScorpPlugin
{
    public class MonoTexture : ITexture
    {
        #region Props
        internal Texture2D Texture { get; private set; }

        public int Width => Texture.Width;

        public int Height => Texture.Height;
        #endregion


        #region Public Methods
        public T GetData<T>(int option) where T : class
        {
            if (option == 1)
                return Texture as T;


            throw new Exception($"The option '{option}' is not valid. \n\nValid options are 1.");
        }


        public void InjectData<T>(T data) where T : class
        {
            //If the incoming data is not a monogame texture, throw an exception
            if (data.GetType() != typeof(Texture2D))
                throw new Exception($"Data getting injected into {nameof(MonoTexture)} is not of type {nameof(Texture2D)}.  Incorrect type is {data.GetType().ToString()}");

            Texture = data as Texture2D;
        }
        #endregion
    }
}

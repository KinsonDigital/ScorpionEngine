using Microsoft.Xna.Framework.Graphics;
using System;
using KDScorpionCore.Graphics;

namespace MonoScorpPlugin
{
    public class MonoTexture : ITexture
    {
        #region Props
        internal Texture2D Texture { get; private set; }

        public int Width
        {
            get => Texture.Width;
            set { }
        }

        public int Height
        {
            get => Texture.Height;
            set { }
        }
        #endregion

        #region Public Methods
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public T GetTexture<T>() where T : class
        {
            return Texture as T;
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

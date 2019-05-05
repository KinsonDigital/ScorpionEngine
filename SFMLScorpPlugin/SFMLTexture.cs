using KDScorpionCore.Graphics;
using System;
using SFMLLibTexture = SFML.Graphics.Texture;

namespace SFMLScorpPlugin
{
    public class SFMLTexture : ITexture
    {
        #region Fields
        private SFMLLibTexture _texture;
        #endregion


        #region Props
        public int Width => (int)_texture.Size.X;

        public int Height => (int)_texture.Size.Y;
        #endregion


        #region Public Methods
        public object GetData(string dataType) => throw new NotImplementedException();


        public T GetTexture<T>() where T : class => _texture as T;


        public void InjectData<T>(T data) where T : class
        {
            //If the incoming data is not a monogame texture, throw an exception
            if (data.GetType() != typeof(SFMLTexture))
                throw new Exception($"Data getting injected into {nameof(SFMLTexture)} is not of type {nameof(SFML.Graphics.Texture)}.  Incorrect type is {data.GetType().ToString()}");

            _texture = data as SFMLLibTexture;
        }
        #endregion
    }
}

using KDScorpionCore.Graphics;
using SDL2;
using System;

namespace SDLScorpPlugin
{
    public class SDLTexture : ITexture
    {
        #region Fields
        //TODO: This needs to be fixed.  SDL returns a pointer to a texture. Figure this out.
        private IntPtr _texture;
        #endregion


        #region Props
        public int Width => -1;

        public int Height => -1;
        #endregion


        #region Public Methods
        public object GetData(string dataType) => throw new NotImplementedException();


        public T GetTexture<T>() where T : class => _texture as T;


        public void InjectData<T>(T data) where T : class
        {
            ////If the incoming data is not a monogame texture, throw an exception
            //if (data.GetType() != typeof(SDLTexture))
            //    throw new Exception($"Data getting injected into {nameof(SDLTexture)} is not of type {nameof(SFML.Graphics.Texture)}.  Incorrect type is {data.GetType().ToString()}");

            //_texture = data as SDL.Texture;
        }
        #endregion
    }
}

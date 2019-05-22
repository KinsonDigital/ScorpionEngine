using KDScorpionCore.Graphics;
using SDL2;
using System;

namespace SDLScorpPlugin
{
    public class SDLTexture : ITexture
    {
        #region Fields
        //TODO: This needs to be fixed.  SDL returns a pointer to a texture. Figure this out.
        private IntPtr _texturePtr;
        private int _width;
        private int _height;
        #endregion


        #region Constructors
        public SDLTexture(IntPtr texturePtr)
        {
            _texturePtr = texturePtr;

            //Query the texture data which gets the width and height of the texture
            SDL.SDL_QueryTexture(_texturePtr, out uint _, out _, out _width, out _height);
        }
        #endregion


        #region Props
        public int Width => _width;

        public int Height => _height;
        #endregion


        #region Public Methods
        public object GetData(string dataType) => throw new NotImplementedException();


        public T GetTextureAsClass<T>() where T : class => _texturePtr as T;

        public T GetTextureAsStruct<T>() where T : struct
        {
            throw new NotImplementedException();
        }

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

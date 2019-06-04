using KDScorpionCore;
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
        public T GetData<T>(int option) where T : class
        {
            if (option == 1)
            {
                var ptrContainer = new PointerContainer();
                ptrContainer.PackPointer(_texturePtr);


                return ptrContainer as T;
            }

            throw new Exception($"The option '{option}' is not a valid option.");
        }


        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();
        #endregion
    }
}

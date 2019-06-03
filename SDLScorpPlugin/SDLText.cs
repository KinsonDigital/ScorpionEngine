using KDScorpionCore;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLScorpPlugin
{
    public class SDLText : IText
    {
        #region Private Fields
        private readonly IntPtr _rendererPtr;
        private readonly IntPtr _fontPtr;
        private IntPtr _surfacePtr;
        private string _text;
        #endregion


        #region Constructors
        public SDLText(string fontFilePath, string text, int fontSize)
        {
            _text = text;

            _fontPtr = SDL_ttf.TTF_OpenFont(fontFilePath, fontSize);

            //Create a surface for which to render the text to
            _surfacePtr = SDL_ttf.TTF_RenderText_Solid(_fontPtr, text, _color);

            //Create a texture from the surface
            TexturePointer = SDL.SDL_CreateTextureFromSurface(_rendererPtr, _surfacePtr);

            UpdateSize();
        }
        #endregion


        #region Props
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;

                if (_surfacePtr == IntPtr.Zero)
                    SDL.SDL_FreeSurface(_surfacePtr);

                //Create a surface for which to render the text to
                _surfacePtr = SDL_ttf.TTF_RenderText_Solid(_fontPtr, value, _color);

                //Remove the old texture pointer before creating a new one to prevent a memory leak
                if (TexturePointer != IntPtr.Zero)
                    SDL.SDL_DestroyTexture(TexturePointer);

                //Create a texture from the surface
                TexturePointer = SDL.SDL_CreateTextureFromSurface(_rendererPtr, _surfacePtr);

                UpdateSize();

                SDL.SDL_FreeSurface(_surfacePtr);
            }
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public byte[] Color { get; set; }

        public IntPtr TexturePointer { get; set; }
        #endregion


        #region Public Methods
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public IntPtr GetPointer() => throw new NotImplementedException();


        public T GetText<T>() where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectPointer(IntPtr pointer) => _rendererPtr = pointer;
        #endregion


        #region Private Methods
        private void UpdateSize()
        {
            SDL.SDL_QueryTexture(TexturePointer, out var format, out var access, out var width, out var height);
            Width = width;
            Height = height;
        }
        #endregion
    }
}

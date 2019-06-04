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
        private readonly IntPtr _fontPtr;
        private IntPtr _texturePointer;
        private string _text;
        #endregion


        #region Constructors
        public SDLText(string fontFilePath, string text, int fontSize)
        {
            Text = text;
            _fontPtr = SDL_ttf.TTF_OpenFont(fontFilePath, fontSize);
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

                var color = new SDL.SDL_Color();

                if (Color == null || Color.Length <= 0)
                {
                    color.r = 255;
                    color.g = 255;
                    color.b = 255;
                    color.a = 255;
                }
                else
                {
                    color.r = Color.Length >= 1 ? Color[0] : (byte)255;
                    color.g = Color.Length >= 2 ? Color[1] : (byte)255;
                    color.b = Color.Length >= 3 ? Color[2] : (byte)255;
                    color.a = Color.Length >= 4 ? Color[3] : (byte)255;
                }

                //Create a surface for which to render the text to
                var surfacePtr = SDL_ttf.TTF_RenderText_Solid(_fontPtr, value, color);

                //Remove the old texture pointer before creating a new one to prevent a memory leak
                if (_texturePointer != IntPtr.Zero)
                    SDL.SDL_DestroyTexture(_texturePointer);

                //Create a texture from the surface
                _texturePointer = SDL.SDL_CreateTextureFromSurface(SDLEngineCore.RendererPointer, surfacePtr);

                UpdateSize();

                SDL.SDL_FreeSurface(surfacePtr);
            }
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public byte[] Color { get; set; }
        #endregion


        #region Public Methods
        public T GetData<T>(int option) where T : class
        {
            var ptrContainer = new PointerContainer();

            ptrContainer.PackPointer(_texturePointer);


            return ptrContainer as T;
        }


        public T GetText<T>() where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Private Methods
        private void UpdateSize()
        {
            SDL.SDL_QueryTexture(_texturePointer, out var format, out var access, out var width, out var height);
            Width = width;
            Height = height;
        }
        #endregion
    }
}

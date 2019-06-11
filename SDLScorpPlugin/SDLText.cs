using KDScorpionCore;
using SDL2;
using System;

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
        public SDLText(IntPtr fontPtr, string text)
        {
            _fontPtr = fontPtr;
            Color = new byte[] { 255, 255, 255, 255 };
            Text = text;
        }
        #endregion


        #region Props
        public string Text
        {
            get => _text;
            set
            {
                _text = value;

                var color = new SDL.SDL_Color()
                {
                    r = Color.Length >= 1 ? Color[0] : (byte)255,
                    g = Color.Length >= 2 ? Color[1] : (byte)255,
                    b = Color.Length >= 3 ? Color[2] : (byte)255,
                    a = Color.Length >= 4 ? Color[3] : (byte)255,
                };

                //Create a surface for which to render the text to
                var surfacePtr = SDL_ttf.TTF_RenderText_Solid(_fontPtr, value, color);

                //Remove the old texture pointer before creating a new one to prevent a memory leak
                if (_texturePointer != IntPtr.Zero)
                    SDL.SDL_DestroyTexture(_texturePointer);

                //Create a texture from the surface
                _texturePointer = SDL.SDL_CreateTextureFromSurface(SDLEngineCore.RendererPointer, surfacePtr);

                SDL.SDL_FreeSurface(surfacePtr);
            }
        }

        public int Width
        {
            get
            {
                SDL.SDL_QueryTexture(_texturePointer, out var _, out var _, out var width, out var _);


                return width;
            }
        }

        public int Height
        {
            get
            {
                SDL.SDL_QueryTexture(_texturePointer, out var _, out var _, out var _, out var height);


                return height;
            }
        }

        public byte[] Color { get; set; }
        #endregion


        #region Public Methods
        public T GetData<T>(int option) where T : class
        {
            if (option == 1)
            {
                var ptrContainer = new PointerContainer();

                ptrContainer.PackPointer(_texturePointer);


                return ptrContainer as T;
            }


            //TODO: Create a custome InvalidGetDataOptionException class.  Implement this for all GetData<T> implementations
            throw new Exception($"The option '{option}' is not valid. \n\nValid options are 1.");
        }


        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();
        #endregion
    }
}

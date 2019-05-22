using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDLScorpPlugin
{
    public class SDLRenderer : IRenderer
    {
        private bool _beginInvoked = false;
        private IntPtr _rendererPtr;


        public void Start()
        {
            _beginInvoked = true;
        }


        public void End()
        {
            if (!_beginInvoked)
                throw new Exception($"The '{nameof(Start)}' method must be invoked first before the '{nameof(End)}' method.");

            SDL.SDL_RenderPresent(_rendererPtr);

            _beginInvoked = false;
        }


        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            //TODO: See if we can clear to a particular color
            SDL.SDL_RenderClear(_rendererPtr);
        }


        public void Render(ITexture texture, float x, float y) => Render(texture, x, y, 0f, 0f, 255, 255, 255, 255);


        public void Render(ITexture texture, float x, float y, float angle) => Render(texture, x, y, angle, 0f, 255, 255, 255, 255);


        public void Render(ITexture texture, float x, float y, float angle, float size, byte red, byte green, byte blue, byte alpha)
        {
            var textureOrigin = new SDL.SDL_Point()
            {
                x = texture.Width / 2,
                y = texture.Height / 2
            };

            var srcRect = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = texture.Width,
                h = texture.Height
            };

            var destRect = new SDL.SDL_Rect()
            {
                x = (int)x,//Texture X on screen
                y = (int)y,//Texture Y on screen
                w = texture.Width,
                h = texture.Height
            };

            var texturePtr = texture.GetTextureAsStruct<IntPtr>();

            SDL.SDL_SetTextureBlendMode(texturePtr, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetTextureColorMod(texturePtr, red, green, blue);
            SDL.SDL_SetTextureAlphaMod(texturePtr, alpha);
            SDL.SDL_RenderCopyEx(_rendererPtr, texturePtr, ref srcRect, ref destRect, 0.0, ref textureOrigin, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }


        public void RenderTextureArea(ITexture texture, Rect area, float x, float y)
        {
            var textureOrigin = new SDL.SDL_Point()
            {
                x = texture.Width / 2,
                y = texture.Height / 2
            };

            var srcRect = new SDL.SDL_Rect()
            {
                x = (int)area.X,
                y = (int)area.Y,
                w = texture.Width,
                h = texture.Height
            };

            var destRect = new SDL.SDL_Rect()
            {
                x = (int)x,//Texture X on screen
                y = (int)y,//Texture Y on screen
                w = texture.Width,
                h = texture.Height
            };

            var texturePtr = texture.GetTextureAsStruct<IntPtr>();

            SDL.SDL_RenderCopyEx(_rendererPtr, texturePtr, ref srcRect, ref destRect, 0.0, ref textureOrigin, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }


        public void Render(IText text, float x, float y)
        {
            throw new NotImplementedException();
        }


        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY) =>
            RenderLine(lineStartX, lineStartY, lineStopX, lineStopY, new byte[] { 255, 255, 255, 255 });


        public void FillCircle(float centerX, float centerY, float radius, byte[] color)
        {
            /*Midpoint Algorith
             * 1. https://stackoverflow.com/questions/38334081/howto-draw-circles-arcs-and-vector-graphics-in-sdl
             * 2. https://en.wikipedia.org/wiki/Midpoint_circle_algorithm#C_Example
             */

            if (color == null || color.Length != 4)
                throw new ArgumentException($"The argument '{nameof(color)}' must not be null and must have exactly 4 elements.");

            int diameter = (int)(radius * 2f);

            int x = (int)(radius - 1);
            int y = 0;
            int tx = 1;
            int ty = 1;
            int error = (tx - diameter);

            int centerXIntValue = (int)centerX;
            int centerYIntValue = (int)centerY;

            SDL.SDL_SetRenderDrawColor(_rendererPtr, color[0], color[1], color[2], color[3]);

            while (x >= y)
            {
                //Each of the following renders an octant of the circle
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue + x, centerYIntValue - y);
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue + x, centerYIntValue + y);
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue - x, centerYIntValue - y);
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue - x, centerYIntValue + y);
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue + y, centerYIntValue - x);
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue + y, centerYIntValue + x);
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue - y, centerYIntValue - x);
                SDL.SDL_RenderDrawPoint(_rendererPtr, centerXIntValue - y, centerYIntValue + x);

                if (error <= 0)
                {
                    ++y;
                    error += ty;
                    ty += 2;
                }

                if (error > 0)
                {
                    --x;
                    tx += 2;
                    error += (tx - diameter);
                }
            }
        }


        public void RenderLine(float startX, float startY, float endX, float endY, byte[] color)
        {
            if (color == null || color.Length != 4)
                throw new ArgumentException($"The argument '{nameof(color)}' must not be null and must have exactly 4 elements.");

            SDL.SDL_SetRenderDrawColor(_rendererPtr, color[0], color[1], color[2], color[3]);
            SDL.SDL_RenderDrawLine(_rendererPtr, (int)startX, (int)startY, (int)endX, (int)endY);
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectPointer(IntPtr pointer) => _rendererPtr = pointer;
    }
}

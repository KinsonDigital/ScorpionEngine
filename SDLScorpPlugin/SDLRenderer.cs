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
        #region Private Fields
        private bool _beginInvoked = false;
        private IntPtr _rendererPtr;
        #endregion


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


        /// <summary>
        /// Clears the screen to the color using the given color components of
        /// <paramref name="red"/>, <paramref name="green"/>, <paramref name="blue"/> and <paramref name="alpha"/>.
        /// </summary>
        /// <param name="red">The red component of the color to clearn the screen to.</param>
        /// <param name="green">The green component of the color to clearn the screen to.</param>
        /// <param name="blue">The blue component of the color to clearn the screen to.</param>
        /// <param name="alpha">The alpha component of the color to clearn the screen to.</param>
        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            SDL.SDL_SetRenderDrawColor(_rendererPtr, red, green, blue, alpha);
            SDL.SDL_RenderClear(_rendererPtr);
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render the at.</param>
        /// <param name="y">The Y coordinate location on the screen to render the at.</param>
        public void Render(ITexture texture, float x, float y) => Render(texture, x, y, 0f, 0f, new byte[] { 255, 255, 255, 255 });


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render the at.</param>
        /// <param name="y">The Y coordinate location on the screen to render the at.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        public void Render(ITexture texture, float x, float y, float angle) => Render(texture, x, y, angle, 0f, new byte[] { 255, 255, 255, 255 });


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render the at.</param>
        /// <param name="y">The Y coordinate location on the screen to render the at.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        /// <param name="color">The array of color components of the color to add to the texture.
        /// Only aloud to have 4 elements or less.  Any more than 4 elements will throw an exception.
        /// If element does not exist, the value 255 will be used.</param>
        public void Render(ITexture texture, float x, float y, float angle, float size, byte[] color)
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

            var red = color.Length >= 1 ? color[0] : (byte)255;
            var green = color.Length >= 2 ? color[1] : (byte)255;
            var blue = color.Length >= 3 ? color[2] : (byte)255;
            var alpha = color.Length >= 4 ? color[3] : (byte)255;

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

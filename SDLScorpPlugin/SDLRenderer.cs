using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using SDL2;
using System;

namespace SDLScorpPlugin
{
    /// <summary>
    /// Provides methods for rendering textures, text and primitives to the screen.
    /// </summary>
    public class SDLRenderer : IRenderer
    {
        #region Private Fields
        private bool _beginInvoked = false;
        private IntPtr _rendererPtr;
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the process of rendering a batch of <see cref="Texture"/>s, <see cref="GameText"/> items
        /// or primitives.  This method must be invoked before rendering.
        /// </summary>
        public void Start()
        {
            _beginInvoked = true;
        }


        /// <summary>
        /// Stops the batching process and renders all of the batches textures to the screen.
        /// </summary>
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
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        public void Render(ITexture texture, float x, float y) => Render(texture, x, y, 0f, 0f, new byte[] { 255, 255, 255, 255 });


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        public void Render(ITexture texture, float x, float y, float angle) => Render(texture, x, y, angle, 1f, new byte[] { 255, 255, 255, 255 });


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
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
                w = (int)(texture.Width * size),//Scaled occurding to size
                h = (int)(texture.Height * size)
            };

            var texturePtr = texture.GetData<PointerContainer>(1).UnpackPointer();

            var red = color.Length >= 1 ? color[0] : (byte)255;
            var green = color.Length >= 2 ? color[1] : (byte)255;
            var blue = color.Length >= 3 ? color[2] : (byte)255;
            var alpha = color.Length >= 4 ? color[3] : (byte)255;

            SDL.SDL_SetTextureBlendMode(texturePtr, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetTextureColorMod(texturePtr, red, green, blue);
            SDL.SDL_SetTextureAlphaMod(texturePtr, alpha);
            SDL.SDL_RenderCopyEx(_rendererPtr, texturePtr, ref srcRect, ref destRect, 0.0, ref textureOrigin, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }


        /// <summary>
        /// Renders an area of the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="area">The area/section of the texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
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

            var texturePtr = texture.GetData<PointerContainer>(1).UnpackPointer();

            SDL.SDL_RenderCopyEx(_rendererPtr, texturePtr, ref srcRect, ref destRect, 0.0, ref textureOrigin, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }


        /// <summary>
        /// Renders the given <paramref name="text"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        public void Render(IText text, float x, float y)
        {
            var texturePtr = text.GetData<PointerContainer>(1).UnpackPointer();

            //TODO:  Check for color index values first
            SDL.SDL_SetTextureColorMod(texturePtr, text.Color[0], text.Color[1], text.Color[2]);
            SDL.SDL_SetTextureAlphaMod(texturePtr, text.Color[3]);
            SDL.SDL_SetTextureBlendMode(texturePtr, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);


            var srcRect = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = text.Width,
                h = text.Height
            };

            var destRect = new SDL.SDL_Rect()
            {
                x = (int)x,
                y = (int)y,
                w = text.Width,
                h = text.Height
            };

            //Render texture to screen
            SDL.SDL_RenderCopy(_rendererPtr, texturePtr, ref srcRect, ref destRect);
        }


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY) =>
            RenderLine(lineStartX, lineStartY, lineStopX, lineStopY, new byte[] { 255, 255, 255, 255 });


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        /// <param name="color">The color of the line.  Must be a total of 4 color component channels consisting of
        /// red, green, blue and alpha in that order.  A missing element will result in a default value of 255.</param>
        public void RenderLine(float startX, float startY, float endX, float endY, byte[] color)
        {
            SDL.SDL_SetRenderDrawColor(_rendererPtr,
                                       color == null || color.Length >= 1 ? color[0] : (byte)255,
                                       color == null || color.Length >= 2 ? color[1] : (byte)255,
                                       color == null || color.Length >= 3 ? color[2] : (byte)255,
                                       color == null || color.Length >= 4 ? color[3] : (byte)255);

            SDL.SDL_RenderDrawLine(_rendererPtr, (int)startX, (int)startY, (int)endX, (int)endY);
        }


        /// <summary>
        /// Creates a filled circle at the given <paramref name="x"/> and <paramref name="y"/> location
        /// with the given <paramref name="radius"/> and with the given <paramref name="color"/>.  The
        /// <paramref name="x"/> and <paramref name="y"/> coordinates represent the center of the circle.
        /// </summary>
        /// <param name="x">The X coordinate on the screen of where to render the circle.</param>
        /// <param name="y">The Y coordinate on the screen of where to render the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.  Must be a total of 4 color component channels consisting of
        /// red, green, blue and alpha in that order.  A missing element will result in a default value of 255.</param>
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

            SDL.SDL_SetRenderDrawColor(_rendererPtr,
                                       color == null || color.Length >= 1 ? color[0] : (byte)255,
                                       color == null || color.Length >= 2 ? color[1] : (byte)255,
                                       color == null || color.Length >= 3 ? color[2] : (byte)255,
                                       color == null || color.Length >= 4 ? color[3] : (byte)255);

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


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class
        {
            //TODO: Replace this with a custom exception called InjectDataException class
            if (data.GetType() != typeof(PointerContainer))
                throw new Exception($"Data getting injected into {nameof(SDLRenderer)} is not of type {nameof(PointerContainer)}.  Incorrect type is {data.GetType().ToString()}");

            _rendererPtr = (data as PointerContainer).UnpackPointer();
        }


        /// <summary>
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class
        {
            var ptrContainer = new PointerContainer();
            ptrContainer.PackPointer(_rendererPtr);


            return ptrContainer as T;
        }
        #endregion
    }
}

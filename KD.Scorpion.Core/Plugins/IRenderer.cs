using KDScorpionCore.Graphics;
using System;

namespace KDScorpionCore.Plugins
{
    /// <summary>
    /// Provides methods for rendering textures, text and primitives to the screen.
    /// </summary>
    public interface IRenderer : IPlugin, IDisposable
    {
        #region Methods
        /// <summary>
        /// Starts the process of rendering a batch of <see cref="Texture"/>s, <see cref="GameText"/> items
        /// or primitives.  This method must be invoked before rendering.
        /// </summary>
        void Start();


        /// <summary>
        /// Stops the batching process and renders all of the batches textures to the screen.
        /// </summary>
        void End();


        /// <summary>
        /// Clears the screen to the color using the given color components of
        /// <paramref name="red"/>, <paramref name="green"/>, <paramref name="blue"/> and <paramref name="alpha"/>.
        /// </summary>
        /// <param name="red">The red component of the color to clearn the screen to.</param>
        /// <param name="green">The green component of the color to clearn the screen to.</param>
        /// <param name="blue">The blue component of the color to clearn the screen to.</param>
        /// <param name="alpha">The alpha component of the color to clearn the screen to.</param>
        void Clear(byte red, byte green, byte blue, byte alpha);


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        void Render(ITexture texture, float x, float y);


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        void Render(ITexture texture, float x, float y, float angle);


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
        void Render(ITexture texture, float x, float y, float angle, float size, byte[] color);


        /// <summary>
        /// Renders the given <paramref name="text"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        void Render(IText text, float x, float y);


        /// <summary>
        /// Renders an area of the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="area">The area/section of the texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        void RenderTextureArea(ITexture texture, Rect area, float x, float y);


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY);


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        /// <param name="color">The color of the line.  Must be a total of 4 color component channels consisting of
        /// red, green, blue and alpha in that order.  A missing element will result in a default value of 255.</param>
        void RenderLine(float startX, float startY, float endX, float endY, byte[] color);


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
        void FillCircle(float x, float y, float radius, byte[] color);


        /// <summary>
        /// Renders a filled rectangle using the given <paramref name="rect"/>
        /// and using the given <paramref name="color"/>.
        /// </summary>
        /// <param name="rect">The rectangle to render.</param>
        /// <param name="color">The color to render the rectangle.</param>
        void FillRect(Rect rect, byte[]color);
        #endregion
    }
}

using KDScorpionCore.Graphics;

namespace KDScorpionCore.Plugins
{
    public interface IRenderer : IPlugin
    {
        #region Methods
        /// <summary>
        /// Clears the screen to the color using the given color components of
        /// <paramref name="red"/>, <paramref name="green"/>, <paramref name="blue"/> and <paramref name="alpha"/>.
        /// </summary>
        /// <param name="red">The red component of the color to clearn the screen to.</param>
        /// <param name="green">The green component of the color to clearn the screen to.</param>
        /// <param name="blue">The blue component of the color to clearn the screen to.</param>
        /// <param name="alpha">The alpha component of the color to clearn the screen to.</param>
        void Clear(byte red, byte green, byte blue, byte alpha);


        void Start();


        void End();


        void FillCircle(float x, float y, float radius, byte[] color);


        void RenderLine(float startX, float startY, float endX, float endY, byte[] color);


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render the at.</param>
        /// <param name="y">The Y coordinate location on the screen to render the at.</param>
        void Render(ITexture texture, float x, float y);


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render the at.</param>
        /// <param name="y">The Y coordinate location on the screen to render the at.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        void Render(ITexture texture, float x, float y, float angle);


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
        void Render(ITexture texture, float x, float y, float angle, float size, byte[] color);


        void Render(IText text, float x, float y);


        void RenderTextureArea(ITexture texture, Rect area, float x, float y);


        void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY);
        #endregion
    }
}

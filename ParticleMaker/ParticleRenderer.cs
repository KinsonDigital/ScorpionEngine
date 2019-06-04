using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ParticleMaker
{
    /// <summary>
    /// Provides methods for rendering textures, text and primitives to the screen.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ParticleRenderer : IRenderer
    {
        #region Fields
        private SpriteBatch _spriteBatch;
        #endregion


        #region Public Methods
        /// <summary>
        /// Creates a new instance of <see cref="ParticleRenderer"/>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public ParticleRenderer(SpriteBatch spriteBatch) => _spriteBatch = spriteBatch;


        /// <summary>
        /// Starts the process of rendering a batch of <see cref="Texture"/>s, <see cref="GameText"/> items
        /// or primitives.  This method must be invoked before rendering.
        /// </summary>
        public void Start() => throw new NotImplementedException();


        /// <summary>
        /// Stops the batching process and renders all of the batches textures to the screen.
        /// </summary>
        public void End() => throw new NotImplementedException();


        /// <summary>
        /// Clears the screen by setting entire screen to the given color components.
        /// </summary>
        /// <param name="red">The red color value.</param>
        /// <param name="green">The green color value.</param>
        /// <param name="blue">The blue color value.</param>
        /// <param name="alpha">The alpha value of the color.</param>
        public void Clear(byte red, byte green, byte blue, byte alpha) => throw new NotImplementedException();


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/> and <paramref name="y"/> location
        /// set at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="texture">The texture to render to the screen.</param>
        /// <param name="x">The x location to render the <paramref name="texture"/> at.</param>
        /// <param name="y">The y location to render the <paramref name="texture"/> at.</param>
        public void Render(ITexture texture, float x, float y) => throw new NotImplementedException();


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/> and <paramref name="y"/> location
        /// set at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="texture">The texture to render to the screen.</param>
        /// <param name="x">The x location to render the <paramref name="texture"/> at.</param>
        /// <param name="y">The y location to render the <paramref name="texture"/> at.</param>
        /// <param name="angle">The angle to render the <paramref name="texture"/> at.</param>
        public void Render(ITexture texture, float x, float y, float angle) => throw new NotImplementedException();


        /// <summary>
        /// Renders the given <paramref name="texture"/> using the given parameters.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate on the screen to render the texture.</param>
        /// <param name="y">The X coordinate on the screen to render the texture.</param>
        /// <param name="angle">The angle to set the texture at.</param>
        /// <param name="size">The size of the texture.</param>
        /// <param name="red">The red component of the color to apply to the texture.</param>
        /// <param name="green">The green component of the color to apply to the texture.</param>
        /// <param name="blue">The blue component of the color to apply to the texture.</param>
        /// <param name="alpha">The alpha component of the color to apply to the texture.</param>
        public void Render(ITexture texture, float x, float y, float angle, float size, byte[] color)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            var red = color.Length >= 1 ? color[0] : 255;
            var green = color.Length >= 2 ? color[1] : 255;
            var blue = color.Length >= 3 ? color[2] : 255;
            var alpha = color.Length >= 4 ? color[3] : 255;

            _spriteBatch.Draw(texture.GetData<Texture2D>(1), position, null, new Color(red, green, blue, alpha), angle.ToRadians(), textureOrigin, size, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders an area of the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="area">The area/section of the texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        public void RenderTextureArea(ITexture texture, Rect area, float x, float y) => throw new NotImplementedException();


        /// <summary>
        /// Renders the given <paramref name="text"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        public void Render(IText text, float x, float y) => throw new NotImplementedException();


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY) => throw new NotImplementedException();


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        /// <param name="color">The color of the line.  Must be a total of 4 color component channels consisting of
        /// red, green, blue and alpha in that order.  A missing element will result in a default value of 255.</param>
        public void RenderLine(float startX, float startY, float endX, float endY, byte[] color) => throw new NotImplementedException();


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
        public void FillCircle(float x, float y, float radius, byte[] color) => throw new NotImplementedException();


        /// <summary>
        /// Renders a filled rectangle using the given <paramref name="rect"/>
        /// and using the given <paramref name="color"/>.
        /// </summary>
        /// <param name="rect">The rectangle to render.</param>
        /// <param name="color">The color to render the rectangle.</param>
        public void FillRect(Rect rect, byte[] color)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();
        #endregion
    }
}

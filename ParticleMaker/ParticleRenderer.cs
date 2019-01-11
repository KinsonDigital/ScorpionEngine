using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticleMaker
{
    /// <summary>
    /// Renders particles to the screen.
    /// </summary>
    public class ParticleRenderer : IRenderer
    {
        #region Fields
        private SpriteBatch _spriteBatch;
        #endregion


        #region Public Methods
        //TODO: Move the injection of this sprite batch to the InjectData method.
        /// <summary>
        /// Creates a new instance of <see cref="ParticleRenderer"/>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public ParticleRenderer(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }


        /// <summary>
        /// Clears the screen by setting entire screen to the given color components.
        /// </summary>
        /// <param name="red">The red color value.</param>
        /// <param name="green">The green color value.</param>
        /// <param name="blue">The blue color value.</param>
        /// <param name="alpha">The alpha value of the color.</param>
        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Ends the rendering process.
        /// </summary>
        public void End()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders a filled circle to the screen at the given <paramref name="x"/> and <paramref name="y"/> location
        /// with the given <paramref name="radius"/> and <paramref name="color"/>.
        /// </summary>
        /// <param name="x">The X location of where to render the circle.</param>
        /// <param name="y">The Y location of the where to render the circle.</param>
        /// <param name="radius">The radius to the circle.</param>
        /// <param name="color">The color of the circle.</param>
        public void FillCircle(float x, float y, float radius, byte[] color)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Gets the data based on the given type.
        /// </summary>
        /// <param name="dataType">The type of data to get.</param>
        /// <returns></returns>
        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        //TODO: Use this to inject the spritbatch into class.
        /// <summary>
        /// Injects the given <paramref name="data"/> into the class for use.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders a line to the screen based on the given parameters.
        /// </summary>
        /// <param name="lineStartX">The starting X location of the line.</param>
        /// <param name="lineStartY">The starting Y location of the line.</param>
        /// <param name="lineStopX">The ending X location of the line.</param>
        /// <param name="lineStopY">The ending Y location of the line.</param>
        /// <param name="color">The color to render the line.</param>
        public void RenderLine(float startX, float startY, float endX, float endY, byte[] color)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/> and <paramref name="y"/> location
        /// set at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="texture">The texture to render to the screen.</param>
        /// <param name="x">The x location to render the <paramref name="texture"/> at.</param>
        /// <param name="y">The y location to render the <paramref name="texture"/> at.</param>
        public void Render(ITexture texture, float x, float y)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/> and <paramref name="y"/> location
        /// set at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="texture">The texture to render to the screen.</param>
        /// <param name="x">The x location to render the <paramref name="texture"/> at.</param>
        /// <param name="y">The y location to render the <paramref name="texture"/> at.</param>
        /// <param name="angle">The angle to render the <paramref name="texture"/> at.</param>
        public void Render(ITexture texture, float x, float y, float angle)
        {
            throw new NotImplementedException();
        }


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
        public void Render(ITexture texture, float x, float y, float angle, float size, byte red, byte green, byte blue, byte alpha)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, null, new Color(red, green, blue, alpha), angle.ToRadians(), textureOrigin, size, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders the given <paramref name="text"/> to the screen at the given <paramref name="x"/> and <paramref name="y"/> location.
        /// </summary>
        /// <param name="text">The text to render to the screen.</param>
        /// <param name="x">The x location to render the <paramref name="text"/> at.</param>
        /// <param name="y">The y location to render the <paramref name="text"/> at.</param>
        public void Render(IText text, float x, float y)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders a line to the screen based on the given parameters.
        /// </summary>
        /// <param name="lineStartX">The starting X location of the line.</param>
        /// <param name="lineStartY">The starting Y location of the line.</param>
        /// <param name="lineStopX">The ending X location of the line.</param>
        /// <param name="lineStopY">The ending Y location of the line.</param>
        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> to the given <paramref name="area"/> at the given <paramref name="x"/> and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="area">The area of the texture to render.</param>
        /// <param name="x">The X location to render the texture.</param>
        /// <param name="y">The Y location to render the texture.</param>
        public void RenderTextureArea(ITexture texture, Rect area, float x, float y)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Starts the rendering process.  Must be called before rendering.
        /// </summary>
        public void Start()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

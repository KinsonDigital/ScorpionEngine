using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using System;

namespace MonoScorpPlugin
{
    /// <summary>
    /// Provides methods for rendering textures, text and primitives to the screen.
    /// </summary>
    public class MonoRenderer : IRenderer
    {
        #region Private Fields
        private static SpriteBatch _spriteBatch;
        //This will treated as a MonoGame graphics device.
        private static dynamic _graphicsDevice;
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the process of rendering a batch of <see cref="Texture"/>s, <see cref="GameText"/> items
        /// or primitives.  This method must be invoked before rendering.
        /// </summary>
        public void Start()
        {
            _spriteBatch.Begin();
        }


        /// <summary>
        /// Stops the batching process and renders all of the batches textures to the screen.
        /// </summary>
        public void End()
        {
            _spriteBatch.End();
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
            _graphicsDevice.Clear(new Color(red, green, blue, alpha));
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        public void Render(ITexture texture, float x, float y)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTextureAsClass<Texture2D>(), position, null, Color.White, 0, textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given 
        /// <paramref name="angle"/> in degrees.
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        public void Render(ITexture texture, float x, float y, float angle)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTextureAsClass<Texture2D>(), position, null, Color.White, angle.ToRadians(), textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given 
        /// <paramref name="angle"/> in degrees.
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        /// <param name="color">The array of color components of the color to add to the texture.
        /// Only aloud to have 4 elements or less.  Any more than 4 elements will throw an exception.
        /// If element does not exist, the value 255 will be used.</param>
        public void Render(ITexture texture, float x, float y, float angle, float size, byte[] color)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            var red = color.Length >= 1 ? color[0] : 255;
            var green = color.Length >= 2 ? color[1] : 255;
            var blue = color.Length >= 3 ? color[2] : 255;
            var alpha = color.Length >= 4 ? color[3] : 255;

            _spriteBatch.Draw(texture.GetTextureAsClass<Texture2D>(), position, null, new Color(red, green, blue, alpha), angle.ToRadians(), textureOrigin, size, SpriteEffects.None, 0f);
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
            var srcRect = new Rectangle((int)area.X, (int)area.Y, (int)area.Width, (int)area.Height);
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTextureAsClass<Texture2D>(), position, srcRect, Color.Orange, 0, textureOrigin, 1, SpriteEffects.None, 0f);
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
            var color = new Color(text.Color[0], text.Color[1], text.Color[2]);

            _spriteBatch.DrawString(text.GetText<SpriteFont>(), text.Text, new Vector2(x, y), color);
        }


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY)
        {
            var colorValues = new byte[]
            {
                Color.White.R,
                Color.White.G,
                Color.White.B,
                Color.White.A
            };

            RenderLine(lineStartX, lineStartY, lineStopX, lineStopY, colorValues);
        }


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
            var lineColor = new Color(color == null || color.Length >= 1 ? color[0] : 255,
                                      color == null || color.Length >= 2 ? color[1] : 255,
                                      color == null || color.Length >= 3 ? color[2] : 255,
                                      color == null || color.Length >= 4 ? color[3] : 255);

            _spriteBatch.DrawLine(startX, startY, endX, endY, lineColor);
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
        public void FillCircle(float x, float y, float radius, byte[] color)
        {
            //If the color param does not have at least 4 items, throw an exception
            if (color == null)
                throw new ArgumentException($"The param '{nameof(color)}' cannot be null");

            if (color.Length < 4)
                throw new ArgumentException($"The param '{nameof(color)}' must have at lest 4 items.");

            var circleColor = new Color(color == null || color.Length >= 1 ? color[0] : 255,
                                        color == null || color.Length >= 2 ? color[1] : 255,
                                        color == null || color.Length >= 3 ? color[2] : 255,
                                        color == null || color.Length >= 4 ? color[3] : 255);

            _spriteBatch.FillCircle(new Vector2(x, y), radius, (int)radius * 2, circleColor);
        }


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class
        {
            _graphicsDevice = data;

            _spriteBatch = new SpriteBatch(_graphicsDevice as GraphicsDevice);
        }


        /// <summary>
        /// Injects a pointer into the plugin for use.
        /// </summary>
        /// <param name="pointer"></param>
        public object GetData(string dataType) => throw new NotImplementedException();


        /// <summary>
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public void InjectPointer(IntPtr pointer) => throw new NotImplementedException();
        #endregion
    }
}

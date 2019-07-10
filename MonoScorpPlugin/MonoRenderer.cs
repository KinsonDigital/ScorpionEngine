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
        public void Begin() => _spriteBatch.Begin();


        /// <summary>
        /// Stops the batching process and renders all of the batched textures to the screen.
        /// </summary>
        public void End() => _spriteBatch.End();


        /// <summary>
        /// Clears the screen to the given color.
        /// </summary>
        /// <param name="color">The color to clear the screen to.</param>
        public void Clear(GameColor color) => _graphicsDevice.Clear(new Color(color.Red, color.Green, color.Blue, color.Alpha));


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

            _spriteBatch.Draw(texture.GetData<Texture2D>(1), position, null, Color.White, 0, textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        public void Render(ITexture texture, float x, float y, float angle)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetData<Texture2D>(1), position, null, Color.White, angle.ToRadians(), textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location and rotates the texture to the given
        /// <paramref name="angle"/> in degrees.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        /// <param name="angle">The angle in degrees to rotate the texture to.</param>
        /// <param name="color">The color to apply to the texture.</param>
        public void Render(ITexture texture, float x, float y, float angle, float size, GameColor color)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetData<Texture2D>(1), position,
                null, new Color(color.Red, color.Green, color.Blue, color.Alpha),
                angle.ToRadians(), textureOrigin, 
                size, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders the given <paramref name="text"/> at the given <paramref name="x"/>
        /// and <paramref name="y"/> location.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="x">The X coordinate location on the screen to render.</param>
        /// <param name="y">The Y coordinate location on the screen to render.</param>
        public void Render(IText text, float x, float y) => Render(text, x, y, text.Color);


        /// <summary>
        /// Renders the given text at the given <paramref name="x"/> and <paramref name="y"/>
        /// location and using the given <paramref name="color"/>.
        /// </summary>
        /// <param name="text">The text to render.</param>
        /// <param name="x">The X coordinate location of where to render the text.</param>
        /// <param name="y">The Y coordinate location of where to render the text.</param>
        /// <param name="color">The color to render the text.</param>
        public void Render(IText text, float x, float y, GameColor color) =>
            _spriteBatch.DrawString(text.GetData<SpriteFont>(1), text.Text, new Vector2(x, y), color.ToXNAColor());


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

            _spriteBatch.Draw(texture.GetData<Texture2D>(1), position, srcRect, Color.Orange, 0, textureOrigin, 1, SpriteEffects.None, 0f);
        }


        /// <summary>
        /// Renders a line using the given start and stop coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY) =>
            RenderLine(lineStartX, lineStartY, lineStopX, lineStopY, new GameColor(255, 255, 255, 255));


        /// <summary>
        /// Renders a line using the given start and stop X and Y coordinates.
        /// </summary>
        /// <param name="lineStartX">The starting X coordinate of the line.</param>
        /// <param name="lineStartY">The starting Y coordinate of the line.</param>
        /// <param name="lineStopX">The ending X coordinate of the line.</param>
        /// <param name="lineStopY">The ending Y coordinate of the line.</param>
        /// <param name="color">The color of the line.</param>
        public void RenderLine(float startX, float startY, float endX, float endY, GameColor color) =>
            _spriteBatch.DrawLine(startX, startY, endX, endY, new Color(color.Red, color.Green, color.Blue, color.Alpha));


        /// <summary>
        /// Creates a filled circle at the given <paramref name="x"/> and <paramref name="y"/> location
        /// with the given <paramref name="radius"/> and with the given <paramref name="color"/>.  The
        /// <paramref name="x"/> and <paramref name="y"/> coordinates represent the center of the circle.
        /// </summary>
        /// <param name="x">The X coordinate on the screen of where to render the circle.</param>
        /// <param name="y">The Y coordinate on the screen of where to render the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        public void FillCircle(float x, float y, float radius, GameColor color) =>
            _spriteBatch.FillCircle(new Vector2(x, y), radius, (int)radius * 2, new Color(color.Red, color.Green, color.Blue, color.Alpha));


        /// <summary>
        /// Renders a filled rectangle using the given <paramref name="rect"/>
        /// and using the given <paramref name="color"/>.
        /// </summary>
        /// <param name="rect">The rectangle to render.</param>
        /// <param name="color">The color of the rectangle.</param>
        public void FillRect(Rect rect, GameColor color) => _spriteBatch.FillRectangle(rect.ToRectangle(), color.ToXNAColor());


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
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Properly destroys the SDL renderer.
        /// </summary>
        public void Dispose() => _spriteBatch.Dispose();
        #endregion
    }
}

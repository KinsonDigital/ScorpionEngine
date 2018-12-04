using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionCore;
using ScorpionCore.Graphics;
using ScorpionCore.Plugins;
using System;

namespace MonoScorpPlugin
{
    //TODO: Add docs to this file
    public class MonoRenderer : IRenderer
    {
        private static SpriteBatch _spriteBatch;
        //This will treated as a MonoGame graphics device.
        private static dynamic _graphicsDevice;


        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            _graphicsDevice.Clear(new Color(red, green, blue, alpha));
        }


        public void Start()
        {
            _spriteBatch.Begin();
        }


        public void End()
        {
            _spriteBatch.End();
        }


        public void FillCircle(float x, float y, float radius, byte[] color)
        {
            //If the color param does not have at least 4 items, throw an exception
            if (color == null)
                throw new ArgumentException($"The param '{nameof(color)}' cannot be null");

            if (color.Length < 4)
                throw new ArgumentException($"The param '{nameof(color)}' must have at lest 4 items.");

            var circleColor = new Color(color[0], color[1], color[2], color[3]);

            _spriteBatch.FillCircle(new Vector2(x, y), radius, (int)radius * 2, circleColor);
        }


        public void Render(ITexture texture, float x, float y)
        {
            var srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, srcRect, Color.White, 0, textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        public void Line(float startX, float startY, float endX, float endY, byte[] color)
        {
            //If the color param does not have at least 4 items, throw an exception
            if (color == null)
                throw new ArgumentException($"The param '{nameof(color)}' cannot be null");

            if (color.Length < 4)
                throw new ArgumentException($"The param '{nameof(color)}' must have at lest 4 items.");

            var lineColor = new Color(color[0], color[1], color[2], color[3]);

            _spriteBatch.DrawLine(startX, startY, endX, endY, lineColor);
        }


        //Angle is in degrees
        public void Render(ITexture texture, float x, float y, float angle)
        {
            var srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, srcRect, Color.White, angle.ToRadians(), textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        //Angle is in degrees
        public void Render(ITexture texture, float x, float y, float angle, float size, byte red, byte green, byte blue, byte alpha)
        {
            var srcRect = new Rectangle(0, 0, texture.Width, texture.Height);
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, srcRect, new Color(red, green, blue, alpha), angle.ToRadians(), textureOrigin, size, SpriteEffects.None, 0f);
        }


        public void Render(IText text, float x, float y)
        {
            var color = new Color(text.Color[0], text.Color[1], text.Color[2]);

            _spriteBatch.DrawString(text.GetText<SpriteFont>(), text.Text, new Vector2(x, y), color);
        }


        public void InjectData<T>(T data) where T : class
        {
            _graphicsDevice = data;

            _spriteBatch = new SpriteBatch(_graphicsDevice as GraphicsDevice);
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }


        public void RenderLine(float lineStartX, float lineStartY, float lineStopX, float lineStopY)
        {
            _spriteBatch.DrawLine(lineStartX, lineStartY, lineStopX, lineStopY, Color.White);
        }
    }
}

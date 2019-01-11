using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
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
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, null, Color.White, 0, textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        //Angle is in degrees
        public void Render(ITexture texture, float x, float y, float angle)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, null, Color.White, angle.ToRadians(), textureOrigin, 1f, SpriteEffects.None, 0f);
        }


        //Angle is in degrees
        public void Render(ITexture texture, float x, float y, float angle, float size, byte red, byte green, byte blue, byte alpha)
        {
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, null, new Color(red, green, blue, alpha), angle.ToRadians(), textureOrigin, size, SpriteEffects.None, 0f);
        }


        public void RenderTextureArea(ITexture texture, Rect area, float x, float y)
        {
            var srcRect = new Rectangle((int)area.X, (int)area.Y, (int)area.Width, (int)area.Height);
            var textureOrigin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            var position = new Vector2(x, y);

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), position, srcRect, Color.Orange, 0, textureOrigin, 1, SpriteEffects.None, 0f);
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
            RenderLine(lineStartX, lineStartY, lineStopX, lineStopY, Color.White);
        }


        public void RenderLine(float startX, float startY, float endX, float endY, byte[] color)
        {
            //If the color param does not have at least 4 items, throw an exception
            if (color == null)
                throw new ArgumentException($"The param '{nameof(color)}' cannot be null");

            if (color.Length < 4)
                throw new ArgumentException($"The param '{nameof(color)}' must have at lest 4 items.");

            var lineColor = new Color(color[0], color[1], color[2], color[3]);

            _spriteBatch.DrawLine(startX, startY, endX, endY, lineColor);
        }

    }
}

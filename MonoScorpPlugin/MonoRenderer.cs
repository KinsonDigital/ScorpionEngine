using SysColor = System.Drawing.Color;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore;
using ScorpionCore.Plugins;
using System;

namespace MonoScorpPlugin
{
    public class MonoRenderer : IRenderer
    {
        private static SpriteBatch _spriteBatch;
        //This will treated as a MonoGame graphics device.
        private static dynamic _graphicsDevice;


        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            _graphicsDevice.Clear(new Color(red, green, blue, alpha));
        }


        public void Render(ITexture texture, float x, float y)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), new Vector2(x, y), Color.White);

            _spriteBatch.End();
        }


        public void Render(IText text, float x, float y)
        {
            _spriteBatch.Begin();

            var color = new Color(text.Color[0], text.Color[1], text.Color[2]);

            _spriteBatch.DrawString(text.GetText<SpriteFont>(), text.Text, new Vector2(x, y), color);

            _spriteBatch.End();
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
    }
}

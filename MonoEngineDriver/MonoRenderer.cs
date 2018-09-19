using SysColor = System.Drawing.Color;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngineDriver
{
    public class MonoRenderer : IRenderer
    {
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;


        public MonoRenderer(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }



        public void Clear(SysColor color)
        {
            _graphicsDevice.Clear(Tools.ToMonoColor(color));
        }


        public void Render(ITexture texture, Vector position)
        {
            _spriteBatch.Begin();

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), Tools.ToVector2(position), Color.White);

            _spriteBatch.End();
        }


        public void Render(IText text, Vector position)
        {
            _spriteBatch.Begin();

            _spriteBatch.DrawString(text.GetText<SpriteFont>(), text.Text, Tools.ToVector2(position), Tools.ToMonoColor(text.Color));

            _spriteBatch.End();
        }
    }
}

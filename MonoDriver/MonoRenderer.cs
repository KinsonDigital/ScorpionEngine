using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDriver
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


        
        public void Render(ITexture texture, Vector position)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(texture.GetTexture<Texture2D>(), Tools.ToVector2(position), Color.White);

            _spriteBatch.End();
        }
    }
}

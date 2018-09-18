using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpTestGame.Mono
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


        public void Render(ITexture texture)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //_spriteBatch.Draw(texture.GetTexture(), new Rectangle(), Color.White);

            _spriteBatch.End();
        }
    }
}

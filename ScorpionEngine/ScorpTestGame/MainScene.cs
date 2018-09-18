using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.Content;
using ScorpionEngine.Scene;
using ScorpTestGame.Mono;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpTestGame
{
    public class MainScene : GameScene
    {
        private ITexture _rectangle;


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void LoadContent(IContentLoader contentLoader)
        {
            _rectangle = contentLoader.LoadTexture("Rectangle");

            base.LoadContent(contentLoader);
        }


        public override void Render(IRenderer renderer)
        {
            renderer.Render(_rectangle);

            base.Render(renderer);
        }
    }
}

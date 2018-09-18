using Microsoft.Xna.Framework.Graphics;
using MonoDriver;
using ScorpionEngine.Content;
using ScorpionEngine.Scene;
using System;
using System.Collections.Generic;
using System.IO;
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
            base.Render(renderer);
        }
    }
}

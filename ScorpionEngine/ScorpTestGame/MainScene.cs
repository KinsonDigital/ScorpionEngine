using MonoDriver;
using ScorpionEngine;
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
        private MonoTexture _rectangle;


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void LoadContent(IContentLoader contentLoader)
        {
            _rectangle = contentLoader.LoadTexture<MonoTexture>("Rectangle");

            base.LoadContent(contentLoader);
        }


        public override void Render(IRenderer renderer)
        {
            renderer.Render(_rectangle, new Vector(100, 100));

            base.Render(renderer);
        }
    }
}

using MonoDriver;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpTestGame.Scenes
{
    public class MainScene : GameScene
    {
        public MonoText _helloWorld;
        public MonoTexture _orangeBox;
        public MonoTexture _grayBox;


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void LoadContent(IContentLoader contentLoader)
        {
            _helloWorld = contentLoader.LoadText<MonoText>("stats", "hello world");
            _orangeBox = contentLoader.LoadTexture<MonoTexture>("Rectangle");
            _grayBox = contentLoader.LoadTexture<MonoTexture>("GrayRectangle");


            base.LoadContent(contentLoader);
        }


        public override void Render(IRenderer renderer)
        {
            renderer.Clear(Color.CornflowerBlue);

            renderer.Render(_helloWorld, new Vector(10, 10));
            renderer.Render(_orangeBox, new Vector(100, 100));
            renderer.Render(_grayBox, new Vector(100, 300));

            base.Render(renderer);
        }
    }
}

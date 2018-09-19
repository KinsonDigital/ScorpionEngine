using MonoEngineDriver;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using ScorpionEngine.Objects;
using ScorpionEngine.Scene;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysicsDriver;

namespace ScorpTestGame.Scenes
{
    public class MainScene : GameScene
    {
        public VelcroWorld _world;
        public MonoText _helloWorld;
        public MonoTexture _orangeRectTexture;
        public GameObject _orangeObject;


        public override void Initialize()
        {
            _world = new VelcroWorld(new Vector(0, 5));

            _orangeObject = new GameObject();

            var vertices = new Vector[]
            {
                new Vector(-50, -25),
                new Vector(50, -25),
                new Vector(50, 50),
                new Vector(-50, 25)
            };

            _world.AddBody(_orangeObject.PhysicsBody, vertices);

            base.Initialize();
        }


        public override void LoadContent(IContentLoader contentLoader)
        {
            _helloWorld = contentLoader.LoadText<MonoText>("stats", "hello world");
            _orangeObject.Texture = contentLoader.LoadTexture<MonoTexture>("Rectangle");


            base.LoadContent(contentLoader);
        }


        public override void Update(IEngineTiming gameTime)
        {
            _world.Update((float)gameTime.ElapsedEngineTime.TotalSeconds);

            base.Update(gameTime);
        }


        public override void Render(IRenderer renderer)
        {
            renderer.Clear(Color.CornflowerBlue);

            renderer.Render(_orangeObject.Texture, new Vector(200, 200));
            base.Render(renderer);
        }
    }
}

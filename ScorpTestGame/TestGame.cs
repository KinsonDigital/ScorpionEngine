using System;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Graphics;
using ScorpionEngine.Scene;

namespace ScorpTestGame
{
    /// <summary>
    /// The engine of the game.
    /// </summary>
    public class TestGame : Engine
    {
        private SceneManager _sceneManager;
        private Texture _texture;


        /// <summary>
        /// Creates a new space shooter game engine.
        /// </summary>
        public TestGame()
        {
            //Do not set the world in here.  The world has to be set in the OnInit() method so that way
            //the graphics device has been created.  The graphics device will not be created until the
            //engine has started up.
        }


        public override void Init()
        {
            base.Init();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _texture = contentLoader.LoadTexture<Texture>("GrayRectangle");

            base.LoadContent(contentLoader);
        }


        public override void Update(IEngineTiming engineTime)
        {
            base.Update(engineTime);
        }


        public override void Render(IRenderer renderer)
        {
            renderer.Clear(100, 149, 237, 255);

            renderer.Render(_texture, 100, 100);

            base.Render(renderer);
        }

        //TODO: Another override should go here for unload content
        //This would come from Engine and then from there from IEngineCore
    }
}
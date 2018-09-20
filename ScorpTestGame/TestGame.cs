using System;
using MonoScorpPlugin;
using ScorpionCore;
using ScorpionEngine;
using ScorpionEngine.Config;
using ScorpionEngine.Content;
using ScorpionEngine.Scene;
using ScorpTestGame.Scenes;

namespace ScorpTestGame
{
    /// <summary>
    /// The engine of the game.
    /// </summary>
    public class TestGame : Engine
    {
        private SceneManager _sceneManager;


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


        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Render(IRenderer renderer)
        {
            renderer.Clear(100, 149, 237, 255);

            base.Render(renderer);
        }

        //TODO: Override update method

        //TODO: Override render method

        //TODO: Another override should go here for unload content
        //This would come from Engine and then from there from IEngineCore
    }
}
using System;
using MonoDriver;
using ScorpionEngine;
using ScorpionEngine.Content;
using ScorpionEngine.Core;
using ScorpionEngine.Scene;
using ScorpTestGame.Scenes;

namespace ScorpTestGame
{
    /// <summary>
    /// The engine of the game.
    /// </summary>
    public class TestGame : Engine
    {
        private IEngineCore _engineCore;
        private IScene _mainScene;
        private IContentLoader _contentLoader;
        private SceneManager _sceneManager;


        /// <summary>
        /// Creates a new space shooter game engine.
        /// </summary>
        public TestGame()
        {
            _contentLoader = new MonoContentLoader();
            var keyboard = new MonoKeyboard();

            _sceneManager = new SceneManager(_contentLoader, keyboard);

            _mainScene = new MainScene();
            _sceneManager.AddScene(_mainScene);

            _engineCore = new MonoEngineCore(_mainScene);

            SetEngineCore(_engineCore, _contentLoader);

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


        public override void Update(IEngineTiming engineTime)
        {
            base.Update(engineTime);
        }


        public override void Render()
        {
            base.Render();
        }


        //TODO: Another override should go here for unload content
        //This would come from Engine and then from there from IEngineCore
    }
}
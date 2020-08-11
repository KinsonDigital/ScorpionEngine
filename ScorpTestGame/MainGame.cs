// <copyright file="MainGame.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame
{
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Raptor;
    using Raptor.Content;
    using ScorpTestGame.Scenes;

    /// <summary>
    /// The main game.
    /// </summary>
    public class MainGame : Engine
    {
        private Level1 level1;
        private ParticleTestingScene particleScene;

        /// <summary>
        /// Creates a new space shooter game engine.
        /// </summary>
        public MainGame()
        {
            //Do not set the world in here.  The world has to be set in the OnInit() method so that way
            //the graphics device has been created.  The graphics device will not be created until the
            //engine has started up.
        }

        public override void Init()
        {
            this.level1 = new Level1();
            this.particleScene = new ParticleTestingScene();

            SceneManager.Add(this.level1);
            SceneManager.Add(this.particleScene);

            SceneManager.SetCurrentScene(0);

            base.Init();
        }

        public override void LoadContent(ContentLoader contentLoader)
        {
            base.LoadContent(contentLoader);
        }

        public override void Update(EngineTime engineTime)
        {
            base.Update(engineTime);
        }

        public override void Render(GameRenderer renderer)
        {
            base.Render(renderer);
        }
    }
}
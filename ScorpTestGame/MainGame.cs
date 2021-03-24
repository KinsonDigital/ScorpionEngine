// <copyright file="MainGame.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame
{
    using KDScorpionEngine;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Raptor.Content;
    using ScorpTestGame.Scenes;

    /// <summary>
    /// The main game.
    /// </summary>
    public class MainGame : Engine
    {
        private Level1 level1;

        /// <summary>
        /// Creates a new space shooter game engine.
        /// </summary>
        public MainGame()
            : base(800, 600)
        {
            //Do not set the world in here.  The world has to be set in the OnInit() method so that way
            //the graphics device has been created.  The graphics device will not be created until the
            //engine has started up.

            WindowWidth = base.WindowWidth;
            WindowHeight = base.WindowHeight;
        }

        public static int WindowWidth { get; private set; }

        public static int WindowHeight { get; private set; }

        public override void Init()
        {
            this.level1 = new Level1();

            SceneManager.Add(this.level1);
            SceneManager.SetCurrentScene(0);

            base.Init();
        }

        public override void LoadContent(IContentLoader contentLoader) => base.LoadContent(contentLoader);

        public override void Update(GameTime gameTime) => base.Update(gameTime);

        public override void Render(IRenderer renderer) => base.Render(renderer);
    }
}

// <copyright file="Level1.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame.Scenes
{
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Factories;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using KDScorpTestGame;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    /// <summary>
    /// Level 1 scene.
    /// </summary>
    public class Level1 : GameScene
    {
        private Entity sub;
        private Entity fish;
        private IKeyboard keyboard;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Level1"/> class.
        /// </summary>
        public Level1()
            : base(new Vector2(0f, 0f))
        {
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        public override void Initialize()
        {
            this.sub = new Sub();
            this.sub.Init();

            this.fish = EntityFactory.CreateNonAnimatedFromTextureAtlas("Main-Atlas", "fish");
            this.fish.Position = new Vector2(200, 550);
            this.fish.Init();

            base.Initialize();
        }

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <param name="contentLoader">Loads the content items.</param>
        public override void LoadContent(IContentLoader contentLoader)
        {
            this.sub.LoadContent(contentLoader);
            this.fish.LoadContent(contentLoader);
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="gameTime">Updates the scene objects.</param>
        public override void Update(GameTime gameTime)
        {
            this.sub.Update(gameTime);
            this.fish.Update(gameTime);
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        /// <param name="renderer">Renders the graphics in the scene.</param>
        public override void Render(Renderer renderer)
        {
            //renderer.Render(this.sub);
            renderer.Render(this.fish);

            base.Render(renderer);
        }

        /// <summary>
        /// Unloads the content from the scene.
        /// </summary>
        /// <param name="contentLoader">Used to unload content.</param>
        public override void UnloadContent(IContentLoader contentLoader)
        {
            // TODO: NEed to add unloading of content here
        }
    }
}

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
    using Raptor.Content;
    using Raptor.Graphics;

    /// <summary>
    /// Level 1 scene.
    /// </summary>
    public class Level1 : GameScene
    {
        private Entity sub;

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
            var factory = EntityFactory.CreateFactory();

            this.sub = factory.CreateNonAnimated("Sub-Still");

            //sub = new Entity()
            //{
            //    Position = new Vector2(200, 200),
            //};

            base.Initialize();
        }

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <param name="contentLoader">Loads the content items.</param>
        public override void LoadContent(IContentLoader contentLoader)
        {
            //sub.Texture = contentLoader.Load<ITexture>("Sub-Still");
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="gameTime">Updates the scene objects.</param>
        public override void Update(GameTime gameTime)
        {
            sub.Update(gameTime);
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        /// <param name="renderer">Renders the graphics in the scene.</param>
        public override void Render(Renderer renderer)
        {
            renderer.Render(sub);

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

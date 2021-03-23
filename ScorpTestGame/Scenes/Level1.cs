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
    using KDScorpionEngine.Input;
    using KDScorpionEngine.Scene;
    using KDScorpTestGame;
    using Raptor.Audio;
    using Raptor.Content;
    using Raptor.Input;

    /// <summary>
    /// Level 1 scene.
    /// </summary>
    public class Level1 : GameScene
    {
        private IEntity sub;
        private IEntity fish;
        private EntityPool<Bubble> bubblePool;
        private IEntity enemySub;
        private int bubbleSpawnElapsed;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private EntityFactory entityFactory;
        private KeyboardWatcher keyboardWatcher;
        private ISound gameMusic;

        /// <summary>
        /// Initializes a new instance of the <see cref="Level1"/> class.
        /// </summary>
        public Level1()
        {
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        public override void Initialize()
        {
            this.entityFactory = new EntityFactory();
            this.keyboardWatcher = InputFactory.CreateKeyboardWatcher();
            this.keyboardWatcher.HitCountMax = 2;
            this.keyboardWatcher.Input = KeyCode.F;
            this.keyboardWatcher.DownTimeOut = 5000;
            this.keyboardWatcher.InputDownTimedOut += KeyboardWatcher_InputDownTimedOut;
            this.keyboardWatcher.InputHitCountReached += KeyboardWatcher_InputHitCountReached;

            this.bubblePool = new EntityPool<Bubble>
            {
                MaxPoolSize = 1000,
            };

            this.sub = this.entityFactory.CreateAnimated<Sub>("Main-Atlas", "sub");
            this.sub.Position = new Vector2(400, 400);

            this.fish = this.entityFactory.CreateNonAnimatedFromTextureAtlas<Entity>("Main-Atlas", "fish");
            this.fish.Position = new Vector2(200, 550);

            this.enemySub = this.entityFactory.CreateNonAnimatedFromTexture<Entity>("Sub-Still");
            this.enemySub.Position = new Vector2(500, 400);

            AddEntity(this.sub);
            AddEntity(this.fish);
            AddEntity(this.enemySub);

            base.Initialize();
        }

        private void KeyboardWatcher_InputDownTimedOut(object sender, System.EventArgs e) => this.sub.Position = new Vector2(this.sub.Position.X, this.sub.Position.Y - 20);

        private void KeyboardWatcher_InputHitCountReached(object sender, System.EventArgs e) => this.sub.Position = new Vector2(this.sub.Position.X + 10, this.sub.Position.Y);

        /// <summary>
        /// Loads content.
        /// </summary>
        /// <param name="contentLoader">Loads the content items.</param>
        public override void LoadContent(IContentLoader contentLoader)
        {
            this.gameMusic = contentLoader.Load<ISound>(@"Music\SubDiver");

            this.gameMusic.PlaySound();
            base.LoadContent(contentLoader);
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="gameTime">Updates the scene objects.</param>
        public override void Update(GameTime gameTime)
        {
            this.keyboardWatcher.Update(gameTime);
            this.bubblePool.Update(gameTime);

            this.bubbleSpawnElapsed += gameTime.CurrentFrameElapsed;

            if (this.bubbleSpawnElapsed >= 125)
            {
                this.bubblePool.GenerateNonAnimatedFromTextureAtlas("Main-Atlas", "bubble");

                this.bubbleSpawnElapsed = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Renders the scene.
        /// </summary>
        /// <param name="renderer">Renders the graphics in the scene.</param>
        public override void Render(IRenderer renderer)
        {
            this.bubblePool.Render(renderer);

            base.Render(renderer);
        }

        /// <summary>
        /// Unloads the content from the scene.
        /// </summary>
        /// <param name="contentLoader">Used to unload content.</param>
        public override void UnloadContent(IContentLoader contentLoader)
        {
            // TODO: Need to add unloading of content here
        }
    }
}

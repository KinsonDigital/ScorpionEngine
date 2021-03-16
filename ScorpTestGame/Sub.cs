// <copyright file="Sub.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpTestGame
{
    using System;
    using System.Linq;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Factories;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Input;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    public class Sub : Entity
    {
        private KeyboardWatcher launchMissleKeyWatcher;
        private KeyboardWatcher moveLeftKeyWatcher;
        private KeyboardWatcher moveRightKeyWatcher;
        private int speed = 100;
        private EntityPool<Missile> missiles;
        private float secondsElapsed;
        private KeyboardWatcher moveUpKeyWatcher;
        private KeyboardWatcher moveDownKeyWatcher;

        public Sub()
            : base("sub")
        {
        }

        public override void Init()
        {
            SectionToRender.Animator = new Animator();
            Position = new Vector2(200, 400);

            SetupInput();

            this.missiles = new EntityPool<Missile>();
        }

        private void SetupInput()
        {
            this.launchMissleKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.launchMissleKeyWatcher.Input = KeyCode.Space;
            this.launchMissleKeyWatcher.DownTimeOut = 1000;
            this.launchMissleKeyWatcher.InputDownTimedOut += KeyWatcher_InputDownTimedOut;

            this.moveLeftKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.moveLeftKeyWatcher.Input = KeyCode.Left;
            this.moveLeftKeyWatcher.InputDown += MoveLeftKeyWatcher_InputDown;

            this.moveRightKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.moveRightKeyWatcher.Input = KeyCode.Right;
            this.moveRightKeyWatcher.InputDown += MoveRightKeyWatcher_InputDown;

            this.moveUpKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.moveUpKeyWatcher.Input = KeyCode.Up;
            this.moveUpKeyWatcher.InputDown += MoveUpKeyWatcher_InputDown;

            this.moveDownKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.moveDownKeyWatcher.Input = KeyCode.Down;
            this.moveDownKeyWatcher.InputDown += MoveDownKeyWatcher_InputDown;
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            AtlasData = contentLoader.Load<IAtlasData>("Main-Atlas");
            SectionToRender.Animator.Frames = AtlasData.GetFrames("sub")
                .Select(f => f.Bounds).ToArray().ToReadOnlyCollection();
        }

        public override void UnloadContent(IContentLoader contentLoader)
        {
            this.launchMissleKeyWatcher.InputDownTimedOut -= KeyWatcher_InputDownTimedOut;
            this.moveLeftKeyWatcher.InputDown -= MoveLeftKeyWatcher_InputDown;
            this.moveRightKeyWatcher.InputDown -= MoveRightKeyWatcher_InputDown;
            this.moveUpKeyWatcher.InputDown -= MoveUpKeyWatcher_InputDown;
            this.moveDownKeyWatcher.InputDown -= MoveDownKeyWatcher_InputDown;

            base.UnloadContent(contentLoader);
        }

        public override void Update(GameTime gameTime)
        {
            this.launchMissleKeyWatcher.Update(gameTime);
            this.missiles.Update(gameTime);

            this.secondsElapsed = gameTime.CurrentFrameElapsed / 1000f;

            this.moveLeftKeyWatcher.Update(gameTime);
            this.moveRightKeyWatcher.Update(gameTime);
            this.moveUpKeyWatcher.Update(gameTime);
            this.moveDownKeyWatcher.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Render(IRenderer renderer)
        {
            this.missiles.Render(renderer);
            base.Render(renderer);
        }

        private void MoveLeftKeyWatcher_InputDown(object sender, EventArgs e)
        {
            Position = new Vector2(Position.X - (this.speed * this.secondsElapsed), Position.Y);
            FlippedHorizontally = true;
        }

        private void MoveRightKeyWatcher_InputDown(object sender, EventArgs e)
        {
            Position = new Vector2(Position.X + (this.speed * this.secondsElapsed), Position.Y);
            FlippedHorizontally = false;
        }

        private void MoveUpKeyWatcher_InputDown(object sender, EventArgs e)
        {
            Position = new Vector2(Position.X, Position.Y - (this.speed * this.secondsElapsed));
        }

        private void MoveDownKeyWatcher_InputDown(object sender, EventArgs e)
        {
            Position = new Vector2(Position.X, Position.Y + (this.speed * this.secondsElapsed));
        }

        private void KeyWatcher_InputDownTimedOut(object sender, EventArgs e)
            => this.missiles.GenerateNonAnimatedFromTextureAtlas("Main-Atlas", "missile", (entity) =>
            {
                var launchPosition = new Vector2(Position.X + (SectionToRender.RenderBounds.Width / 2),
                    Position.Y + (SectionToRender.RenderBounds.Height / 4));
                entity.Launch(launchPosition, HorizontalDirection.Left);
            });
    }
}

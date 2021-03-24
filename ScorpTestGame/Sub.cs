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
        private readonly Keyboard keyboard;
        private KeyboardWatcher launchMissleKeyWatcher;
        private EntityPool<Missile> missiles;
        private readonly int speed = 100;
        private readonly float secondsElapsed;
        private KeyboardState keyboardState;

        public Sub()
            : base("sub")
        {
            this.keyboard = new Keyboard();
        }

        public override void Init()
        {
            SectionToRender.Animator = new Animator();
            Position = new Vector2(200, 400);

            // Setup the missle launch key
            this.launchMissleKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.launchMissleKeyWatcher.Input = KeyCode.Space;
            this.launchMissleKeyWatcher.DownTimeOut = 1000;
            this.launchMissleKeyWatcher.InputDownTimedOut += KeyWatcher_InputDownTimedOut;

            SetupMovementBehaviors();

            this.missiles = new EntityPool<Missile>();
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

            base.UnloadContent(contentLoader);
        }

        public override void Update(GameTime gameTime)
        {
            this.launchMissleKeyWatcher.Update(gameTime);
            this.missiles.Update(gameTime);

            this.keyboardState = this.keyboard.GetState();

            if (this.keyboardState.IsKeyDown(KeyCode.Left))
            {
                FlippedHorizontally = true;
            }

            if (this.keyboardState.IsKeyDown(KeyCode.Right))
            {
                FlippedHorizontally = false;
            }

            base.Update(gameTime);
        }

        public override void Render(IRenderer renderer)
        {
            foreach (var missile in this.missiles.Entitities)
            {
                renderer.Render(
                    missile,
                    missile.Position.X,
                    missile.Position.Y,
                    flippedHorizontally: FlippedHorizontally,
                    flippedVertically: FlippedVertically);
            }

            base.Render(renderer);
        }

        private void SetupMovementBehaviors()
        {
            Behaviors.Add(BehaviorFactory.CreateKeyboardMovement(
                this,
                KeyCode.Left,
                (gameTime, currentPosition) =>
                {
                    var seconds = gameTime.CurrentFrameElapsed / 1000f;

                    return new Vector2(currentPosition.X - (100 * seconds), currentPosition.Y);
                }));

            Behaviors.Add(BehaviorFactory.CreateKeyboardMovement(
                this,
                KeyCode.Right,
                (gameTime, currentPosition) =>
                {
                    var seconds = gameTime.CurrentFrameElapsed / 1000f;

                    return new Vector2(currentPosition.X + (100 * seconds), currentPosition.Y);
                }));

            Behaviors.Add(BehaviorFactory.CreateKeyboardMovement(
                this,
                KeyCode.Up,
                (gameTime, currentPosition) =>
                {
                    var seconds = gameTime.CurrentFrameElapsed / 1000f;

                    return new Vector2(currentPosition.X, currentPosition.Y - (100 * seconds));
                }));

            Behaviors.Add(BehaviorFactory.CreateKeyboardMovement(
                this,
                KeyCode.Down,
                (gameTime, currentPosition) =>
                {
                    var seconds = gameTime.CurrentFrameElapsed / 1000f;

                    return new Vector2(currentPosition.X, currentPosition.Y + (100 * seconds));
                }));
        }

        private void KeyWatcher_InputDownTimedOut(object sender, EventArgs e)
            => this.missiles.GenerateNonAnimatedFromTextureAtlas("Main-Atlas", "missile", (missile) =>
            {
                var launchPosition = new Vector2(
                    Position.X + (SectionToRender.RenderBounds.Width / 2),
                    Position.Y + (SectionToRender.RenderBounds.Height / 4));

                missile.TravelDirection = FlippedHorizontally
                    ? HorizontalDirection.Left
                    : HorizontalDirection.Right;

                missile.Launch(launchPosition);
            });
    }
}

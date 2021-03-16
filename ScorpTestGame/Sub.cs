// <copyright file="Sub.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpTestGame
{
    using System.Linq;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;
    using Raptor.Graphics;
    using Raptor.Input;

    public class Sub : Entity
    {
        public IGameInput<KeyCode, KeyboardState> keyboard;
        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;
        private int speed = 200;

        public Sub()
            : base("sub")
        {
        }

        public override void Init()
        {
            SectionToRender.Animator = new Animator();
            Position = new Vector2(400, 400);
            this.keyboard = new Keyboard();
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            AtlasData = contentLoader.Load<IAtlasData>("Main-Atlas");
            SectionToRender.Animator.Frames = AtlasData.GetFrames("sub")
                .Select(f => f.Bounds).ToArray().ToReadOnlyCollection();
        }

        public override void Update(GameTime gameTime)
        {
            this.currentKeyState = this.keyboard.GetState();

            if (this.currentKeyState.IsKeyUp(KeyCode.Space) && this.previousKeyState.IsKeyDown(KeyCode.Space))
            {
                switch (RenderSection.Animator.CurrentState)
                {
                    case AnimateState.Playing:
                        RenderSection.Animator.CurrentState = AnimateState.Paused;
                        break;
                    case AnimateState.Paused:
                        RenderSection.Animator.CurrentState = AnimateState.Playing;
                        break;
                }
            }

            var secondsElapsed = gameTime.CurrentFrameElapsed / 1000f;

            if (IsLeftDown())
            {
                Position = new Vector2(Position.X - (this.speed * secondsElapsed), Position.Y);
            }

            if (IsRightDown())
            {
                Position = new Vector2(Position.X + (this.speed * secondsElapsed), Position.Y);
            }

            if (IsUpDown())
            {
                Position = new Vector2(Position.X, Position.Y - (this.speed * secondsElapsed));
            }

            if (IsDownDown())
            {
                Position = new Vector2(Position.X, Position.Y + (this.speed * secondsElapsed));
            }

            this.previousKeyState = this.currentKeyState;
            base.Update(gameTime);
        }

        private bool IsLeftDown() => this.currentKeyState.IsKeyDown(KeyCode.Left);

        private bool IsRightDown() => this.currentKeyState.IsKeyDown(KeyCode.Right);

        private bool IsUpDown() => this.currentKeyState.IsKeyDown(KeyCode.Up);

        private bool IsDownDown() => this.currentKeyState.IsKeyDown(KeyCode.Down);
    }
}

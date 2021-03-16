// <copyright file="Missile.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpTestGame
{
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Factories;
    using KDScorpionEngine.Input;
    using Raptor.Content;
    using Raptor.Input;
    using ScorpTestGame;

    public class Missile : Entity
    {
        private int speed = 0;
        private int speedIncreaseInterval = 125;
        private int speedIncreaseElapsed;
        private HorizontalDirection travelDirection;
        private KeyboardWatcher leftKeyWatcher;
        private KeyboardWatcher rightKeyWatcher;

        public Missile()
            : base("Main-Atlas", "missile")
        {
        }

        public bool Alive { get; set; }

        public override void Init()
        {
            this.leftKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.leftKeyWatcher.Input = KeyCode.Left;
            this.leftKeyWatcher.InputDown += LeftKeyWatcher_InputDown;

            this.rightKeyWatcher = InputFactory.CreateKeyboardWatcher();
            this.rightKeyWatcher.Input = KeyCode.Right;
            this.rightKeyWatcher.InputDown += RightKeyWatcher_InputDown;

            base.Init();
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            base.LoadContent(contentLoader);
        }

        public void Launch(Vector2 launchPosition, HorizontalDirection direction)
        {
            Position = launchPosition;
            travelDirection = direction;

            Alive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                this.speedIncreaseElapsed += gameTime.CurrentFrameElapsed;

                if (this.speedIncreaseElapsed >= this.speedIncreaseInterval)
                {
                    this.speedIncreaseElapsed = 0;

                    if (this.speed < 300)
                    {
                        this.speed += 25;
                    }
                }

                var secondsElapsed = gameTime.CurrentFrameElapsed / 1000f;

                var moveAmount = 0f;

                switch (this.travelDirection)
                {
                    case HorizontalDirection.Left:
                        moveAmount = this.speed * secondsElapsed * -1;
                        break;
                    case HorizontalDirection.Right:
                        moveAmount = this.speed * secondsElapsed;
                        break;
                }

                Position = Position.AddX(moveAmount);
            }

            // If off the right side of the screen
            if (Position.X - SectionToRender.HalfWidth > MainGame.WindowWidth)
            {
                Alive = false;
            }

            // If off the left side of the screen
            if (Position.X + SectionToRender.HalfWidth < 0)
            {
                Alive = false;
            }

            base.Update(gameTime);
        }

        private void LeftKeyWatcher_InputDown(object sender, System.EventArgs e)
        {
            travelDirection = HorizontalDirection.Left;
        }

        private void RightKeyWatcher_InputDown(object sender, System.EventArgs e)
        {
            travelDirection = HorizontalDirection.Right;
        }
    }
}

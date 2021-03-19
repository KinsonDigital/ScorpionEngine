// <copyright file="Missile.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpTestGame
{
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using Raptor.Content;
    using ScorpTestGame;

    public class Missile : Entity
    {
        private int speed = 0;
        private readonly int speedIncreaseInterval = 125;
        private int speedIncreaseElapsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Missile"/> class.
        /// </summary>
        public Missile()
            : base("Main-Atlas", "missile")
        {
        }

        public bool Alive { get; set; }

        public HorizontalDirection TravelDirection { get; set; } = HorizontalDirection.Right;

        public override void Init()
        {
            base.Init();
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            base.LoadContent(contentLoader);
        }

        public void Launch(Vector2 launchPosition)
        {
            Position = launchPosition;

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

                switch (TravelDirection)
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
    }
}

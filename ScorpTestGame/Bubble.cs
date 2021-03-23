// <copyright file="Bubble.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpTestGame
{
    using System;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using Raptor.Content;
    using ScorpTestGame;

    public class Bubble : Entity
    {
        private static int halfHeight;

        public Bubble() : base("Main-Atlas", nameof(Bubble).ToLower())
        {
        }

        public override void Init()
        {
            var random = new Random();

            Position = new Vector2(
                random.Next(SectionToRender.HalfWidth, MainGame.WindowWidth - SectionToRender.HalfWidth),
                MainGame.WindowHeight + halfHeight);

            base.Init();
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            base.LoadContent(contentLoader);

            if (halfHeight <= 0)
            {
                halfHeight = SectionToRender.HalfHeight;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Position = new Vector2(Position.X, Position.Y - (30 * (gameTime.CurrentFrameElapsed / 1000f)));

            if (Position.Y <= SectionToRender.HalfHeight * -1)
            {
                Visible = false;
                Enabled = false;
            }

            base.Update(gameTime);
        }
    }
}

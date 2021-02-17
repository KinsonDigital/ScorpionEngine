// <copyright file="Bubble.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpTestGame
{
    using System;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;
    using ScorpTestGame;

    public class Bubble : Entity
    {
        public Bubble() : base("Main-Atlas", nameof(Bubble).ToLower())
        {
        }

        public override void Init()
        {
            var random = new Random();

            Position = new Vector2(
                random.Next(RenderSection.GetTextureHalfWidth(), MainGame.WindowWidth - RenderSection.GetTextureHalfWidth()),
                MainGame.WindowHeight);

            base.Init();
        }

        public override void LoadContent(IContentLoader contentLoader)
        {
            base.LoadContent(contentLoader);
        }

        public override void Update(GameTime gameTime)
        {
            Position = new Vector2(Position.X, Position.Y - (30 * (gameTime.CurrentFrameElapsed / 1000f)));
            base.Update(gameTime);
        }
    }
}

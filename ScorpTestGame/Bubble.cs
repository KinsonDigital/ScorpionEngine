using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using KDScorpionEngine;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Graphics;
using Raptor.Content;
using Raptor.Factories;
using Raptor.Graphics;
using ScorpTestGame;

namespace KDScorpTestGame
{
    public class Bubble : Entity
    {
        public Bubble() : base("Main-Atlas", nameof(Bubble).ToLower())
        {
        }

        public override void Init()
        {
            var random = new Random();

            Position = new Vector2(random.Next(0, MainGame.WindowWidth), MainGame.WindowHeight);

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

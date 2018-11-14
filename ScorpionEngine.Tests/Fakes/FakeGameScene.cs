﻿using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;
using ScorpionEngine.Scene;

namespace ScorpionEngine.Tests.Fakes
{
    public class FakeGameScene : GameScene
    {
        public FakeGameScene(Vector gravity) : base(gravity)
        {
        }


        public override void Render(Renderer renderer)
        {
            IsRenderingScene = true;

            base.Render(renderer);
        }
    }
}

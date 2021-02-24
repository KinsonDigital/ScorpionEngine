// <copyright file="FakeGameScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Fakes
{
    using System.Numerics;
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;

    /// <summary>
    /// Provides a fake implementation of the <see cref="GameScene"/> abstract class.
    /// </summary>
    public class FakeGameScene : GameScene
    {
        public FakeGameScene()
            : base(Vector2.Zero)
        {
        }

        public override void Render(IRenderer renderer)
        {
            IsRenderingScene = true;

            base.Render(renderer);
        }
    }
}

// <copyright file="FakeGameScene.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Fakes
{
    using KDScorpionEngine.Graphics;
    using KDScorpionEngine.Scene;
    using Raptor.Plugins;

    /// <summary>
    /// Provides a fake implementation of the <see cref="GameScene"/> abstract class.
    /// </summary>
    public class FakeGameScene : GameScene
    {
        public FakeGameScene(IPhysicsWorld physicsWorld)
            : base(physicsWorld)
        {
        }

        public override void Render(GameRenderer renderer)
        {
            IsRenderingScene = true;

            base.Render(renderer);
        }
    }
}

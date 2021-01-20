// <copyright file="FakeEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Fakes
{
    using KDScorpionEngine;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;

    /// <summary>
    /// Provides a fake implementation of the <see cref="KDScorpionEngine.Entities.Entity"/> abstract class.
    /// </summary>
    public class FakeEntity : Entity
    {
        public FakeEntity()
        {
        }

        public bool UpdateInvoked { get; set; }

        public bool RenderInvoked { get; set; }

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public override void Update(GameTime gameTime)
        {
            UpdateInvoked = true;

            base.Update(gameTime);
        }

        public override void Render(Renderer renderer) => RenderInvoked = true;

        public override string ToString() => base.ToString();
    }
}

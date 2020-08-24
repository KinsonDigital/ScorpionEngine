// <copyright file="FakeEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Fakes
{
    using System.Numerics;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor;
    using Raptor.Graphics;
    using Raptor.Plugins;

    /// <summary>
    /// Provides a fake implementation of the <see cref="KDScorpionEngine.Entities.Entity"/> abstract class.
    /// </summary>
    public class FakeEntity : Entity
    {
        public FakeEntity(bool isStaticBody)
            : base(isStaticBody: isStaticBody)
        {
        }

        public FakeEntity(Vector2 position)
            : base(position)
        {
        }

        public FakeEntity(IPhysicsBody body)
            : base(body)
        {
        }

        public FakeEntity(Vector2[] polyVertices, Vector2 position)
            : base(polyVertices, position, isStaticBody: false)
        {
        }

        public FakeEntity(Texture texture, Vector2[] polyVertices, Vector2 position)
            : base(texture, polyVertices, position, isStaticBody: false)
        {
        }

        public bool UpdateInvoked { get; set; }

        public bool RenderInvoked { get; set; }

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public override void Update(EngineTime engineTime)
        {
            UpdateInvoked = true;

            base.Update(engineTime);
        }

        public override void Render(GameRenderer renderer) => RenderInvoked = true;

        public override string ToString() => base.ToString();
    }
}

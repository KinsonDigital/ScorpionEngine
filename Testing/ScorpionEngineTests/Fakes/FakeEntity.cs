using KDScorpionEngine.Entities;
using KDScorpionEngine.Graphics;
using Raptor;
using Raptor.Graphics;
using Raptor.Plugins;
using System.Numerics;

namespace KDScorpionEngineTests.Fakes
{
    /// <summary>
    /// Provides a fake implementation of the <see cref="KDScorpionEngine.Entities.Entity"/> abstract class.
    /// </summary>
    public class FakeEntity : Entity
    {
        #region Constructors
        public FakeEntity(bool isStaticBody) : base(isStaticBody: isStaticBody)
        {
        }


        public FakeEntity(Vector2 position) : base(position)
        {
        }


        public FakeEntity(IPhysicsBody body) : base(body)
        {
        }


        public FakeEntity(Vector2[] polyVertices, Vector2 position) : base(polyVertices, position, isStaticBody: false)
        {
        }


        public FakeEntity(Texture texture, Vector2[] polyVertices, Vector2 position) : base(texture, polyVertices, position, isStaticBody: false)
        {
        }
        #endregion


        #region Props
        public bool UpdateInvoked { get; set; }

        public bool RenderInvoked { get; set; }
        #endregion


        #region Public Methods
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public override void Update(EngineTime engineTime)
        {
            UpdateInvoked = true;

            base.Update(engineTime);
        }


        public override void Render(GameRenderer renderer) => RenderInvoked = true;


        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
    }
}

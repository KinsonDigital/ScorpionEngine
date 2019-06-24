using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Graphics;

namespace KDScorpionEngineTests.Fakes
{
    public class FakeEntity : Entity
    {
        #region Constructors
        public FakeEntity(bool isStaticBody) : base(isStaticBody: isStaticBody)
        {
        }


        public FakeEntity(Vector position) : base(position)
        {
        }


        public FakeEntity(IPhysicsBody body) : base(body)
        {
        }


        public FakeEntity(Vector[] polyVertices, Vector position) : base(polyVertices, position, isStaticBody: false)
        {
        }


        public FakeEntity(Texture texture, Vector[] polyVertices, Vector position) : base(texture, polyVertices, position, isStaticBody: false)
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

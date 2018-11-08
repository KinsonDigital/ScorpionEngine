using ScorpionEngine.Entities;
using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Tests.Fakes
{
    public class FakeEntity : Entity
    {
        public FakeEntity(Texture texture, Vector position, bool isStaticBody = false) : base(texture, position, isStaticBody)
        {
        }

        public FakeEntity(Vector[] polyVertices, Vector position, bool isStaticBody = false) : base(polyVertices, position, isStaticBody)
        {
        }


        public FakeEntity(Texture texture, Vector[] polyVertices, Vector position, bool isStaticBody = false) : base(texture, polyVertices, position, isStaticBody)
        {
        }

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
            base.Update(engineTime);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

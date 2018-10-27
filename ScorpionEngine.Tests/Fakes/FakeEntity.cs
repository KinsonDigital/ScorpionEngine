using ScorpionEngine.Objects;
using ScorpionEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Tests.Fakes
{
    public class FakeEntity : Entity
    {
        public FakeEntity(Vector position, bool isStaticBody = false) : base(position, isStaticBody)
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

        public override void OnUpdate(EngineTime engineTime)
        {
            base.OnUpdate(engineTime);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScorpionCore;
using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Entities
{
    public class StaticEntity : Entity
    {
        public StaticEntity(Vector position) : base(position, true)
        {
            _usesPhysics = false;
        }


        public StaticEntity(Texture texture, Vector position) : base(texture, position, true)
        {
            _usesPhysics = false;
        }


        public override void OnUpdate(EngineTime engineTime)
        {
            base.OnUpdate(engineTime);
        }
    }
}

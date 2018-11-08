﻿using ScorpionEngine.Graphics;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Entities
{
    public class StaticEntity : Entity
    {
        public StaticEntity(Texture texture, Vector position) : base(texture, position, true)
        {
            _usesPhysics = false;
        }


        public override void Update(EngineTime engineTime)
        {
            base.Update(engineTime);
        }
    }
}

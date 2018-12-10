
using KDScorpionCore;
using KDScorpionCore.Graphics;

namespace ScorpionEngine.Entities
{
    public class StaticEntity : Entity
    {
        public StaticEntity(Texture texture, Vector position) : base(texture, position, isStaticBody: true)
        {
            _usesPhysics = false;
        }


        public override void Update(EngineTime engineTime)
        {
            base.Update(engineTime);
        }
    }
}


using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;

namespace KDScorpionEngine.Entities
{
    public class StaticEntity : Entity
    {
        #region Constructors
        internal StaticEntity(IPhysicsBody body) : base(body) { }


        public StaticEntity(Texture texture, Vector position) : base(texture, position, isStaticBody: true) => _usesPhysics = false;
        #endregion


        #region Public Methods
        public override void Update(EngineTime engineTime) => base.Update(engineTime);
        #endregion
    }
}

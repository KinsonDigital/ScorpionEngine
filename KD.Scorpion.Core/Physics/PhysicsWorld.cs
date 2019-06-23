using KDScorpionCore.Plugins;

namespace KDScorpionCore.Physics
{
    public class PhysicsWorld
    {
        #region Private Fields
        private readonly IPhysicsWorld _internalWorld;
        #endregion


        #region Constructors
        internal PhysicsWorld(IPhysicsWorld physicsWorld) => _internalWorld = physicsWorld;


        public PhysicsWorld(Vector gravity) =>
            _internalWorld = CorePluginSystem.Plugins.PhysicsPlugins.LoadPlugin<IPhysicsWorld>(new object[] { gravity.X, gravity.Y });
        #endregion


        #region Props
        public Vector Gravity => new Vector(_internalWorld.GravityX, _internalWorld.GravityY);
        #endregion


        #region Public Methods
        public void AddBody(IPhysicsBody body)
        {
            //Only add it to the physics world if the entity has been initialized.
            _internalWorld.AddBody(body);
        }


        public void Update(float dt) => _internalWorld.Update(dt);
        #endregion
    }
}

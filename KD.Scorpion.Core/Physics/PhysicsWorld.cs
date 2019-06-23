using KDScorpionCore.Plugins;

namespace KDScorpionCore.Physics
{
    public class PhysicsWorld
    {
        private readonly IPhysicsWorld _internalWorld;


        internal PhysicsWorld(Vector gravity, IPhysicsWorld physicsWorld) => _internalWorld = physicsWorld;


        public PhysicsWorld(Vector gravity) =>
            _internalWorld = EnginePluginSystem.Plugins.PhysicsPlugins.LoadPlugin<IPhysicsWorld>(new object[] { gravity.X, gravity.Y });


        public Vector Gravity => new Vector(_internalWorld.GravityX, _internalWorld.GravityY);


        public void AddEntity(IPhysicsBody body)
        {
            //Only add it to the physics world if the entity has been initialized.
            _internalWorld.AddBody(body);
        }


        public void Update(float dt) => _internalWorld.Update(dt);
    }
}

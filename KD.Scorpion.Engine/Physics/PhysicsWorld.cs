using KDScorpionCore;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Exceptions;

namespace KDScorpionEngine.Physics
{
    public class PhysicsWorld
    {
        private IPhysicsWorld _internalWorld;


        public PhysicsWorld(Vector gravity)
        {
            object[] ctrParams = new object[] { gravity.X, gravity.Y };

            _internalWorld = PluginSystem.PhysicsPlugins.LoadPlugin<IPhysicsWorld>(gravity.X, gravity.Y);
        }


        public Vector Gravity => new Vector(_internalWorld.GravityX, _internalWorld.GravityY);


        public void AddEntity(Entity entity)
        {
            //Only add it to the physics world if the entity has been initialized.
            if (entity.IsInitialized)
            {
                _internalWorld.AddBody(entity.Body.InternalPhysicsBody);
                return;
            }

            throw new EntityNotInitializedException();
        }


        public void Update(float dt)
        {
            _internalWorld.Update(dt);
        }
    }
}

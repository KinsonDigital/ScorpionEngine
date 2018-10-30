using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Physics
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
            _internalWorld.AddBody(entity.Body.InternalPhysicsBody);
        }


        public void Update(float dt)
        {
            _internalWorld.Update(dt);
        }
    }
}

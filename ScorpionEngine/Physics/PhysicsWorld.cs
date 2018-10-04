using ScorpionCore;
using ScorpionCore.Plugins;
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

            _internalWorld = Engine.PhysicsPlugins.GetPluginByType<IPhysicsWorld>(ctrParams);
        }


        public IVector Gravity { get; set; }


        public void AddBody(PhysicsBody body)
        {
            _internalWorld.AddBody<IPhysicsBody>(body.Body);
        }


        public void GetBody()
        {
            throw new NotImplementedException();
        }


        public void RemoveBody()
        {
            throw new NotImplementedException();
        }


        public void Update(float dt)
        {
            _internalWorld.Update(dt);
        }
    }
}

using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Plugins
{
    public interface IPhysicsWorld : IPlugin
    {
        float GravityX { get; set; }

        float GravityY { get; set; }

        void Update(float dt);

        void AddBody<T>(T body) where T : IPhysicsBody;

        void RemoveBody();

        void GetBody();
    }
}

using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    public interface IPhysicsWorld
    {
        Vector Gravity { get; set; }

        void Update(float dt);

        void AddBody<T>(T body, Vector[] vertices) where T : IPhysicsBody;

        void RemoveBody();

        void GetBody();
    }
}

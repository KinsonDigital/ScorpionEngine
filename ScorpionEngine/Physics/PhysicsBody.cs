using ScorpionCore;
using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Physics
{
    public class PhysicsBody
    {
        public PhysicsBody(Vector[] vertices, Vector position, float angle = 0, float density = 1, float friction = 0.2f, float restitution = 0, bool isStatic = false)
        {
            object[] ctorParams = new object[9];

            //Setup the vertices
            var verticesParam = new List<InternalVector>();
            foreach (var vector in vertices)
                verticesParam.Add(new InternalVector(vector.X, vector.Y));

            ctorParams[0] = (from v in verticesParam.ToArray() select v.X).ToArray();
            ctorParams[1] = (from v in verticesParam.ToArray() select v.Y).ToArray();
            ctorParams[2] = position.X;
            ctorParams[3] = position.Y;
            ctorParams[4] = angle;
            ctorParams[5] = density;
            ctorParams[6] = friction;
            ctorParams[7] = restitution;
            ctorParams[8] = isStatic;

            //TODO: Need to figure out how to get the vertices into the VelcroBody without using IVector or any
            //vector that implements IVector.  Maybe int[] array of X and another int[] array of Y?
            Body = Engine.PhysicsPlugins.GetPluginByType<IPhysicsBody>(ctorParams);
        }


        internal IPhysicsBody Body { get; set; }

        public Vector[] Vertices { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Angle { get; set; }

        public float Density { get; set; }

        public float Friction { get; set; }

        public float Restitution { get; set; }
    }
}

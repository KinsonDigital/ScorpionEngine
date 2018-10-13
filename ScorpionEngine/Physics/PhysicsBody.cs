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

            InternalPhysicsBody = Engine.PhysicsPlugins.GetPluginByType<IPhysicsBody>(ctorParams);
        }


        internal IPhysicsBody InternalPhysicsBody { get; set; }

        public Vector[] Vertices { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Angle { get; set; }

        public float Density { get; set; }

        public float Friction { get; set; }

        public float Restitution { get; set; }

        public float LinearAcceleration { get; set; }

        public float LinearDeceleration
        {
            get => InternalPhysicsBody.LinearDeceleration;
            set => InternalPhysicsBody.LinearDeceleration = value;
        }

        public float AngularAcceleration { get; set; }

        public float AngularDeceleration
        {
            get => InternalPhysicsBody.AngularDeceleration;
            set => InternalPhysicsBody.AngularDeceleration = value;
        }


        public Vector LinearVelocity
        {
            get => new Vector(InternalPhysicsBody.LinearVelocityX, InternalPhysicsBody.LinearVelocityY);
            set
            {
                InternalPhysicsBody.LinearVelocityX = value.X;
                InternalPhysicsBody.LinearVelocityY = value.Y;
            }
        }

        public float AngularVelocity
        {
            get => InternalPhysicsBody.AngularVelocity;
            set => InternalPhysicsBody.AngularVelocity = value;
        }
    }
}

using KDScorpionCore;
using KDScorpionCore.Plugins;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KDScorpionEngine.Physics
{
    public class PhysicsBody
    {
        /// <summary>
        /// Creates a new instance of <see cref="PhysicsBody"/> using the given <paramref name="internalPhysicsBody"/>.
        /// NOTE: This is only used for unit testing.
        /// </summary>
        /// <param name="internalPhysicsBody"></param>
        internal PhysicsBody(IPhysicsBody internalPhysicsBody) => InternalPhysicsBody = internalPhysicsBody;


        [ExcludeFromCodeCoverage]
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

            InternalPhysicsBody = EnginePluginSystem.Plugins.PhysicsPlugins.LoadPlugin<IPhysicsBody>(ctorParams);
        }


        internal IPhysicsBody InternalPhysicsBody { get; set; }

        public Vector[] Vertices
        {
            get
            {
                var result = new List<Vector>();

                if (InternalPhysicsBody.XVertices == null || InternalPhysicsBody.YVertices == null)
                    return null;

                for (int i = 0; i < InternalPhysicsBody.XVertices.Length; i++)
                {
                    result.Add(new Vector(InternalPhysicsBody.XVertices[i], InternalPhysicsBody.YVertices[i]));
                }


                return result.ToArray();
            }
            set
            {
                InternalPhysicsBody.XVertices = (from v in value select v.X).ToArray();
                InternalPhysicsBody.YVertices = (from v in value select v.Y).ToArray();
            }
        }

        public float X
        {
            get => InternalPhysicsBody.X;
            set => InternalPhysicsBody.X = value;
        }

        public float Y
        {
            get => InternalPhysicsBody.Y;
            set => InternalPhysicsBody.Y = value;
        }

        //In Degrees
        public float Angle
        {
            get => InternalPhysicsBody.Angle;
            set => InternalPhysicsBody.Angle = value;
        }

        public float Density
        {
            get => InternalPhysicsBody.Density;
            set => InternalPhysicsBody.Density = value;
        }

        public float Friction
        {
            get => InternalPhysicsBody.Friction;
            set => InternalPhysicsBody.Friction = value;
        }

        public float Restitution
        {
            get => InternalPhysicsBody.Restitution;
            set => InternalPhysicsBody.Restitution = value;
        }

        public float LinearDeceleration
        {
            get => InternalPhysicsBody.LinearDeceleration;
            set => InternalPhysicsBody.LinearDeceleration = value;
        }

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

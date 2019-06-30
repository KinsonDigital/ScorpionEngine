using KDScorpionCore.Plugins;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KDScorpionCore.Physics
{
    public class PhysicsBody
    {
        #region Private Fields
        private IPhysicsBody _internalPhysicsBody;
        private object[] _ctorParams;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="PhysicsBody"/> and
        /// injects the given <paramref name="body"/> for mocking and unit testing.
        /// </summary>
        /// <param name="body">The mocked body to inject.</param>
        internal PhysicsBody(IPhysicsBody body)
        {
            InternalPhysicsBody = body;
            Setup(new Vector[1] { Vector.Zero }, Vector.Zero, 0, 1, 0.2f, 0, false);
        }


        /// <summary>
        /// Creates a new instance of a <see cref="PhysicsBody"/>.
        /// </summary>
        /// <param name="vertices">The vertices that give the body shape.</param>
        /// <param name="position">The position of the body.</param>
        /// <param name="angle">The angle in degress that the body is rotated.</param>
        /// <param name="density">The density of the body.</param>
        /// <param name="friction">The friction of the body.</param>
        /// <param name="restitution">The restitution of the body.</param>
        /// <param name="isStatic">True if the body is a static body.</param>
        [ExcludeFromCodeCoverage]
        public PhysicsBody(Vector[] vertices, Vector position, float angle = 0, float density = 1, float friction = 0.2f, float restitution = 0, bool isStatic = false) =>
            Setup(vertices, position, angle, density, friction, restitution, isStatic);
        #endregion


        #region Props
        internal IPhysicsBody InternalPhysicsBody
        {
            [ExcludeFromCodeCoverage]
            get
            {
                if (_internalPhysicsBody == null)
                    _internalPhysicsBody = CorePluginSystem.Plugins.PhysicsPlugins.LoadPlugin<IPhysicsBody>(_ctorParams);


                return _internalPhysicsBody;
            }
            private set => _internalPhysicsBody = value;
        }

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
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the internal <see cref="IPhysicsBody"/>.
        /// </summary>
        /// <param name="vertices">The vertices that give the body shape.</param>
        /// <param name="position">The position of the body.</param>
        /// <param name="angle">The angle in degress that the body is rotated.</param>
        /// <param name="density">The density of the body.</param>
        /// <param name="friction">The friction of the body.</param>
        /// <param name="restitution">The restitution of the body.</param>
        /// <param name="isStatic">True if the body is a static body.</param>
        private void Setup(Vector[] vertices, Vector position, float angle, float density, float friction, float restitution, bool isStatic)
        {
            _ctorParams = new object[9];

            //Setup the vertices
            var verticesParam = new List<InternalVector>();

            foreach (var vector in vertices)
                verticesParam.Add(new InternalVector(vector.X, vector.Y));

            _ctorParams[0] = (from v in verticesParam.ToArray() select v.X).ToArray();
            _ctorParams[1] = (from v in verticesParam.ToArray() select v.Y).ToArray();
            _ctorParams[2] = position.X;
            _ctorParams[3] = position.Y;
            _ctorParams[4] = angle;
            _ctorParams[5] = density;
            _ctorParams[6] = friction;
            _ctorParams[7] = restitution;
            _ctorParams[8] = isStatic;
        }
        #endregion
    }
}

using ScorpionEngine;
using ScorpionEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Primitives;
using VelcroPhysics.Shared;

namespace VelcroPhysicsDriver
{
    //TODO: Add docs
    public class VelcroBody : IPhysicsBody
    {
        public VelcroBody(Vector[] vertices, Vector position, float angle, float density = 1, float friction = 0.2f, float restitution = 0, bool isStatic = false)
        {

        }


        internal Body PhysicsBody { get; set; }

        internal PolygonShape PolyShape { get; set; }

        public Vector Position
        {
            get
            {
                return PhysicsBody.Position.ToMonoVector().ToPixels();
            }
            set
            {
                PhysicsBody.Position = value.ToVelcroVector().ToPhysics();
            }
        }

        public float Angle { get; set; }

        public float Density { get; set; }

        public float Friction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float Restitution { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        public void DataSender(Func<dynamic> dataGetter)
        {
            var data = dataGetter() as object[];

            if (data == null)
                throw new Exception($"Data must be an array with 2 items.  Data type is {data.GetType()}");

            if (data.Length != 2)
                throw new Exception($"Data must be a length of exactly 2.  Total Items: {data.Length}");

            PhysicsBody = data[0] as Body;
            PolyShape = data[1] as PolygonShape;

            if (PhysicsBody == null)
                throw new Exception($"Setup data did not successfully cast to type {nameof(Body)}");

            if (PolyShape == null)
                throw new Exception($"Setup data did not successfully cast to type {nameof(PolygonShape)}");
        }
    }
}

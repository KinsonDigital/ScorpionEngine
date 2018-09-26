using ScorpionCore;
using ScorpionCore.Plugins;
using System;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Primitives;

namespace VelcroPhysicsDriver
{
    //TODO: Add docs
    public class VelcroBody : IPhysicsBody
    {
        public VelcroBody(VelVector[] vertices, VelVector position, float angle, float density = 1, float friction = 0.2f, float restitution = 0, bool isStatic = false)
        {

        }


        internal Body PhysicsBody { get; set; }

        internal PolygonShape PolyShape { get; set; }

        public float Angle { get; set; }

        public float Density { get; set; }

        public float Friction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float Restitution { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float X
        {
            get => PhysicsBody.Position.X;
            set => PhysicsBody.Position = new Vector2(PhysicsBody.Position.X + value, PhysicsBody.Position.Y);
        }

        public float Y
        {
            get => PhysicsBody.Position.Y;
            set => PhysicsBody.Position = new Vector2(PhysicsBody.Position.X, PhysicsBody.Position.Y + value);
        }

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

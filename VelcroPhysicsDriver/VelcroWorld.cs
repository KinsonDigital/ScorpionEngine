using ScorpionCore;
using ScorpionCore.Plugins;
using System;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Primitives;
using VelcroPhysics.Shared;

namespace VelcroPhysicsDriver
{
    public class VelcroWorld : IPhysicsWorld
    {
        public static World PhysicsWorld { get; set; }

        public IVector Gravity { get; set; }


        public VelcroWorld(IVector gravity)
        {
            PhysicsWorld = new World(gravity.ToVelcroVector());
        }


        #region Public Methods
        public void Update(float dt)
        {
            PhysicsWorld.Step(dt);
        }


        public void AddBody<T>(T body, IVector[] vertices) where T : IPhysicsBody
        {
            var velVertices = new Vertices();

            foreach (var vert in vertices)
            {
                velVertices.Add(new Vector2(vert.X.ToPhysics(), vert.Y.ToPhysics()));
            }


            var physicsBody = BodyFactory.CreatePolygon(PhysicsWorld, velVertices, body.Density, new Vector2(body.X, body.Y).ToPhysics(), body.Angle.ToRadians(), BodyType.Dynamic);
            physicsBody.Friction = body.Friction;
            physicsBody .Restitution = body.Restitution;
            var polyShape = (PolygonShape)physicsBody.FixtureList[0].Shape;

            //WARNING: This data sender is not ideal and error prone as well as 
            //not performent.  Need to find a way to send data to the physics body 
            //in a type safe and performant way
            body.DataSender(() => new object[] { physicsBody, polyShape });
        }


        public void GetBody()
        {
            throw new NotImplementedException();
        }


        public void RemoveBody()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

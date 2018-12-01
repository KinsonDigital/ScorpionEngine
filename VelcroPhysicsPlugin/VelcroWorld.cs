using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Primitives;
using VelcroPhysics.Shared;

namespace VelcroPhysicsPlugin
{
    public class VelcroWorld : IPhysicsWorld
    {
        internal static World PhysicsWorld { get; set; }

        public float GravityX { get; set; }

        public float GravityY { get; set; }


        public VelcroWorld()
        {
            //Required for the plugin system to work
        }


        public VelcroWorld(float gravityX, float gravityY)
        {
            PhysicsWorld = new World(new Vector2(gravityX, gravityY));

            GravityX = gravityX;
            GravityY = gravityY;
        }


        #region Public Methods
        public void Update(float dt)
        {
            PhysicsWorld.Step(dt);
        }


        public void AddBody<T>(T body) where T : IPhysicsBody
        {
            var velVertices = new Vertices();

            var xVertices = body.GetData("x_vertices") as float[];
            var yVertices = body.GetData("y_vertices") as float[];
            var vectors = new List<Vector2>();

            for (int i = 0; i < xVertices.Length; i++)
            {
                velVertices.Add(new Vector2(xVertices[i], yVertices[i]).ToPhysics());
            }

            var xPosition = float.Parse(body.GetData("x_position").ToString());
            var yPosition = float.Parse(body.GetData("y_position").ToString());
            var angle = float.Parse(body.GetData("angle").ToString());
            var density = float.Parse(body.GetData("density").ToString());
            var friction = float.Parse(body.GetData("friction").ToString());
            var restitution = float.Parse(body.GetData("restitution").ToString());
            var isStatic = bool.Parse(body.GetData("is_static").ToString());

            var physicsBody = BodyFactory.CreatePolygon(PhysicsWorld, velVertices, density, new Vector2(xPosition, yPosition).ToPhysics(), angle.ToRadians(), isStatic ? BodyType.Static : BodyType.Dynamic);
            physicsBody.Friction = friction;
            physicsBody .Restitution = restitution;
            var polyShape = (PolygonShape)physicsBody.FixtureList[0].Shape;

            //WARNING: This data sender is not ideal and error prone as well as 
            //not performant.  Need to find a way to send data to the physics body 
            //in a type safe and performant way
            body.DataSender(() => new object[] { physicsBody, polyShape, friction });

            //Execute any deferred actions if any exist
            body.AfterAddedToWorldActions.ExecuteAll();
        }


        public void GetBody()
        {
            throw new NotImplementedException();
        }


        public void RemoveBody()
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

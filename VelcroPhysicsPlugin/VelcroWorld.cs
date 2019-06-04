using KDScorpionCore;
using KDScorpionCore.Plugins;
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

            var bodySettings = body.GetData<PhysicsBodySettings>(100);

            //var xVertices = body.GetData<float[]>("x_vertices");//1
            //var yVertices = body.GetData("y_vertices") as float[];//2
            var vectors = new List<Vector2>();

            for (int i = 0; i < bodySettings.XVertices.Length; i++)
            {
                velVertices.Add(new Vector2(bodySettings.XVertices[i], bodySettings.YVertices[i]).ToPhysics());
            }

            //var xPosition = float.Parse(body.GetData("x_position").ToString());//3
            //var yPosition = float.Parse(body.GetData("y_position").ToString());//4
            //var angle = float.Parse(body.GetData("angle").ToString());//5
            //var density = float.Parse(body.GetData("density").ToString());//6
            //var friction = float.Parse(body.GetData("friction").ToString());//7
            //var restitution = float.Parse(body.GetData("restitution").ToString());//8
            //var isStatic = bool.Parse(body.GetData("is_static").ToString());//9

            var physicsBody = BodyFactory.CreatePolygon(PhysicsWorld, velVertices, bodySettings.Density, new Vector2(bodySettings.XPosition, bodySettings.YPosition).ToPhysics(), bodySettings.Angle.ToRadians(), bodySettings.IsStatic ? BodyType.Static : BodyType.Dynamic);
            physicsBody.Friction = bodySettings.Friction;
            physicsBody.Restitution = bodySettings.Restitution;
            var polyShape = (PolygonShape)physicsBody.FixtureList[0].Shape;

            body.InjectData(physicsBody);
            body.InjectData(polyShape);

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


        public T GetData<T>(int option) where T : class
        {
            throw new NotImplementedException();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

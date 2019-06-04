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

            for (int i = 0; i < bodySettings.XVertices.Length; i++)
            {
                velVertices.Add(new Vector2(bodySettings.XVertices[i], bodySettings.YVertices[i]).ToPhysics());
            }

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

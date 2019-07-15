﻿using KDScorpionCore;
using KDScorpionCore.Plugins;
using System;
using System.Diagnostics.CodeAnalysis;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Primitives;
using VelcroPhysics.Shared;

namespace VelcroPhysicsPlugin
{
    /// <summary>
    /// Represents a world with simulated physics.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VelcroWorld : IPhysicsWorld
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="VelcroWorld"/>.
        /// NOTE: Required for the plugin system to work. The IoC container must have a parameterless constructor
        /// </summary>
        public VelcroWorld() { }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the velcro physics world that is used internally.
        /// </summary>
        internal static World PhysicsWorld { get; set; }

        /// <summary>
        /// Gets or sets the world's gravity in the X plane.
        /// </summary>
        public float GravityX { get; set; }

        /// <summary>
        /// Gets or sets the world's gravity in the Y plane.
        /// </summary>
        public float GravityY { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the physics world.
        /// </summary>
        /// <param name="dt">The time that has passed for the current frame.</param>
        public void Update(float dt) => PhysicsWorld.Step(dt);


        /// <summary>
        /// Adds the given <paramref name="body"/> to the physics world.
        /// </summary>
        /// <typeparam name="T">The type of physics body to add.</typeparam>
        /// <param name="body">The body to add.</param>
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


        /// <summary>
        /// Removes the body from the physics world.
        /// </summary>
        public void RemoveBody() => throw new NotImplementedException();


        /// <summary>
        /// Gets the from the world.
        /// </summary>
        public T GetBody<T>() => throw new NotImplementedException();


        /// <summary>
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();
        #endregion
    }
}

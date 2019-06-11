using KDScorpionCore;
using KDScorpionCore.Plugins;
using System;
using System.Linq;
using System.Collections.Generic;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Primitives;

namespace VelcroPhysicsPlugin
{
    //TODO: Add docs
    public class VelcroBody : IPhysicsBody
    {
        private readonly PhysicsBodySettings _tempSettings = new PhysicsBodySettings();


        #region Constructors
        public VelcroBody()
        {
            //This must be here for the plugin system to work
        }


        public VelcroBody(float[] xVertices, float[] yVertices, float xPosition, float yPosition, float angle, float density = 1, float friction = 0.2f, float restitution = 0, bool isStatic = false)
        {
            if (xVertices.Length != yVertices.Length)
                throw new ArgumentOutOfRangeException($"The params {nameof(xVertices)} and {nameof(yVertices)} must have the same number of elements.");

            _tempSettings.XVertices = xVertices;
            _tempSettings.YVertices = yVertices;
            _tempSettings.XPosition = xPosition;
            _tempSettings.YPosition = yPosition;
            _tempSettings.Angle = angle;
            _tempSettings.Density = density;
            _tempSettings.Friction = friction;
            _tempSettings.Restitution = restitution;
            _tempSettings.IsStatic = isStatic;
        }
        #endregion


        #region Props
        internal Body PolygonBody { get; set; }

        internal PolygonShape PolygonShape { get; set; }

        public DeferredActions AfterAddedToWorldActions { get; set; } = new DeferredActions();

        public float[] XVertices
        {
            get
            {
                var result = new List<float>();
                var positionX = PolygonBody.Position.X;//In physics units

                if (PolygonShape == null)
                {
                    result.AddRange(_tempSettings.XVertices);
                }
                else
                {
                    //This gets the vertices as world vertices
                    var xVertices = (from v in PolygonShape.Vertices
                                     select v.X + positionX).ToArray();

                    result.AddRange(xVertices.ToPixels());
                }


                return result.ToArray();
            }
            set { }
        }

        public float[] YVertices
        {
            get
            {
                var result = new List<float>();
                var positionY = PolygonBody.Position.Y;//In physics units

                if (PolygonShape == null)
                {
                    result.AddRange(_tempSettings.YVertices);
                }
                else
                {
                    //This gets the vertices as world vertices
                    var yVertices = (from v in PolygonShape.Vertices
                                     select v.Y + positionY).ToArray();

                    result.AddRange(yVertices.ToPixels());
                }


                return result.ToArray();
            }
            set { }
        }

        public float X
        {
            get => PolygonBody == null ? _tempSettings.XPosition : PolygonBody.Position.X.ToPixels();
            set
            {
                if (PolygonBody == null)
                    throw new Exception("Body must be added to a world first");

                PolygonBody.Position = new Vector2(value.ToPhysics(), PolygonBody.Position.Y);
            }
        }

        public float Y
        {
            get => PolygonBody == null ? _tempSettings.YPosition : PolygonBody.Position.Y.ToPixels();
            set
            {
                if (PolygonBody == null)
                    throw new Exception("Body must be added to a world first");

                PolygonBody.Position = new Vector2(PolygonBody.Position.X, value.ToPhysics());
            }
        }

        /// <summary>
        /// Gets the angle of the body in degrees.
        /// </summary>
        public float Angle
        {
            get => PolygonBody == null ? _tempSettings.Angle : PolygonBody.Rotation.ToDegrees();
            set
            {
                if(PolygonBody == null)
                {
                    AfterAddedToWorldActions.Add(() =>
                    {
                        PolygonBody.Rotation = value.ToRadians();
                    });
                }
                else
                {
                    PolygonBody.Rotation = value.ToRadians();
                }

                _tempSettings.Angle = value;//Degrees
            }
        }

        public float Density
        {
            get => PolygonShape == null ? _tempSettings.Density : PolygonShape.Density;
            set
            {
                //TODO: We might be able to change the density after its been added, look into this.
                throw new Exception("Cannot set the density after the body has been add to the world");
            }
        }

        public float Friction
        {
            get => _tempSettings.Friction;
            set => PolygonBody.Friction = value;
        }

        public float Restitution
        {
            get => _tempSettings.Restitution;
            set
            {
                if (PolygonBody == null)
                {
                    AfterAddedToWorldActions.Add(() =>
                    {
                        PolygonBody.Restitution = value;
                    });
                }
                else
                {
                    PolygonBody.Restitution = value;
                }

                _tempSettings.Restitution = value;
            }
        }

        public float LinearVelocityX
        {
            get => PolygonBody.LinearVelocity.X.ToPixels();
            set
            {
                PolygonBody.LinearVelocity = new Vector2(value.ToPhysics(), PolygonBody.LinearVelocity.Y);
            }
        }

        public float LinearVelocityY
        {
            get => PolygonBody.LinearVelocity.Y.ToPixels();
            set
            {
                PolygonBody.LinearVelocity = new Vector2(PolygonBody.LinearVelocity.X, value.ToPhysics());
            }
        }

        public float LinearDeceleration
        {
            get => PolygonBody.LinearDamping.ToPixels();
            set
            {
                if (PolygonBody == null)
                {
                    AfterAddedToWorldActions.Add(() =>
                    {
                        PolygonBody.LinearDamping = value.ToPhysics();
                    });
                }
                else
                {
                    PolygonBody.LinearDamping = value.ToPhysics();
                }
            }
        }

        public float AngularVelocity
        {
            get => PolygonBody.AngularVelocity.ToPixels();
            set
            {
                PolygonBody.AngularVelocity = value.ToPhysics();
            }
        }

        public float AngularDeceleration
        {
            get => PolygonBody.AngularDamping.ToPixels();
            set
            {
                if (PolygonBody == null)
                {
                    AfterAddedToWorldActions.Add(() =>
                    {
                        PolygonBody.AngularDamping = value.ToPhysics();
                    });
                }
                else
                {
                    PolygonBody.AngularDamping = value.ToPhysics();
                }
            }
        }
        #endregion


        #region Public Methods
        public void ApplyLinearImpulse(float x, float y) => PolygonBody.ApplyLinearImpulse(new Vector2(x.ToPhysics(), y.ToPhysics()));


        public void ApplyAngularImpulse(float value) => PolygonBody.ApplyAngularImpulse(value.ToPhysics());


        public void ApplyForce(float forceX, float forceY, float worldLocationX, float worldLocationY) =>
            PolygonBody.ApplyForce(new Vector2(forceX.ToPhysics(), forceY.ToPhysics()), new Vector2(worldLocationX.ToPhysics(), worldLocationY.ToPhysics()));


        public T GetData<T>(int option) where T : class
        {
            if (option == 100)
            {
                return _tempSettings as T;
            }
            else
            {
                throw new Exception($"Do not recognize the option '{option}'");
            }
        }


        public void InjectData<T>(T data) where T : class
        {
            if (data.GetType() == typeof(Body))
            {
                PolygonBody = data as Body;
            }
            else if(data.GetType() == typeof(PolygonShape))
            {
                PolygonShape = data as PolygonShape;
            }
            else
            {
                throw new Exception($"Data getting injected into {nameof(VelcroBody)} is not of type {nameof(Body)} or {nameof(PolygonShape)}.  Incorrect type is {data.GetType().ToString()}");
            }
        }
        #endregion
    }
}

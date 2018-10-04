using ScorpionCore;
using ScorpionCore.Plugins;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Primitives;
using VelcroPhysics.Shared;

namespace VelcroPhysicsPlugin
{
    //TODO: Add docs
    public class VelcroBody : IPhysicsBody
    {
        private PhysicsBodySettings _tempSettings;


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


        #region Props
        internal Body PolygonBody { get; set; }

        internal PolygonShape PolygonShape { get; set; }

        public float[] XVertices
        {
            get
            {
                var result = new List<float>();

                if (PolygonShape == null)
                {
                    result.AddRange(_tempSettings.XVertices);
                }
                else
                {
                    var xVertices = (from v in PolygonShape.Vertices.ToVelcroVectors()
                                    select v.X).ToArray();

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

                if (PolygonShape == null)
                {
                    result.AddRange(_tempSettings.YVertices);
                }
                else
                {
                    var yVertices = (from v in PolygonShape.Vertices.ToVelcroVectors()
                                     select v.Y).ToArray();

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

                PolygonBody.Position = new Vector2(PolygonBody.Position.X + value, PolygonBody.Position.Y);
            }
        }

        public float Y
        {
            get => PolygonBody == null ? _tempSettings.YPosition : PolygonBody.Position.Y.ToPixels();
            set
            {
                if (PolygonBody == null)
                    throw new Exception("Body must be added to a world first");

                PolygonBody.Position = new Vector2(PolygonBody.Position.X, PolygonBody.Position.Y + value);
            }
        }

        public float Angle
        {
            get => PolygonBody == null ? _tempSettings.Angle : PolygonBody.Rotation;
            set
            {
                if (PolygonBody == null)
                    throw new Exception("Body must be added to a world first");

                PolygonBody.Rotation = value;
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
            set
            {
                //TODO: We might be able to change the friction after its been added, look into this.
                throw new Exception("Cannot set the fiction after the body has been add to the world");
            }
        }

        public float Restitution
        {
            get => _tempSettings.Restitution;
            set
            {
                //TODO: We might be able to change the restitution after its been added, look into this.
                throw new Exception("Cannot set the restitution after the body has been add to the world");
            }
        }
        #endregion


        #region Public Methos
        public void DataSender(Func<dynamic> dataGetter)
        {
            var result = dataGetter();

            //TODO: Do some array and array element type checking here. Very verbose
            //exception methods will help with debugging and troubleshooting
            PolygonBody = result[0];
            PolygonShape = result[1];
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public object GetData(string dataType)
        {
            switch (dataType)
            {
                case "x_vertices":
                    return _tempSettings.XVertices;
                case "y_vertices":
                    return _tempSettings.YVertices;
                case "x_position":
                    return _tempSettings.XPosition;
                case "y_position":
                    return _tempSettings.YPosition;
                case "angle":
                    return _tempSettings.Angle;
                case "density":
                    return _tempSettings.Density;
                case "friction":
                    return _tempSettings.Friction;
                case "restitution":
                    return _tempSettings.Restitution;
                case "is_static":
                    return _tempSettings.IsStatic;
                default:
                    return null;
            }
        }
        #endregion
    }
}

using Raptor;
using Raptor.Plugins;
using System;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngineTests.Fakes
{
    /// <summary>
    /// Provides a fake implementation of the <see cref="IPhysicsBody"/> interface.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakePhysicsBody : IPhysicsBody
    {
        #region Constructors
        public FakePhysicsBody(float[] xVertices, float[] yVertices)
        {
            XVertices = xVertices;
            YVertices = yVertices;
        }


        public FakePhysicsBody(float[] xVertices, float[] yVertices, float x, float y)
        {
            XVertices = xVertices;
            YVertices = yVertices;
            X = x;
            Y = y;
        }
        #endregion


        #region Props
        public float[] XVertices { get; set; }

        public float[] YVertices { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float Angle { get; set; }

        public float Density { get; set; }

        public float Friction { get; set; }

        public float Restitution { get; set; }

        public float LinearVelocityX { get; set; }

        public float LinearVelocityY { get; set; }

        public float LinearDeceleration { get; set; }

        public float AngularVelocity { get; set; }

        public float AngularDeceleration { get; set; }

        public DeferredActions AfterAddedToWorldActions { get; set; }
        #endregion


        #region Public Methods
        public void ApplyAngularImpulse(float value)
        {
            AngularVelocity += value;

            Angle = value;
        }


        public void ApplyForce(float forceX, float forceY, float worldLocationX, float worldLocationY)
        {
            X = worldLocationX + forceX;
            Y = forceY;
            LinearVelocityX += forceX;
            LinearVelocityY += forceY;
        }


        public void ApplyLinearImpulse(float x, float y)
        {
            X = x;
            Y = y;
            LinearVelocityX += x;
            LinearVelocityY += y;
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

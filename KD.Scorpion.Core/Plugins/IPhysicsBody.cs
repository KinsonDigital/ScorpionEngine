using PluginSystem;

namespace KDScorpionCore.Plugins
{
    //TODO: Add docs
    public interface IPhysicsBody : IPlugin
    {
        #region Props
        float[] XVertices { get; set; }

        float[] YVertices { get; set; }

        float X { get; set; }

        float Y { get; set; }

        /// <summary>
        /// Gets or sets the angle of the physics body in degrees.
        /// </summary>
        float Angle { get; set; }

        float Density { get; set; }

        float Friction { get; set; }

        float Restitution { get; set; }

        float LinearVelocityX { get; set; }

        float LinearVelocityY { get; set; }

        float LinearDeceleration { get; set; }

        float AngularVelocity { get; set; }

        float AngularDeceleration { get; set; }

        DeferredActions AfterAddedToWorldActions { get; set; }
        #endregion


        #region Methods
        void ApplyLinearImpulse(float x, float y);


        void ApplyAngularImpulse(float value);


        void ApplyForce(float forceX, float forceY, float worldLocationX, float worldLocationY);
        #endregion
    }
}

using KDScorpionCore.Plugins;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionCore.Physics
{
    /// <summary>
    /// Represents a world with simulated physics.
    /// </summary>
    public class PhysicsWorld
    {
        #region Private Fields
        private readonly IPhysicsWorld _internalWorld;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="PhysicsWorld"/> that will
        /// hold all instances of <see cref="PhysicsBody"/> objects and also
        /// control the physics of those bodies. Used for testing.
        /// </summary>
        /// <param name="physicsWorld">The mocked world object used for unit testing.</param>
        internal PhysicsWorld(IPhysicsWorld physicsWorld) => _internalWorld = physicsWorld;


        /// <summary>
        /// Creates a new instance of <see cref="PhysicsWorld"/> that will
        /// hold all instances of <see cref="PhysicsBody"/> objects and also
        /// control the physics of those bodies.
        /// </summary>
        /// <param name="gravity">The gravity of the world.</param>
        [ExcludeFromCodeCoverage]
        public PhysicsWorld(Vector gravity) =>
            _internalWorld = CorePluginSystem.Plugins.PhysicsPlugins.LoadPlugin<IPhysicsWorld>(new object[] { gravity.X, gravity.Y });
        #endregion


        #region Props
        /// <summary>
        /// Gets the worlds gravity.
        /// </summary>
        public Vector Gravity => new Vector(_internalWorld.GravityX, _internalWorld.GravityY);
        #endregion


        #region Public Methods
        /// <summary>
        /// Adds the given <paramref name="body"/> to the world.
        /// </summary>
        /// <param name="body">The body to add.</param>
        public void AddBody(IPhysicsBody body) => _internalWorld.AddBody(body);


        /// <summary>
        /// Updates the physics world to keep the physics simulation moving ahead.
        /// </summary>
        /// <param name="dt">The time passed in milliseconds since the last frame.</param>
        public void Update(float dt) => _internalWorld.Update(dt);
        #endregion
    }
}

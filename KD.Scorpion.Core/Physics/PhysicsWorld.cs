using KDScorpionCore.Plugins;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionCore.Physics
{
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
        public Vector Gravity => new Vector(_internalWorld.GravityX, _internalWorld.GravityY);
        #endregion


        #region Public Methods
        public void AddBody(IPhysicsBody body)
        {
            //Only add it to the physics world if the entity has been initialized.
            _internalWorld.AddBody(body);
        }


        public void Update(float dt) => _internalWorld.Update(dt);
        #endregion
    }
}

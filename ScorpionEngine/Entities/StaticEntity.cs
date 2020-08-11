// <copyright file="StaticEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using Raptor;
using Raptor.Graphics;
using Raptor.Plugins;
using System.Numerics;

namespace KDScorpionEngine.Entities
{
    /// <summary>
    /// A static entity that can be rendered to the screen.
    /// </summary>
    public class StaticEntity : Entity
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="StaticEntity"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="body">The physics body to inject.</param>
        internal StaticEntity(IPhysicsBody body) : base(body) { }


        /// <summary>
        /// Creates a new instance of <see cref="StaticEntity"/>.
        /// </summary>
        /// <param name="texture">The texture fo the entity to render to the screen.</param>
        /// <param name="position">The position on the surface of where to render the texture.</param>
        public StaticEntity(Texture texture, Vector2 position) : base(texture, position, isStaticBody: true) => _usesPhysics = false;
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the <see cref="StaticEntity"/>.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public override void Update(EngineTime engineTime) => base.Update(engineTime);
        #endregion
    }
}

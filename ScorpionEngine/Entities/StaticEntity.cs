// <copyright file="StaticEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System.Numerics;
    using Raptor.Graphics;

    /// <summary>
    /// A static entity that can be rendered to the screen.
    /// </summary>
    public class StaticEntity : Entity
    {
        /// <summary>
        /// Creates a new instance of <see cref="StaticEntity"/>.
        /// </summary>
        /// <param name="texture">The texture fo the entity to render to the screen.</param>
        /// <param name="position">The position on the surface of where to render the texture.</param>
        public StaticEntity(Texture texture, Vector2 position)
            : base(texture, position, isStaticBody: true) => this.usesPhysics = false;

        /// <summary>
        /// Updates the <see cref="StaticEntity"/>.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public override void Update(GameTime gameTime) => base.Update(gameTime);
    }
}

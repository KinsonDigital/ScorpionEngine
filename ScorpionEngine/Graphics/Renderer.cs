// <copyright file="Renderer.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System.Drawing;
    using KDScorpionEngine.Entities;
    using Raptor.Factories;
    using Raptor.Graphics;

    /// <summary>
    /// Renders entities to a graphics surface.
    /// </summary>
    public class Renderer
    {
        private readonly ISpriteBatch spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        public Renderer(int renderSurfaceWidth, int renderSurfaceHeight)
        {
            this.spriteBatch = SpriteBatchFactory.CreateSpriteBatch(renderSurfaceWidth, renderSurfaceHeight);
        }

        internal void Clear() => this.spriteBatch.Clear();

        internal void Begin()
        {
            this.spriteBatch.BeginBatch();
        }

        internal void End()
        {
            this.spriteBatch.EndBatch();
        }

        /// <summary>
        /// Renders the given entity.
        /// </summary>
        /// <param name="entity">The entity to render.</param>
        public void Render(Entity entity)
        {
            this.spriteBatch.Render(entity.Texture, (int)entity.Position.X, (int)entity.Position.Y, Color.White);
        }

        public void Render(ITexture texture)
        {
            this.spriteBatch.Render(texture, 200, 200);
        }
    }
}

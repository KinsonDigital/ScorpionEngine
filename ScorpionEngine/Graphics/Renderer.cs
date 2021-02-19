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

        public Renderer(ISpriteBatch spriteBatch, int renderSurfaceWidth, int renderSurfaceHeight)
        {
            this.spriteBatch = spriteBatch;
            this.spriteBatch.RenderSurfaceWidth = renderSurfaceWidth;
            this.spriteBatch.RenderSurfaceHeight = renderSurfaceHeight;
        }

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
        public void Render(IEntity entity)
        {
            if (!entity.Visible || entity.Texture is null)
            {
                return;
            }

            var srcRect = new Rectangle(
                (int)entity.RenderSection.GetTexturePosition().X,
                (int)entity.RenderSection.GetTexturePosition().Y,
                entity.RenderSection.RenderBounds.Width,
                entity.RenderSection.RenderBounds.Height);

            var destRect = new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.Texture.Width, entity.Texture.Height);

            this.spriteBatch.Render(entity.Texture, srcRect, destRect, 1, 0, Color.White);
        }

        public void Render(ITexture texture)
        {
            this.spriteBatch.Render(texture, 200, 200);
        }
    }
}

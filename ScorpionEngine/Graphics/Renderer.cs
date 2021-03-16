// <copyright file="Renderer.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using KDScorpionEngine.Entities;
    using Raptor.Factories;
    using Raptor.Graphics;

    /// <summary>
    /// Renders entities to a graphics surface.
    /// </summary>
    public class Renderer : IRenderer
    {
        private readonly ISpriteBatch spriteBatch;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        /// <param name="spriteBatch">Performs the rendering to the screen.</param>
        /// <param name="renderSurfaceWidth">
        ///     The width of the rendering surface.
        /// <para>
        ///     NOTE: This is usually the entire width of the window.
        /// </para>
        /// </param>
        /// <param name="renderSurfaceHeight">
        ///     The height of the rendering surface.
        /// <para>
        ///     NOTE: This is usually the entire height of the window.
        /// </para>
        /// </param>
        public Renderer(ISpriteBatch spriteBatch, int renderSurfaceWidth, int renderSurfaceHeight)
        {
            this.spriteBatch = spriteBatch;
            this.spriteBatch.RenderSurfaceWidth = renderSurfaceWidth;
            this.spriteBatch.RenderSurfaceHeight = renderSurfaceHeight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        /// <param name="renderSurfaceWidth">
        ///     The width of the rendering surface.
        /// <para>
        ///     NOTE: This is usually the entire width of the window.
        /// </para>
        /// </param>
        /// <param name="renderSurfaceHeight">
        ///     The height of the rendering surface.
        /// <para>
        ///     NOTE: This is usually the entire height of the window.
        /// </para>
        /// </param>
        [ExcludeFromCodeCoverage]
        public Renderer(int renderSurfaceWidth, int renderSurfaceHeight)
            => this.spriteBatch = SpriteBatchFactory.CreateSpriteBatch(renderSurfaceWidth, renderSurfaceHeight);

        /// <inheritdoc/>
        public void Render(IEntity entity)
        {
            if (!entity.Visible || entity.Texture is null)
            {
                return;
            }

            var srcRect = new Rectangle(
                (int)entity.SectionToRender.GetTexturePosition().X,
                (int)entity.SectionToRender.GetTexturePosition().Y,
                entity.SectionToRender.RenderBounds.Width,
                entity.SectionToRender.RenderBounds.Height);

            var destRect = new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.Texture.Width, entity.Texture.Height);

            this.spriteBatch.Render(entity.Texture, srcRect, destRect, 1, 0, Color.White);
        }

        /// <inheritdoc/>
        public void Render(ITexture texture, int x, int y) => this.spriteBatch.Render(texture, x, y);

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clears the screen.
        /// </summary>
        public void Clear() => this.spriteBatch.Clear();

        /// <summary>
        /// Begins the process for rendering a batch.
        /// </summary>
        /// <remarks>You must invoke this before invoking the <see cref="End"/>() method.</remarks>
        public void Begin() => this.spriteBatch.BeginBatch();

        /// <summary>
        /// Ends the process of rendering a batch.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Invoking this will actually perform the rendering on the GPU.
        /// </para>
        /// <para>
        ///     The <see cref="Begin"/>() method must be invoked before this is invoked.
        /// </para>
        /// </remarks>
        public void End() => this.spriteBatch.EndBatch();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.spriteBatch.Dispose();
            }

            this.isDisposed = true;
        }
    }
}

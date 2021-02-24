// <copyright file="IRenderer.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System;
    using KDScorpionEngine.Entities;
    using Raptor.Graphics;

    /// <summary>
    /// Renders <see cref="ITexture"/>s and <see cref="Entity"/> objects to the screen.
    /// </summary>
    public interface IRenderer : IDisposable
    {
        /// <summary>
        /// Renders the given <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The entity to render.</param>
        void Render(IEntity entity);

        /// <summary>
        /// Renders the given <paramref name="texture"/> at the given
        /// <paramref name="x"/> and <paramref name="y"/> position.
        /// </summary>
        /// <param name="texture">The texture to render.</param>
        /// <param name="y">The X coordinate of where to render the texture.</param>
        /// <param name="x">The Y coordinate of where to render the texture.</param>
        void Render(ITexture texture, int y, int x);
    }
}

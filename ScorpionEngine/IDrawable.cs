﻿// <copyright file="IDrawable.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using KDScorpionEngine.Graphics;

    /// <summary>
    /// Provides functionality for game content to be rendered to the screen.
    /// </summary>
    public interface IDrawable
    {
        #region Methods
        /// <summary>
        /// Renders things to the graphics surface using the given <paramref name="renderer"/>.
        /// </summary>
        /// <param name="renderer">The rederer to use for rendering.</param>
        void Render(GameRenderer renderer);
        #endregion
    }
}

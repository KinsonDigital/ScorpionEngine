// <copyright file="AtlasSpriteData.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Content
{
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;

    // TODO: Look into using this later during the building of a test game.
    [ExcludeFromCodeCoverage]
    internal class AtlasSpriteData
    {
        /// <summary>
        /// Gets or sets the bounds of the sprite data.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the name of the sprite data.
        /// </summary>
        public string Name { get; set; }
    }
}

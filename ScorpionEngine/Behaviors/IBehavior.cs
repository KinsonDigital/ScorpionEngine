// <copyright file="IBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    /// <summary>
    /// A simple behavior with an enabled state and name.
    /// </summary>
    public interface IBehavior : IUpdatableObject
    {
        /// <summary>
        /// Enables or disables the <see cref="IBehavior"/>.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="IBehavior"/>.
        /// </summary>
        string Name { get; set; }
    }
}

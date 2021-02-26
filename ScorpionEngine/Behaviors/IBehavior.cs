// <copyright file="IBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;

    /// <summary>
    /// A simple behavior with an enabled state and name.
    /// </summary>
    public interface IBehavior : IUpdatableObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="IBehavior"/> is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="IBehavior"/>.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the behavior.
        /// </summary>
        Guid ID { get; }
    }
}

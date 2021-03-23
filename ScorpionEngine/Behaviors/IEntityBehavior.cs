// <copyright file="IEntityBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;
    using KDScorpionEngine.Entities;

    /// <summary>
    /// A simple behavior with an enabled state and name.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity that the behavior will manipulate.</typeparam>
    public interface IEntityBehavior : IUpdatableObject, IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="IEntityBehavior"/> is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets the ID of the behavior.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// Gets the entity that the behavior is attached to.
        /// </summary>
        IEntity Entity { get; }
    }
}

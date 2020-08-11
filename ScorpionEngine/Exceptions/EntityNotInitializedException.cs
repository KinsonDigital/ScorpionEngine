// <copyright file="EntityNotInitializedException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;
    using KDScorpionEngine.Entities;
    using Raptor.Physics;

    /// <summary>
    /// Thrown when an entity is trying to be added to a <see cref="PhysicsWorld"/> without being initialized.
    /// </summary>
    public class EntityNotInitializedException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="EntityNotInitializedException"/>.
        /// </summary>
        public EntityNotInitializedException()
            : base($"{nameof(Entity)} not initialized.  Must be initialized before being added to a {nameof(PhysicsWorld)} using {nameof(Entity)}.{nameof(Entity.Initialize)}() method.")
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="EntityNotInitializedException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public EntityNotInitializedException(string message)
            : base(message)
        {
        }
    }
}

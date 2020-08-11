// <copyright file="EntityAlreadyInitializedException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;
    using KDScorpionEngine.Entities;

    /// <summary>
    /// Thrown when an entity has already been initialized.
    /// </summary>
    public class EntityAlreadyInitializedException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="EntityAlreadyInitializedException"/>.
        /// </summary>
        public EntityAlreadyInitializedException() : base($"{nameof(Entity)} is already initialized.  Invocation must be performed before using the {nameof(Entity)}.{nameof(Entity.Initialize)}() method.") { }

        /// <summary>
        /// Creates a new instance of <see cref="EntityAlreadyInitializedException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public EntityAlreadyInitializedException(string message) : base(message) { }
    }
}

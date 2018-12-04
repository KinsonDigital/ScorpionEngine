﻿using ScorpionEngine.Entities;
using System;

namespace ScorpionEngine.Exceptions
{
    /// <summary>
    /// Thrown when an entity is trying to be added to a <see cref="PhysicsWorld"/> without being initialized.
    /// </summary>
    public class EntityAlreadyInitializedException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="EntityAlreadyInitializedException"/>.
        /// </summary>
        public EntityAlreadyInitializedException() : base($"{nameof(Entity)} is already initialized.  Invocation must be performed before using the {nameof(Entity)}.{nameof(Entity.Initialize)}() method.")
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="EntityAlreadyInitializedException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public EntityAlreadyInitializedException(string message) : base(message)
        {
        }
        #endregion
    }
}

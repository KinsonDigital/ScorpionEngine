// <copyright file="IDAlreadyExistsException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;

    /// <summary>
    /// Thrown if an ID already exists.
    /// </summary>
    public class IdAlreadyExistsException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="IdAlreadyExistsException"/>.
        /// </summary>
        public IdAlreadyExistsException() : base("That ID already exists.") { }

        /// <summary>
        /// Creates a new instance of <see cref="IdAlreadyExistsException"/>.
        /// </summary>
        /// <param name="id">The ID that already exists.</param>
        public IdAlreadyExistsException(int id) : base($"The ID '{id}' already exists.") { }

        /// <summary>
        /// Creates a new instance of <see cref="IdAlreadyExistsException"/>.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public IdAlreadyExistsException(string message) : base(message) { }
        #endregion
    }
}

// <copyright file="IdNotFoundException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;

    /// <summary>
    /// Thrown if an ID has not been found.
    /// </summary>
    public class IdNotFoundException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="IdNotFoundException"/>.
        /// </summary>
        public IdNotFoundException() : base("The ID has not been found.") { }

        /// <summary>
        /// Creates a new instance of <see cref="IdNotFoundException"/>.
        /// </summary>
        /// <param name="sceneId">The ID that has not been found.</param>
        public IdNotFoundException(int sceneId) : base($"An ID with the number '{sceneId}' has not been found.") { }

        /// <summary>
        /// Creates a new instance of <see cref="IdNotFoundException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public IdNotFoundException(string message) : base(message) { }
        #endregion
    }
}

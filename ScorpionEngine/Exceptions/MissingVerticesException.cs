// <copyright file="MissingVerticesException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;
    using KDScorpionEngine.Entities;

    /// <summary>
    /// Thrown when an entity is being attempted to be initialized with no setup vertices.  The <see cref="Entity"/>
    /// must have at least 3 or more vertices to be a physical 2D object.
    /// </summary>
    public class MissingVerticesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingVerticesException"/> class.
        /// </summary>
        public MissingVerticesException()
            : base($"An {nameof(Entity)} must have vertices and at least a total of 3 vertices.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingVerticesException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public MissingVerticesException(string message)
            : base(message)
        {
        }
    }
}

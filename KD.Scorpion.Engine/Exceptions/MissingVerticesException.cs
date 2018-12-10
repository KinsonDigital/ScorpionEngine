using KDScorpionEngine.Entities;
using System;

namespace KDScorpionEngine.Exceptions
{
    /// <summary>
    /// Thrown when an entity is being attempted to be initialized with no vertices setup.  The <see cref="Entity"/>
    /// must have at least 3 or more vertices to be a physical 2D object.
    /// </summary>
    public class MissingVerticesException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MissingVerticesException"/>.
        /// </summary>
        public MissingVerticesException() : base($"An {nameof(Entity)} must have vertices and at least a total of 3 vertices.")
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="MissingVerticesException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public MissingVerticesException(string message) : base(message)
        {
        }
        #endregion
    }
}

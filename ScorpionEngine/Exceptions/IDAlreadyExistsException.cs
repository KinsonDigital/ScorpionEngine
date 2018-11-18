using System;


namespace ScorpionEngine.Exceptions
{
    /// <summary>
    /// Thrown if an ID already exists.
    /// </summary>
    public class IdAlreadyExistsException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="IdAlreadyExistsException"/>.
        /// </summary>
        public IdAlreadyExistsException() : base("That id already exists.")
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="IdAlreadyExistsException"/>.
        /// </summary>
        /// <param name="sceneId">The id of the scene that already exists.</param>
        public IdAlreadyExistsException(int sceneId) : base($"The id '{sceneId}' already exists.")
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="IdAlreadyExistsException"/>.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public IdAlreadyExistsException(string message) : base(message)
        {
        }
        #endregion
    }
}

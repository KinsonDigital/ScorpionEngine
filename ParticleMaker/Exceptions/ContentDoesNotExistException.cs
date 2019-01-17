using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Thrown when a content item does not exist in the content directory.
    /// </summary>
    public class ContentDoesNotExistException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ContentDoesNotExistException"/>.
        /// </summary>
        public ContentDoesNotExistException() : base("The content item does not exist in the root content directory.")
        {
        }


        /// <summary>
        /// Creates a new instance of <see cref="ContentDoesNotExistException"/>.
        /// </summary>
        /// <param name="contentItemName">The name of the content item that does not exist.</param>
        public ContentDoesNotExistException(string contentItemName) : base ($"The content item '{contentItemName}' does not exist in the root content directory.")
        {
        }
        #endregion
    }
}

using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Thrown when a file name is illegal.
    /// </summary>
    public class IllegalFileNameCharactersException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="IllegalFileNameCharactersException"/>.
        /// </summary>
        public IllegalFileNameCharactersException() : base("The file name contains illegal characters.  The following characters are not aloud. \n'\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.'") { }
        #endregion
    }
}

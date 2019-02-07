using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Holds exception information about the use of illegal characters in a file name.
    /// </summary>
    public class IllegalFileNameCharactersException : Exception
    {
        /// <summary>
        /// Creates a new exception of <see cref="IllegalFileNameCharactersException"/>.
        /// </summary>
        public IllegalFileNameCharactersException() : base("The file name contains illegal characters.  The following characters are not aloud. \n'\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.'")
        {
        }
    }
}

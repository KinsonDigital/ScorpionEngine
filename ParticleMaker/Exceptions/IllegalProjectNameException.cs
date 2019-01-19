using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Represents an exception where an illegal project name was attempted.
    /// </summary>
    public class IllegalProjectNameException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="IllegalProjectNameException"/>.
        /// </summary>
        public IllegalProjectNameException() : base("Illegal project name.  Cannot not use characters \\/:*?\"<>|")
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="IllegalProjectNameException"/>.
        /// </summary>
        /// <param name="name">The name of the project</param>
        public IllegalProjectNameException(string name) : base($"The project '{name}'.  Cannot not use characters \\/:*?\"<>|")
        {

        }
        #endregion
    }
}

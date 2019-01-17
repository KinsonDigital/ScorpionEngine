using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Holds information about an exception when a project does not exist.
    /// </summary>
    public class ProjectDoesNotExistExistException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectDoesNotExistExistException"/>.
        /// </summary>
        public ProjectDoesNotExistExistException() : base("The project does not exist.")
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ProjectDoesNotExistExistException"/>.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        public ProjectDoesNotExistExistException(string projectName) : base($"The project '{projectName}' does not exist.")
        {
        }
        #endregion
    }
}

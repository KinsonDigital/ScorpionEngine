using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Thrown when a project does not exist.
    /// </summary>
    public class ProjectDoesNotExistException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectDoesNotExistException"/>.
        /// </summary>
        public ProjectDoesNotExistException() : base("The project does not exist.") { }


        /// <summary>
        /// Creates a new instance of <see cref="ProjectDoesNotExistException"/>.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        public ProjectDoesNotExistException(string projectName) : base($"The project '{projectName}' does not exist.") { }
        #endregion
    }
}

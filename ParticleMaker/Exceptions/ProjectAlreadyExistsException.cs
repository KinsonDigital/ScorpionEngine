using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Thrown when a project already exsits.
    /// </summary>
    public class ProjectAlreadyExistsException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectAlreadyExistsException"/>.
        /// </summary>
        public ProjectAlreadyExistsException() : base("The project already exists.") { }


        /// <summary>
        /// Creates a new instance of <see cref="ProjectAlreadyExistsException"/>.
        /// </summary>
        /// <param name="projectName">The name of the project.</param>
        public ProjectAlreadyExistsException(string projectName) : base($"The project '{projectName}' already exists.") { }
        #endregion
    }
}

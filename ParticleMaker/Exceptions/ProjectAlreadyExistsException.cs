using System;

namespace ParticleMaker.Exceptions
{
    /// <summary>
    /// Holds information about an exception where a project trying to be created already exsits.
    /// </summary>
    public class ProjectAlreadyExistsException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ProjectAlreadyExistsException"/>.
        /// </summary>
        public ProjectAlreadyExistsException() : base("The project already exists.")
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ProjectAlreadyExistsException"/>.
        /// </summary>
        /// <param name="projectName">The name of the project to create.</param>
        public ProjectAlreadyExistsException(string projectName) : base($"The project '{projectName}' already exists.")
        {
        }
        #endregion
    }
}

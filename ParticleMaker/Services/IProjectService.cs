namespace ParticleMaker.Services
{
    /// <summary>
    /// Manages projects.
    /// </summary>
    public interface IProjectService
    {
        #region Props
        /// <summary>
        /// Gets the list of projects.
        /// </summary>
        string[] Projects { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Creates a new project using the given name.
        /// </summary>
        /// <param name="name">The name of the project to create.</param>
        void Create(string name);


        /// <summary>
        /// Deletes the project that matches the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the project to delete.</param>
        void Delete(string name);


        /// <summary>
        /// Renames the project with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newName">The new name to give the project.</param>
        void Rename(string name, string newName);
        #endregion
    }
}
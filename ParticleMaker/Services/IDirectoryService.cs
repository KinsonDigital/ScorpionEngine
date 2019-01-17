namespace ParticleMaker.Services
{
    /// <summary>
    /// Manages directories using such operations such as create and exists.
    /// </summary>
    public interface IDirectoryService
    {
        #region Methods
        /// <summary>
        /// Creates a new project using the given <paramref name="folder"/>.
        /// NOTE: Projects must have a unique name in the projects root folder and names must follow
        /// the OS directory naming rules.
        /// </summary>
        /// <param name="folder">The name of the project.</param>
        void Create(string folder);


        /// <summary>
        /// Returns a value indicating if the given folder <paramref name="folder"/> exists.
        /// </summary>
        /// <param name="folder">The folder path to check for.</param>
        /// <returns></returns>
        bool Exists(string folder);


        /// <summary>
        /// Deletes the given folder path and all of its contents.
        /// </summary>
        /// <param name="folder">The folder to delete.</param>
        /// <returns></returns>
        void Delete(string folder);


        #endregion
    }
}
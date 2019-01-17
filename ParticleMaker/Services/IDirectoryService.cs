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
        /// Returns a value indicating if the given folder <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The folder path to check for.</param>
        /// <returns></returns>
        bool Exists(string path);
        #endregion
    }
}
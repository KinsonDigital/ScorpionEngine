namespace ParticleMaker.Services
{
    /// <summary>
    /// Manages directories using such operations such as create and exists.
    /// </summary>
    public interface IDirectoryService
    {
        #region Methods
        /// <summary>
        /// Creates a new project using the given <paramref name="path"/>.
        /// NOTE: Projects must have a unique name in the projects root directory and names must follow
        /// the OS directory naming rules.
        /// </summary>
        /// <param name="path">The name of the project.</param>
        void Create(string folder);


        /// <summary>
        /// Returns a value indicating if the given <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The directory path to check for.</param>
        /// <returns></returns>
        bool Exists(string folder);


        /// <summary>
        /// Deletes the given directory path and all of its contents.
        /// </summary>
        /// <param name="path">The directory to delete.</param>
        /// <returns></returns>
        void Delete(string folder);


        /// <summary>
        /// Renames the given directory at the <paramref name="path"/> to the new given directory name.
        /// </summary>
        /// <param name="path">The directory to rename</param>
        /// <param name="newName">The new name to give the directory.</param>
        void Rename(string folder, string newName);


        /// <summary>
        /// Returns a list of directories at the given path.
        /// </summary>
        /// <param name="path">The directory path of where to get the list of directories from.</param>
        /// <returns></returns>
        string[] GetDirectories(string folder);
        #endregion
    }
}
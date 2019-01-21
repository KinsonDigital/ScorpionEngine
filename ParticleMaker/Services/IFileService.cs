namespace ParticleMaker.Services
{
    /// <summary>
    /// Creates JSON files with data.
    /// </summary>
    public interface IFileService
    {
        #region Methods
        /// <summary>
        /// Creates a file at the given <paramref name="path"/> with the given <paramref name="data"/>.
        /// </summary>
        /// <typeparam name="T">THe type of data to save.</typeparam>
        /// <param name="path">The directory path to the file.</param>
        /// <param name="data">The data to save in the file.</param>
        void Create<T>(string path, T data) where T : class;


        /// <summary>
        /// Loads a file at the given <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="T">The type of data to load from the file.</typeparam>
        /// <param name="path">The directory path to the file.</param>
        /// <returns>The data of type <typeparamref name="T"/>.</returns>
        T Load<T>(string path) where T : class;


        /// <summary>
        /// Returns a value indicating if the file at the given <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">The path to the file to check for.</param>
        /// <returns></returns>
        bool Exists(string path);
        #endregion
    }
}
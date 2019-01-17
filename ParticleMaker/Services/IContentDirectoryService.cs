namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides content directory management and content item checking.
    /// </summary>
    public interface IContentDirectoryService
    {
        #region Props
        /// <summary>
        /// The root directory where the texture to load is located.
        /// </summary>
        string ContentRootDirectory { get; set; }
        #endregion


        #region Methods
        /// <summary>
        /// Returns a value indicating if a content item with the given name exists in the <see cref="ContentRootDirectory"/>.
        /// </summary>
        /// <param name="itemName">The name of the content item.</param>
        /// <returns></returns>
        bool ContentItemExists(string itemName);
        #endregion
    }
}
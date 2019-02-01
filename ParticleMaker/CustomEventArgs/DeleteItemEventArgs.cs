namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about the event of renaming an item.
    /// </summary>
    public class DeleteItemEventArgs
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of <see cref="DeleteItemEventArgs"/>.
        /// </summary>
        /// <param name="name">The name of the item being deleted.</param>
        /// <param name="path">The path to the item.</param>
        public DeleteItemEventArgs(string name, string path)
        {
            Name = name;
            Path = path;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path to the item.
        /// </summary>
        public string Path { get; set; }
        #endregion
    }
}

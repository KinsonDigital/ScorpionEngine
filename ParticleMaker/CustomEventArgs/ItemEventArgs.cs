namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about an event of an item.
    /// </summary>
    public class ItemEventArgs
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of <see cref="ItemEventArgs"/>.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="path">The path to the item.</param>
        public ItemEventArgs(string name, string path)
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

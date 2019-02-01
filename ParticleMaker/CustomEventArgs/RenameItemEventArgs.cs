namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about the event of renaming an item.
    /// </summary>
    public class RenameItemEventArgs
    {
        #region Constructors
        /// <summary>
        /// Create a new instance of <see cref="RenameItemEventArgs"/>.
        /// </summary>
        /// <param name="name">The name of the item being renamed.</param>
        /// <param name="path">The path to the item.</param>
        public RenameItemEventArgs(string name, string path)
        {
            OldName = name;
            OldPath = path;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string OldName { get; set; }

        /// <summary>
        /// Gets or sets the new name.
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// Gets or sets the old path to the item.
        /// </summary>
        public string OldPath { get; set; }

        /// <summary>
        /// Gets or sets the new path to the item.
        /// </summary>
        public string NewPath { get; set; }
        #endregion
    }
}

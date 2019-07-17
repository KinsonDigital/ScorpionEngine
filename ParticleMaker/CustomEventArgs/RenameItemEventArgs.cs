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
        /// <param name="oldName">The name of the item being renamed.</param>
        /// <param name="oldPath">The path to the item.</param>
        public RenameItemEventArgs(string oldName, string oldPath)
        {
            OldName = oldName;
            OldPath = oldPath;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the old name of the item.
        /// </summary>
        public string OldName { get; set; }

        /// <summary>
        /// Gets or sets the new name of the item.
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

namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about event when an add setup has been clicked.
    /// </summary>
    public class AddItemClickedEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="AddItemClickedEventArgs"/>.
        /// </summary>
        /// <param name="itemName">The name of the item to add.</param>
        public AddItemClickedEventArgs(string itemName)
        {
            ItemName = itemName;
        }


        #region Props
        /// <summary>
        /// Gets or sets the name of the item being added.
        /// </summary>
        public string ItemName { get; internal set; }
        #endregion
    }
}

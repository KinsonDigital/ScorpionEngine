namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about event when an add setup has been clicked.
    /// </summary>
    public class AddSetupClickedEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="AddSetupClickedEventArgs"/>.
        /// </summary>
        /// <param name="setupName">The name of the setup to add.</param>
        public AddSetupClickedEventArgs(string setupName)
        {
            SetupName = setupName;
        }


        #region Props
        /// <summary>
        /// Gets or sets the name of the setup being added.
        /// </summary>
        public string SetupName { get; internal set; }
        #endregion
    }
}

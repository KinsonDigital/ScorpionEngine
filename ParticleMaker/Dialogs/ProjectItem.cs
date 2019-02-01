namespace ParticleMaker.Dialogs
{
    /// <summary>
    /// Represents a single project item with an exists indication value.
    /// </summary>
    public class ProjectItem
    {
        #region Props
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the project with the given <see cref="Name"/> exists.
        /// </summary>
        public bool Exists { get; set; }
        #endregion
    }
}

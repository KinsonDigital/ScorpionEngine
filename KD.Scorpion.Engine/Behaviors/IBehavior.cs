namespace KDScorpionEngine.Behaviors
{
    /// <summary>
    /// A simple behavior with an enabled state and name.
    /// </summary>
    public interface IBehavior : IUpdatable
    {
        #region Props
        /// <summary>
        /// Enables or disables the <see cref="IBehavior"/>.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="IBehavior"/>.
        /// </summary>
        string Name { get; set; }
        #endregion
    }
}
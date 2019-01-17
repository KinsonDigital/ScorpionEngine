namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about when a value has changed.
    /// </summary>
    public class ValueChangedEventArgs
    {
        #region Props
        /// <summary>
        /// Gets the old value before the change.
        /// </summary>
        public float OldValue { get; internal set; }

        /// <summary>
        /// Gets the new value after the change.
        /// </summary>
        public float NewValue { get; internal set; }
        #endregion
    }
}
namespace KDScorpionCore
{
    /// <summary>
    /// Provides the ability for an object to be updated.
    /// </summary>
    public interface IUpdatable
    {
        #region Methods
        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <param name="engineTime">The amount of time that has passed in the engine since the last frame.</param>
        void Update(EngineTime engineTime);
        #endregion
    }
}

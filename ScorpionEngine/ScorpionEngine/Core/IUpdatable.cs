namespace ScorpionEngine.Core
{
    /// <summary>
    /// Provides functionality to update a game related object.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Updates game related objects using the given frame time information.
        /// </summary>
        /// <param name="gameTime">The frame information of the last frame.</param>
        void Update(IEngineTiming gameTime);
    }
}

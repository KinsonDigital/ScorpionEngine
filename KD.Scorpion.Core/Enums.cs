namespace KDScorpionCore
{
    /// <summary>
    /// Represents different ways that an <see cref="IScene"/> should run.
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        /// This makes an <see cref="IScene"/> run continously.  Used for standard game running through frames.
        /// </summary>
        Continuous = 1,

        /// <summary>
        /// This gives fine control to run the game a set amount of frames at a time.
        /// </summary>
        FrameStack = 2
    }
}

namespace KDScorpionEngine.Entities
{
    /// <summary>
    /// Represents the direction that an animation is running.
    /// </summary>
    public enum AnimationDirection
    {
        Forward = 1,
        Backward = 2
    }

    /// <summary>
    /// The state of an animation.
    /// </summary>
    public enum AnimationState
    {
        Running = 1,
        Stopped = 2,
        Paused = 3
    }
}

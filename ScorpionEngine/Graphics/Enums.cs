// <copyright file="Enums.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    /// <summary>
    /// The direction that an animation can run.
    /// </summary>
    public enum AnimateDirection
    {
        /// <summary>
        /// Runs an animation in the forward direction.
        /// </summary>
        Forward,

        /// <summary>
        /// Runs an animation in the reverse direction.
        /// </summary>
        Reverse,
    }

    /// <summary>
    /// The state of an animation.
    /// </summary>
    public enum AnimateState
    {
        /// <summary>
        /// The animation is currently playing.
        /// </summary>
        Playing,

        /// <summary>
        /// The animation is currently paused.
        /// </summary>
        Paused,
    }
}

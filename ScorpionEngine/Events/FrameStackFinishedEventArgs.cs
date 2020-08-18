// <copyright file="FrameStackFinishedEventArgs.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Events
{
    /// <summary>
    /// Information about when a complete stack of frames finishes.
    /// </summary>
    public class FrameStackFinishedEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="FrameStackFinishedEventArgs"/>.
        /// </summary>
        /// <param name="totalFramesRan">The total amount of frames that ran in the last frame stack.</param>
        public FrameStackFinishedEventArgs(int totalFramesRan) => TotalFramesRan = totalFramesRan;

        /// <summary>
        /// Gets the total number of frames that has ran.
        /// </summary>
        public int TotalFramesRan { get; set; }
    }
}

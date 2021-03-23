// <copyright file="IAnimator.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System.Collections.ObjectModel;
    using System.Drawing;

    /// <summary>
    /// Manages the animation of an entity.
    /// </summary>
    public interface IAnimator : IUpdatableObject
    {
        /// <summary>
        /// Gets or sets the current state of the animation.
        /// </summary>
        AnimateState CurrentState { get; set; }

        /// <summary>
        /// Gets or sets the direction that the animation is running in.
        /// </summary>
        AnimateDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation will loop from one end to another.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     If the animation <see cref="Direction"/> is set to <see cref="AnimateDirection.Forward"/>,
        ///     then the animation will loop from the last frame back to the first and keep runing.
        /// </para>
        /// <para>
        ///     If the animation <see cref="Direction"/> is set to <see cref="AnimateDirection.Reverse"/>,
        ///     then the animation will loop from the first frame back to the last and keep runing.
        /// </para>
        /// </remarks>
        bool IsLooping { get; set; }

        /// <summary>
        /// Gets or sets the frames per second that the animation will run at.
        /// </summary>
        float FPS { get; set; }

        /// <summary>
        /// Gets or sets the list of animation frames.
        /// </summary>
        ReadOnlyCollection<Rectangle> Frames { get; set; }

        /// <summary>
        /// Gets the current frame bounds in the animation.
        /// </summary>
        Rectangle CurrentFrameBounds { get; }

        /// <summary>
        /// Moves the animation to the next frame.
        /// </summary>
        /// <remarks>
        ///     Ignores the <see cref="Direction"/> setting and only move the animation forward.
        /// </remarks>
        void NextFrame();

        /// <summary>
        /// Moves the animation to the previous frame.
        /// </summary>
        /// <remarks>
        ///     Ignores the <see cref="Direction"/> setting and only move the animation in reverse.
        /// </remarks>
        void PreviousFrame();

        /// <summary>
        /// Sets the animation to an explicit frame.
        /// </summary>
        /// <remarks>
        ///     If the value is set to a value that is larger then the last frame,
        ///     it will automatically be set to the last frame.
        ///
        /// <para>
        ///     This ignores the <see cref="Direction"/> setting.
        /// </para>
        /// </remarks>
        /// <param name="frameIndex">The index of the frame to set the animation to.</param>
        void SetFrame(uint frameIndex);
    }
}

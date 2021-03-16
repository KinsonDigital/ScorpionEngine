// <copyright file="Animator.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;

    public class Animator : IAnimator
    {
        private uint frameIndex;
        private float elapsedFrameTime;
        private float fps = 30f;
        private float fpsMilliseconds = 32f;
        private Rectangle[] frames = Array.Empty<Rectangle>();

        /// <inheritdoc/>
        public AnimateState CurrentState { get; set; } = AnimateState.Playing;

        /// <inheritdoc/>
        public AnimateDirection Direction { get; set; } = AnimateDirection.Forward;

        /// <inheritdoc/>
        public bool IsLooping { get; set; } = true;

        /// <inheritdoc/>
        public float FPS
        {
            get => this.fps;
            set
            {
                this.fps = value;
                this.fpsMilliseconds = 1000f / value;
            }
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<Rectangle> Frames
        {
            get => new ReadOnlyCollection<Rectangle>(this.frames);
            set => this.frames = value.ToArray();
        }

        /// <inheritdoc/>
        public Rectangle CurrentFrameBounds => this.frames[this.frameIndex];

        /// <inheritdoc/>
        public void NextFrame()
        {
            if (CurrentState == AnimateState.Paused)
            {
                return;
            }

            CheckFrames();

            this.frameIndex += 1;

            ProcessIndexRange();
        }

        /// <inheritdoc/>
        public void PreviousFrame()
        {
            if (CurrentState == AnimateState.Paused)
            {
                return;
            }

            CheckFrames();

            // NOTE: Since this is an unsigned integer, if the value attempts to go
            // negative, it will automatically be set to the max value of a uint
            this.frameIndex -= 1;

            ProcessIndexRange();
        }

        /// <inheritdoc/>
        public void SetFrame(uint frameIndex)
        {
            ProcessIndexRange();
            this.frameIndex = frameIndex;
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            if (CurrentState == AnimateState.Paused)
            {
                return;
            }

            CheckFrames();

            this.elapsedFrameTime += gameTime.CurrentFrameElapsed;

            if (this.elapsedFrameTime >= this.fpsMilliseconds)
            {
                this.elapsedFrameTime = 0;

                switch (Direction)
                {
                    case AnimateDirection.Forward:
                        this.frameIndex += 1;
                        break;
                    case AnimateDirection.Reverse:
                        this.frameIndex -= 1;
                        break;
                    default:
                        throw new Exception($"Unknown '{nameof(AnimateDirection)}' value of '{(int)Direction}'.");
                }

                ProcessIndexRange();
            }
        }

        /// <summary>
        /// Processes the current frame index by keeping it withing a valid range
        /// as well as performing looping of the animation.
        /// </summary>
        private void ProcessIndexRange()
        {
            if (this.frameIndex >= uint.MaxValue)
            {
                this.frameIndex = IsLooping ? (uint)this.frames.Length - 1u : 0u;
            }

            if (this.frameIndex > Frames.Count - 1u)
            {
                this.frameIndex = IsLooping ? 0u : (uint)this.frames.Length - 1u;
            }
        }

        /// <summary>
        /// Checks the frames and if no frames exist, throws an exception.
        /// </summary>
        /// <exception cref="Exception">Thrown when no exception exists.</exception>
        private void CheckFrames()
        {
            if (this.frames.Length <= 0)
            {
                // TODO: Create custom no frames exception
                throw new Exception("No frames exist in the animation.");
            }
        }
    }
}

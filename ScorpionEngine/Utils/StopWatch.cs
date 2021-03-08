// <copyright file="StopWatch.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Utils
{
    using System;

    /// <summary>
    /// Keeps track of time passed and invokes events when that time has passed.
    /// </summary>
    public class StopWatch : IStopWatch
    {
        private bool enabled;
        private int timeOut;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? TimeElapsed;

        /// <inheritdoc/>
        public int TimeOut
        {
            get => this.timeOut;
            set => this.timeOut = value < 0 ? 1 : value;
        }

        /// <inheritdoc/>
        public int ElapsedMS { get; private set; }

        /// <inheritdoc/>
        public float ElapsedSeconds => ElapsedMS / 1000.0f;

        /// <inheritdoc/>
        public bool Running { get; private set; }

        /// <inheritdoc/>
        public ResetType ResetMode { get; set; } = ResetType.Auto;

        /// <inheritdoc/>
        public void Start()
        {
            this.enabled = true;
            Running = true;
        }

        /// <inheritdoc/>
        public void Stop()
        {
            this.enabled = false;
            Running = false;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            Stop();
            ElapsedMS = 0;
        }

        /// <summary>
        /// Updates the internal time of the stop watch.
        /// </summary>
        /// <param name="frameTime">The game engine time.</param>
        public void Update(GameTime frameTime)
        {
            // If the stopwatch is enabled, add the amount of time passed to the elapsed value
            if (this.enabled)
            {
                ElapsedMS += frameTime.CurrentFrameElapsed;
            }

            // If the timeout has been reached
            if (ElapsedMS < this.timeOut)
            {
                return;
            }

            TimeElapsed?.Invoke(this, EventArgs.Empty);

            // If the reset mode is set to auto, reset the elapsed time back to 0
            if (ResetMode == ResetType.Auto)
            {
                Reset();
            }
        }
    }
}

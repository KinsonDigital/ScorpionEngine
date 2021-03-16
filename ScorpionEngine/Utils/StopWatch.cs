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
        private int timeOut;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? TimeElapsed;

        /// <inheritdoc/>
        public int ElapsedMS { get; private set; }

        /// <inheritdoc/>
        public float ElapsedSeconds => ElapsedMS / 1000.0f;

        /// <inheritdoc/>
        public ResetType ResetMode { get; set; } = ResetType.Auto;

        /// <inheritdoc/>
        public bool Running { get; private set; }

        /// <inheritdoc/>
        public int TimeOut
        {
            get => this.timeOut;
            set => this.timeOut = value < 0 ? 1 : value;
        }

        /// <inheritdoc/>
        public bool StopOnReset { get; set; }

        /// <inheritdoc/>
        public void Reset()
        {
            if (StopOnReset)
            {
                Stop();
            }

            ElapsedMS = 0;
        }

        /// <inheritdoc/>
        public void Start() => Running = true;

        /// <inheritdoc/>
        public void Stop() => Running = false;

        /// <inheritdoc/>
        public void Restart()
        {
            Stop();
            Reset();
            Start();
        }

        /// <summary>
        /// Updates the internal time of the stop watch.
        /// </summary>
        /// <param name="frameTime">The game engine time.</param>
        public void Update(GameTime frameTime)
        {
            // If the stopwatch is running, add the amount of time passed to the elapsed value
            if (Running)
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

// <copyright file="StopWatch.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Utils
{
    using System;
    using Raptor;

    /// <summary>
    /// Keeps track of time passed and invokes events when that time has passed.
    /// </summary>
    public class StopWatch : IUpdatable
    {
        private bool enabled;
        private int timeOut;

        /// <summary>
        /// Initializes a new instance of the <see cref="StopWatch"/> class.
        /// </summary>
        /// <param name="timeOut">The amount of time in milliseconds before the stopWatch OnTimeElapsed event is invoked.</param>
        public StopWatch(int timeOut) => this.timeOut = timeOut;

        /// <summary>
        /// Occurs every time the stop watch reaches 0.
        /// </summary>
        public event EventHandler OnTimeElapsed;

        /// <summary>
        /// Gets or sets the amount of time in milliseconds before the stopwatch will invoke the OnTimeElapsed event.
        /// NOTE: If a timeout of less then 1 is attempted, timeout will default to 1.
        /// </summary>
        public int TimeOut
        {
            get => this.timeOut;
            set => this.timeOut = value < 0 ? 1 : value;
        }

        /// <summary>
        /// Gets the amount of time passed in milliseconds.
        /// </summary>
        public int ElapsedMS { get; private set; }

        /// <summary>
        /// Gets the amount of time passed in seconds.
        /// </summary>
        public float ElapsedSeconds => ElapsedMS / 1000.0f;

        /// <summary>
        /// Gets a value indicating whether the stopwatch is running.
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// Gets or sets the reset mode of the stopwatch.  If set to auto reset, then the stopwatch will automatically be set to 0 and start counting again.
        /// </summary>
        public ResetType ResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Starts the stopwatch.
        /// </summary>
        public void Start()
        {
            this.enabled = true;
            Running = true;
        }

        /// <summary>
        /// Stops the stopwatch.
        /// </summary>
        public void Stop()
        {
            this.enabled = false;
            Running = false;
        }

        /// <summary>
        /// Stops the stopwatch and resets the elapsed time back to 0.
        /// </summary>
        public void Reset()
        {
            Stop();
            ElapsedMS = 0;
        }

        /// <summary>
        /// Updates the internal time of the stop watch.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public void Update(EngineTime engineTime)
        {
            // If the stopwatch is enabled, add the amount of time passed to the elapsed value
            if (this.enabled)
                ElapsedMS += (int)engineTime.ElapsedEngineTime.TotalMilliseconds;

            // If the timeout has been reached
            if (ElapsedMS < this.timeOut) return;

            OnTimeElapsed?.Invoke(this, new EventArgs());

            // If the reset mode is set to auto, reset the elapsed time back to 0
            if (ResetMode == ResetType.Auto)
                Reset();
        }
    }
}

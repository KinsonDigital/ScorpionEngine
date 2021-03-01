// <copyright file="IStopWatch.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Utils
{
    using System;

    /// <summary>
    /// Keeps track of time passed and invokes events when that time has passed.
    /// </summary>
    public interface IStopWatch: IUpdatableObject
    {
        /// <summary>
        /// Occurs every time the stop watch reaches 0.
        /// </summary>
        event EventHandler<EventArgs>? OnTimeElapsed;

        /// <summary>
        /// Gets the amount of time passed in milliseconds.
        /// </summary>
        int ElapsedMS { get; }

        /// <summary>
        /// Gets the amount of time passed in seconds.
        /// </summary>
        float ElapsedSeconds { get; }

        /// <summary>
        /// Gets or sets the reset mode of the stopwatch.  If set to auto reset, then the stopwatch will automatically be set to 0 and start counting again.
        /// </summary>
        ResetType ResetMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the stopwatch is running.
        /// </summary>
        bool Running { get; }

        /// <summary>
        /// Gets or sets the amount of time in milliseconds before the stopwatch will invoke the OnTimeElapsed event.
        /// NOTE: If a timeout of less then 1 is attempted, timeout will default to 1.
        /// </summary>
        int TimeOut { get; set; }

        /// <summary>
        /// Stops the stopwatch and resets the elapsed time back to 0.
        /// </summary>
        void Reset();

        /// <summary>
        /// Starts the stopwatch.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the stopwatch.
        /// </summary>
        void Stop();
    }
}

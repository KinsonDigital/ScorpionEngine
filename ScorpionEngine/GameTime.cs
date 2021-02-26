// <copyright file="GameTime.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    /// <summary>
    /// Represents the total amount of time that the game has ran in various units of time
    /// as well as the length of time for the most recent frame.
    /// </summary>
    public class GameTime
    {
        private long totalMilliseconds;

        /// <summary>
        /// Gets the amount of time that has passed for the current frame in milliseconds.
        /// </summary>
        public int CurrentFrameElapsed { get; private set; }

        /// <summary>
        /// Gets the total amount of time that the game has ran in hours.
        /// </summary>
        public double TotalGameHours => TotalGameMinutes / 60.0;

        /// <summary>
        /// Gets the total amount of time that the game has ran in minutes.
        /// </summary>
        public double TotalGameMinutes => TotalGameSeconds / 60.0;

        /// <summary>
        /// Gets the total amount of time that the game has ran in seconds.
        /// </summary>
        public double TotalGameSeconds => this.totalMilliseconds / 1000.0;

        /// <summary>
        /// Gets the total amount of time that the game has ran in milliseconds.
        /// </summary>
        public double TotalGameMilliseconds => this.totalMilliseconds;

        /// <summary>
        /// Adds the given time in <paramref name="milliseconds"/>.
        /// </summary>
        /// <param name="milliseconds">The amount of time in milliseconds to add.</param>
        internal void AddTime(int milliseconds)
        {
            CurrentFrameElapsed = milliseconds;
            this.totalMilliseconds += milliseconds;
        }
    }
}

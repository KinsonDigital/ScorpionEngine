// <copyright file="GameTime.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;

    public class GameTime
    {
        private readonly TimeSpan totalGameTime;

        /// <summary>
        /// Gets the amount of time that has passed for the current frame in milliseconds.
        /// </summary>
        public int CurrentFrameElapsed { get; private set; }

        public double TotalGameHours => this.totalGameTime.TotalHours;

        public double TotalGameMinutes => this.totalGameTime.TotalMinutes;

        public double TotalGameMilliseconds => this.totalGameTime.TotalMilliseconds;

        internal void UpdateTotalGameTime(int milliseconds)
        {
            CurrentFrameElapsed = milliseconds;
            this.totalGameTime.Add(new TimeSpan(0, 0, 0, 0, milliseconds));
        }
    }
}

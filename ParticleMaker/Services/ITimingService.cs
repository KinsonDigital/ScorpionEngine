using System;
using System.Collections.Generic;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides the ability to manage time that has passed and calculate frames per second.
    /// </summary>
    public interface ITimingService
    {
        #region Props
        /// <summary>
        /// Gets or sets a list of timings of frames.
        /// </summary>
        Queue<double> FrameTimings { get; set; }

        /// <summary>
        /// The total frame timings to store in the <see cref="FrameTimings"/>.
        /// </summary>
        int TotalFrameTimes { get; set; }

        /// <summary>
        /// The total milliseconds that has passed since the timing sertice was started.
        /// </summary>
        float TotalMilliseconds { get; }

        /// <summary>
        /// The amount of time that has elapsed since the timing sertice was started.
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// The total frames per second that the timing sertice is running at.
        /// </summary>
        float FPS { get; }

        /// <summary>
        /// The amount of time to wait before continuing.
        /// </summary>
        int WaitTime { get; set; }

        /// <summary>
        /// Gets a value indicating if the timer is paused.
        /// </summary>
        bool IsPaused { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Starts the timing service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the timing service.
        /// </summary>
        void Stop();


        /// <summary>
        /// Pauses the timing service.
        /// </summary>
        void Pause();

        /// <summary>
        /// Tells the timing service to wait.
        /// </summary>
        void Wait();

        /// <summary>
        /// Records an amount of time and calculates the frames per second.
        /// </summary>
        void Record();
        #endregion
    }
}

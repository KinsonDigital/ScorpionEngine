using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace ParticleMaker.Services
{
    /// <summary>
    /// Provides the ability to manage time that has passed and calculate frames per second.
    /// </summary>
    public class TimingService : ITimingService
    {
        #region Private Fields
        private readonly Stopwatch _timer = new Stopwatch();
        #endregion


        #region Props
        /// <summary>
        /// The total milliseconds that has passed since the timing sertice was started.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public float TotalMilliseconds => (float)_timer.Elapsed.TotalMilliseconds;

        /// <summary>
        /// The amount of time that has elapsed since the timing sertice was started.
        /// </summary>
        public TimeSpan Elapsed => new TimeSpan(0, 0, 0, 0, (int)TotalMilliseconds);

        /// <summary>
        /// Gets or sets a list of timings of frames.
        /// </summary>
        public Queue<double> FrameTimings { get; set; } = new Queue<double>();

        /// <summary>
        /// The total frame timings to store in the <see cref="FrameTimings"/>.
        /// </summary>
        public int TotalFrameTimes { get; set; } = 100;

        /// <summary>
        /// The total frames per second that the timing sertice is running at.
        /// </summary>
        public float FPS { get; private set; }

        /// <summary>
        /// The amount of time to wait before continuing.
        /// </summary>
        public int WaitTime { get; set; } = 1000;

        /// <summary>
        /// Gets a value indicating if the timer is paused.
        /// </summary>
        public bool IsPaused => !_timer.IsRunning;
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the timing service.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Start() => _timer.Start();


        /// <summary>
        /// Stops the timing service.
        /// </summary>   [ExcludeFromCodeCoverage]
        [ExcludeFromCodeCoverage]
        public void Stop() => _timer.Stop();


        /// <summary>
        /// Pauses the timing service.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Pause() => _timer.Stop();


        /// <summary>
        /// Tells the timing service to wait.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Wait() => Thread.Sleep(WaitTime);


        /// <summary>
        /// Records an amount of time and calculates the frames per second.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Record()
        {
            _timer.Stop();

            FrameTimings.Enqueue(_timer.Elapsed.TotalMilliseconds);

            if (FrameTimings.Count >= TotalFrameTimes + 1)
                FrameTimings.Dequeue();

            FPS = (float)Math.Round(1000f / FrameTimings.Average(), 2);

            _timer.Restart();
        }
        #endregion
    }
}

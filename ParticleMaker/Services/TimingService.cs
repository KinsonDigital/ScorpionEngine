using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParticleMaker.Services
{
    public class TimingService : ITimingService
    {
        #region Private Fields
        private readonly Stopwatch _timer = new Stopwatch();
        #endregion


        #region Props
        public float TotalMilliseconds => (float)_timer.Elapsed.TotalMilliseconds;

        public TimeSpan Elapsed => new TimeSpan(0, 0, 0, 0, (int)TotalMilliseconds);

        public Queue<double> FrameTimings { get; set; } = new Queue<double>();

        public int TotalFrameTimes { get; set; } = 100;

        public float FPS { get; private set; }

        public int WaitTime { get; set; } = 1000;

        public bool IsPaused => !_timer.IsRunning;
        #endregion


        #region Public Methods
        public void Start() => _timer.Start();


        public void Stop() => _timer.Stop();


        public void Pause() => _timer.Stop();


        public void Wait() => Thread.Sleep(WaitTime);


        public void Record()
        {
            _timer.Stop();

            FrameTimings.Enqueue(_timer.Elapsed.TotalMilliseconds);

            if (FrameTimings.Count >= TotalFrameTimes)
                FrameTimings.Dequeue();

            FPS = (float)Math.Round(1000f / FrameTimings.Average(), 2);

            _timer.Restart();
        }
        #endregion
    }
}

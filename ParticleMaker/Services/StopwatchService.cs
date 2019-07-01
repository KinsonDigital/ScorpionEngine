using System;
using System.Diagnostics;

namespace ParticleMaker.Services
{
    public class StopWatchService : IStopWatchService
    {
        #region Private Fields
        private Stopwatch _timer;
        #endregion


        #region Props
        public float TotalMilliseconds => _timer == null ? 0 : (float)_timer.Elapsed.TotalMilliseconds;

        public TimeSpan Elapsed => _timer == null ? new TimeSpan() : new TimeSpan(0, 0, 0, 0, (int)TotalMilliseconds);
        #endregion


        #region Public Methods
        public void Start()
        {
            if (_timer == null)
                _timer = new Stopwatch();

            _timer.Start();
        }


        public void Stop()
        {
            if (_timer == null)
                _timer = new Stopwatch();

            _timer.Stop();
        }


        public void Restart()
        {
            if (_timer == null)
                _timer = new Stopwatch();

            _timer.Restart();
        }
        #endregion
    }
}

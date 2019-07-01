using System;

namespace ParticleMaker.Services
{
    public interface IStopWatchService
    {
        #region Props
        float TotalMilliseconds { get; }

        TimeSpan Elapsed { get; }
        #endregion


        #region Methods
        void Start();


        void Stop();


        void Restart();
        #endregion
    }
}

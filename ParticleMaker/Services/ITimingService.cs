using System;
using System.Collections.Generic;

namespace ParticleMaker.Services
{
    public interface ITimingService
    {
        #region Props
        Queue<double> FrameTimings { get; set; }

        int TotalFrameTimes { get; set; }

        float TotalMilliseconds { get; }

        TimeSpan Elapsed { get; }

        float FPS { get; }

        int WaitTime { get; set; }

        bool IsPaused { get; }
        #endregion


        #region Methods
        void Start();


        void Pause();


        void Wait();


        void Record();
        #endregion
    }
}

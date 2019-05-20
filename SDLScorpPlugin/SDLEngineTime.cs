using KDScorpionCore;
using System;

namespace SDLScorpPlugin
{
    public class SDLEngineTime : IEngineTiming
    {
        #region Constructors
        public SDLEngineTime() { }


        public SDLEngineTime(TimeSpan totalEngineTime, TimeSpan elapsedEngineTime)
        {
            TotalEngineTime = totalEngineTime;
            ElapsedEngineTime = elapsedEngineTime;
        }
        #endregion


        #region Props
        public TimeSpan TotalEngineTime { get; set; }

        public TimeSpan ElapsedEngineTime { get; set; }
        #endregion
    }
}

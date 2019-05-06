using KDScorpionCore;
using System;

namespace SFMLScorpPlugin
{
    public class SFMLEngineTime : IEngineTiming
    {
        #region Constructors
        public SFMLEngineTime() { }


        public SFMLEngineTime(TimeSpan totalEngineTime, TimeSpan elapsedEngineTime)
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

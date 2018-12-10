using System;

namespace KDScorpionCore
{
    public interface IEngineTiming
    {
        TimeSpan TotalEngineTime { get; set; }

        TimeSpan ElapsedEngineTime { get; set; }
    }
}

using System;

namespace ScorpionCore
{
    public interface IEngineTiming
    {
        TimeSpan TotalEngineTime { get; set; }

        TimeSpan ElapsedEngineTime { get; set; }
    }
}

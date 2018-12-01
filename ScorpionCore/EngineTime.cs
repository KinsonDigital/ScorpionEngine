using System;
using System.Diagnostics.CodeAnalysis;

namespace ScorpionCore
{
    //TODO: Add code docs
    [ExcludeFromCodeCoverage]
    public class EngineTime : IEngineTiming
    {
        public TimeSpan TotalEngineTime { get; set; }

        public TimeSpan ElapsedEngineTime { get; set; }
    }
}

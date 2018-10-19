using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine
{
    //TODO: Add code docs
    [ExcludeFromCodeCoverage]
    public class EngineTime : IEngineTiming
    {
        public TimeSpan TotalEngineTime { get; set; }

        public TimeSpan ElapsedEngineTime { get; set; }
    }
}

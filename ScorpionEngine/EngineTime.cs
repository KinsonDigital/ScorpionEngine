using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine
{
    public class EngineTime : IEngineTiming
    {
        public TimeSpan TotalEngineTime { get; set; }

        public TimeSpan ElapsedEngineTime { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    public interface IEngineTiming
    {
        TimeSpan TotalEngineTime { get; set; }

        TimeSpan ElapsedEngineTime { get; set; }
    }
}
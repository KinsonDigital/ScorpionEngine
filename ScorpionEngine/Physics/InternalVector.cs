using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Physics
{
    internal struct InternalVector : IVector
    {
        public InternalVector(float x, float y)
        {
            X = x;
            Y = y;
        }


        public float X { get; set; }
        public float Y { get; set; }
    }
}

using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VelcroPhysicsPlugin
{
    public class VelcroVector : IVector
    {
        public VelcroVector(float x, float y)
        {
            X = x;
            Y = y;
        }


        public float X { get; set; }
        public float Y { get; set; }
    }
}

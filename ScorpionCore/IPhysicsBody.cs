using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore
{
    //TODO: Add docs
    public interface IPhysicsBody
    {
        Vector Position { get; set; }

        float Angle { get; set; }

        float Density { get; set; }

        float Friction { get; set; }

        float Restitution { get; set; }


        void DataSender(Func<dynamic> dataSender);
    }
}

using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Plugins
{
    //TODO: Add docs
    public interface IPhysicsBody
    {
        float X { get; set; }

        float Y { get; set; }

        float Angle { get; set; }

        float Density { get; set; }

        float Friction { get; set; }

        float Restitution { get; set; }


        void DataSender(Func<dynamic> dataSender);
    }
}

using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Plugins
{
    //TODO: Add docs
    public interface IPhysicsBody : IPlugin
    {
        #region Props
        float[] XVertices { get; set; }

        float[] YVertices { get; set; }

        float X { get; set; }

        float Y { get; set; }

        float Angle { get; set; }

        float Density { get; set; }

        float Friction { get; set; }

        float Restitution { get; set; }

        float LinearVelocityX { get; set; }

        float LinearVelocityY { get; set; }

        float AngularVelocity { get; set; }
        #endregion


        #region Methods
        void DataSender(Func<dynamic> dataSender);


        void ApplyLinearImpulse(float x, float y);

        void ApplyAngularImpulse(float value);
        #endregion
    }
}

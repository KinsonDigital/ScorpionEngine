using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionCore.Plugins
{
    public interface IDebugDraw : IPlugin
    {
        void Draw(IRenderer renderer, IPhysicsBody body);
    }
}

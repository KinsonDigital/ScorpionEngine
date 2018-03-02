using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine
{
    /// <summary>
    /// Makes an object and updatable update for the game engine.
    /// </summary>
    interface IUpdatable
    {
        void Update(EngineTime engineTime);
    }
}

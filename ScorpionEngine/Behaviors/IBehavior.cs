using ScorpionCore;
using System;

namespace ScorpionEngine.Behaviors
{
    public interface IBehavior : IUpdatable
    {
        bool Enabled { get; set; }

        string Name { get; set; }
    }
}
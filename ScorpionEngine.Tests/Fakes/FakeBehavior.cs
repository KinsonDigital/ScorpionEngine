using ScorpionEngine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Tests.Fakes
{
    public class FakeBehavior : Behavior
    {
        public bool UpdateActionInvoked { get; private set; }

        public FakeBehavior(bool setupAction)
        {
            if (!setupAction)
                return;

            SetUpdateAction((engineTime) =>
            {
                UpdateActionInvoked = true;
            });
        }
    }
}

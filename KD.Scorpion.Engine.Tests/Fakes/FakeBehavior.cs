using KDScorpionEngine.Behaviors;

namespace KDScorpionEngine.Tests.Fakes
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

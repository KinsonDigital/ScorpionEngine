using KDScorpionEngine.Behaviors;

namespace KDScorpionEngineTests.Fakes
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

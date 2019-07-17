using KDScorpionEngine.Behaviors;

namespace KDScorpionEngineTests.Fakes
{
    /// <summary>
    /// Provides a fake implementation of the <see cref="KDScorpionEngine.Behaviors.Behavior"/> abstract class.
    /// </summary>
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

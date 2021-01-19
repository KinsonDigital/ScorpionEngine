// <copyright file="FakeBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Fakes
{
    using KDScorpionEngine.Behaviors;

    /// <summary>
    /// Provides a fake implementation of the <see cref="KDScorpionEngine.Behaviors.Behavior"/> abstract class.
    /// </summary>
    public class FakeBehavior : Behavior
    {
        public FakeBehavior(bool setupAction)
        {
            if (!setupAction)
            {
                return;
            }

            SetUpdateAction((gameTime) =>
            {
                UpdateActionInvoked = true;
            });
        }

        public bool UpdateActionInvoked { get; private set; }
    }
}

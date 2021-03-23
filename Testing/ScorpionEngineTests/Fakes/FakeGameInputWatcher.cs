// <copyright file="FakeGameInputWatcher.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Fakes
{
    using KDScorpionEngine.Input;
    using KDScorpionEngine.Utils;
    using Raptor.Input;

    /// <summary>
    /// Used to test the abstract <see cref="GameInputWatcher{TInputs}"/> class.
    /// </summary>
    public class FakeGameInputWatcher : GameInputWatcher<KeyCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeGameInputWatcher"/> class.
        /// </summary>
        public FakeGameInputWatcher(
            IStopWatch inputDownTimer,
            IStopWatch inputReleaseTimer,
            ICounter counter)
                : base(inputDownTimer, inputReleaseTimer, counter)
        {
        }
    }
}

// <copyright file="FakeStopWatch.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Fakes
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Utils;

    /// <summary>
    /// Used to test the <see cref="StopWatch"/> class.
    /// </summary>
    public class FakeStopWatch : IStopWatch
    {
        public event EventHandler<EventArgs>? TimeElapsed;

        public int ElapsedMS { get; }

        public float ElapsedSeconds { get; }

        public ResetType ResetMode { get; set; }

        public bool Running { get; }

        public int TimeOut { get; set; }

        public bool StopOnReset { get; set; }

        public void Reset() { }

        public void Start() { }

        public void Stop() => throw new NotImplementedException();

        public void Update(GameTime gameTime)
             => TimeElapsed?.Invoke(null, EventArgs.Empty);

        public void OnTimeElapsed() => throw new NotImplementedException();

        public void Restart() => throw new NotImplementedException();
    }
}

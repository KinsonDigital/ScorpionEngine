using System;
using KDScorpionEngine;
using KDScorpionEngine.Utils;

namespace KDScorpionEngineTests.Fakes
{
    public class FakeStopWatch : IStopWatch
    {
        public int ElapsedMS { get; }

        public float ElapsedSeconds { get; }

        public ResetType ResetMode { get; set; }

        public bool Running { get; }

        public int TimeOut { get; set; }

        public event EventHandler<EventArgs> TimeElapsed;

        public void Reset() { }

        public void Start() { }

        public void Stop() => throw new NotImplementedException();

        public void Update(GameTime gameTime)
        {
            TimeElapsed?.Invoke(null, EventArgs.Empty);
        }
    }
}

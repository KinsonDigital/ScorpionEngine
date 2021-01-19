using System;

namespace KDScorpionEngine
{
    public class GameTime
    {
        private TimeSpan totalGameTime;

        public int CurrentFrameElapsed { get; private set; }

        public double TotalGameHours => this.totalGameTime.TotalHours;

        public double TotalGameMinutes => this.totalGameTime.TotalMinutes;

        public double TotalGameMilliseconds => this.totalGameTime.TotalMilliseconds;

        internal void UpdateTotalGameTime(int milliseconds) => totalGameTime.Add(new TimeSpan(0, 0, 0, 0, milliseconds));
    }
}

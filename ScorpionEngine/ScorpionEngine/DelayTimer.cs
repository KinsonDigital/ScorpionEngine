using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScorpionEngine
{
    /// <summary>
    /// Invokes a registered delegate after a set amount of time passes.
    /// </summary>
    public class DelayTimer
    {
        #region Fields
        private Timer _timer;//The timer to keep track of time.
        private int _delayTime = 1000;//The amount of time to wait until the TimeExpired event is invoked.
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the set time has expired.
        /// </summary>
        public EventHandler TimeExpired;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of DelayTimer.
        /// </summary>
        /// <param name="delayTime">The amount of time to delay.</param>
        public DelayTimer(int delayTime, TimerCallback callback)
        {
            _timer = new Timer(callback);
            _timer.Change(_delayTime, 0);
        }
        #endregion

        #region Properites
        /// <summary>
        /// Gets or sets the delay time.
        /// </summary>
        public int DelayTime
        {
            get { return _delayTime; }
            set
            {
                _delayTime = value;
                _timer.Change(_delayTime, 0);
            }
        }
        #endregion
    }
}

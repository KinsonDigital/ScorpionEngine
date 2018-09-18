using ScorpionEngine.Core;
using System;

namespace ScorpionEngine.Utils
{
    /// <summary>
    /// Keeps track of time passed and invokes events when that time has passed.
    /// </summary>
    public class StopWatch : IUpdatable
    {
        #region Events
        /// <summary>
        /// Occurs every time the stop watch reaches 0.
        /// </summary>
        public event EventHandler OnTimeElapsed;
        #endregion


        #region Fields
        private bool _enabled;
        private int _timeOut;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of stopwatch.
        /// </summary>
        /// <param name="timeOut">The amount of time in milliseconds before the stopWatch OnTimeElapsed event is invoked.</param>
        public StopWatch(int timeOut)
        {
            _timeOut = timeOut;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the amount of time before the stopwatch will invoke the OnTimeElapsed event.
        /// NOTE: If a timeout of less then 1 is attempted, timeout will default to 1.
        /// </summary>
        public int TimeOut
        {
            get
            {
                return _timeOut;
            }
            set
            {
                _timeOut = value < 0 ? 1 : value;
            }
        }

        /// <summary>
        /// Gets the amount of time passed in milliseconds.
        /// </summary>
        public int ElapsedMS { get; private set; }

        /// <summary>
        /// Gets the amount of time passed in seconds.
        /// </summary>
        public float ElapsedSeconds => ElapsedMS / 1000.0f;

        /// <summary>
        /// Gets a value indicating if the stopwatch is running.
        /// </summary>
        public bool Running { get; private set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Starts the stopwatch.
        /// </summary>
        public void Start()
        {
            _enabled = true;
            Running = true;
        }


        /// <summary>
        /// Stops the stopwatch.
        /// </summary>
        public void Stop()
        {
            _enabled = false;
            Running = false;
        } 


        /// <summary>
        /// Gets or sets the reset mode of the stopwatch.  If set to auto reset, then the stopwatch will automatically be set to 0 and start counting again.
        /// </summary>
        public ResetType ResetMode { get; set; }


        /// <summary>
        /// Stops the stopwatch and resets the elapsed time back to 0.
        /// </summary>
        public void Reset()
        {
            Stop();

            ElapsedMS = 0;
        } 


        /// <summary>
        /// Updates the internal time of the stop watch.
        /// </summary>
        /// <param name="engineTime">The engine time passed.</param>
        public void Update(IEngineTiming engineTime)
        {
            //If the stopwatch is enabled, add the amount of time passed to the elapsed value
            if (_enabled)
                ElapsedMS += engineTime.ElapsedEngineTime.Milliseconds;

            //If the timeout has been reached
            if (ElapsedMS < _timeOut) return;

            OnTimeElapsed?.Invoke(this, new EventArgs());

            //If the reset mode is set to auto, reset the elapsed time back to 0
            if (ResetMode == ResetType.Auto)
                Reset();
        }
        #endregion
    }
}
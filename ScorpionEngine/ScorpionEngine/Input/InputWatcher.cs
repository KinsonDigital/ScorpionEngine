using System;
using ScorpionEngine.Utils;

namespace ScorpionEngine.Input
{
    public abstract class InputWatcher
    {
        #region Events
        /// <summary>
        /// Occurs when the input has reached its hit count.
        /// </summary>
        public event EventHandler OnInputHitCountReached;

        /// <summary>
        /// Occurs when the input has been pressed for a set amount of time.
        /// </summary>
        public event EventHandler OnInputDownTimeOut;

        /// <summary>
        /// Occurs when the input has been released for a set amount of time after its release.
        /// </summary>
        public event EventHandler OnInputReleasedTimeOut;

        /// <summary>
        /// Occurs when the given input combo has been pressed.
        /// </summary>
        public event EventHandler OnInputComboPressed;
        #endregion


        #region Fields
        protected Counter _counter;//Keeps track of the hit count of an input
        protected bool _curState;//The current state of the set input
        protected bool _prevState;//The previous state of the set input
        protected bool _enabled;//Enables or disables the watcher
        protected StopWatch _inputDownTimer;//Keeps track of how long the set input has been in the down position
        protected StopWatch _inputReleasedTimer;//Keeps track of how long the set input has been in the up position since it was in the down position
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the maximum amount that the hit counter will count up to before the OnButtonHitCountReached event will be fired.
        /// </summary>
        public int HitCountMax
        {
            get { return _counter?.Max ?? 0; }
            set
            {
                if (_counter == null) _counter = new Counter(1, value <= 0 ? 1 : value, 1);
            }
        }

        /// <summary>
        /// Gets the current amount of times the input has been hit.
        /// </summary>
        public int CurrentHitCount => _counter.Value;

        /// <summary>
        /// Gets the current percentage of hits the input has been hit.
        /// </summary>
        public int CurrentHitCountPercentage => (int)((CurrentHitCount / (float)HitCountMax) * 100f);

        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the set input input should be in the down position before the OnInputDownTimeOut event should be invoked.
        /// </summary>
        public int InputDownTimeOut
        {
            get { return _inputDownTimer?.TimeOut ?? 0; }
            set
            {
                if (_inputDownTimer == null) _inputDownTimer = new StopWatch(value);
            }
        }

        /// <summary>
        /// Gets or sets the amount of time in milliseconds after the set input has been released before the OnInputReleasedTimeOut event should be invoked.
        /// </summary>
        public int InputReleasedTimeout
        {
            get { return _inputReleasedTimer?.TimeOut ?? 0; }
            set
            {
                if (_inputReleasedTimer == null) _inputReleasedTimer = new StopWatch(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the time elapsed should be reset when the watcher is re-enabled.
        /// </summary>
        public bool ResetTimeOnEnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the hit count should be reset when the watcher is re-enabled.
        /// </summary>
        public bool ResetHitCountOnEnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the hit count will be reset when the input has been released.
        /// </summary>
        public bool ResetHitCountOnInputRelease { get; set; }

        /// <summary>
        /// Sets the reset mode for resetting the elapsed time that the input is in the down position. If set to the auto, then the 
        /// time elapsed will reset back to 0 automatically once the InputDownTimeOut has been reached.
        /// </summary>
        public ResetType DownElapsedResetMode { get; set; }

        /// <summary>
        /// Sets the reset mode for resetting the elapsed time that the button has been released. If set to the auto, then the 
        /// time elapsed will reset back to 0 automatically once the InputReleaseTimeOut has been reached.
        /// </summary>
        public ResetType ReleasedElapsedResetMode { get; set; }

        /// <summary>
        /// Sets the reset mode for the hit counter.  If set to auto, then the hit counter will reset back to 0 automatically once the hit max has been reached.
        /// NOTE: If set the manual mode, then the ResetHitCountOnEnable and ResetHitCountOnButtonRelease settings are ignored.
        /// </summary>
        public ResetType HitCountResetMode { get; set; }

        /// <summary>
        /// Gets a value indicating if the keyDownTimeOut has expired.
        /// </summary>
        public bool TimeoutExpired => InputDownElapsedMS >= InputDownTimeOut;

        /// <summary>
        /// Gets the amount of time in milliseconds that the input has been in the down position.
        /// </summary>
        public int InputDownElapsedMS => _inputDownTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that the button has been in the down position.
        /// </summary>
        public float InputDownElapsedSeconds => InputDownElapsedMS / 1000f;

        /// <summary>
        /// Gets the amount of time in milliseconds that the input has been released since the last time it was pressed.
        /// </summary>
        public int InputReleasedElapsedMS => _inputReleasedTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that the input has been released since the last time it was pressed.
        /// </summary>
        public float InputReleasedElapsedSeconds => InputReleasedElapsedMS / 1000f;

        /// <summary>
        /// Gets or sets a value indicating if the watcher will be enabled or disabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                //If the value is going from false to true, reset the time elapsed
                if (ResetTimeOnEnable && DownElapsedResetMode == ResetType.Auto)
                {
                    //If the enabled state is going from false to true
                    if (value && !_enabled)
                        _inputDownTimer.Reset();
                }

                //If the value is going from false to true, reset the hit count
                if (ResetHitCountOnEnable && HitCountResetMode == ResetType.Auto)
                {
                    //If the enabled state is going from false to true
                    if (value && !_enabled)
                        _counter.Reset();
                }

                _enabled = value;
            }
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// Invokes the OnInputHitCountReached event.
        /// </summary>
        protected void InvokeOnInputHitCountReached()
        {
            OnInputHitCountReached?.Invoke(this, new EventArgs());
        }


        /// <summary>
        /// Invokes the OnInputDownTimeOut event.
        /// </summary>
        protected void InvokeOnInputDownTimeOut()
        {
            OnInputDownTimeOut?.Invoke(this, new EventArgs());
        }


        /// <summary>
        /// Invokes the OnInputReleasedTimeOut event.
        /// </summary>
        protected void InvokeOnInputReleaseTimeOut()
        {
            OnInputReleasedTimeOut?.Invoke(this, new EventArgs());
        }


        /// <summary>
        /// Invokes the OnInputComboPressed event.
        /// </summary>
        protected void InvokeOnInputComboPressed()
        {
            OnInputComboPressed?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}

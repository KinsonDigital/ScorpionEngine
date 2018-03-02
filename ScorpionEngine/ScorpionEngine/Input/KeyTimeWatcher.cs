using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Watches a keyboard key and invokes an event when the key has been in the down position for a set amount of time.
    /// </summary>
    public class KeyTimeWatcher : KeyboardWatcher
    {
        #region Events
        /// <summary>
        /// Occurs when the key has been pressed for a set amount of time.
        /// </summary>
        public event EventHandler OnKeyTimeout;
        #endregion

        #region Fields
        private bool _currentKeyState;
        private bool _previousKeyState;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of KeyDownTimeWatcher.
        /// </summary>
        /// <param name="timeout">The amount of time in milliseconds the key should be in the down position before invoking the OnButtonTimeout event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyTimeWatcher(double timeout, InputKeys key, bool enabled = true) : base (enabled)
        {
            Timeout = timeout;
            Key = key;
            ResetOnEnable = true;//Default to reset when re-enabled
            ResetOnKeyRelease = true;//Default the elapsed key down time to be reset when the key is released
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public InputKeys Key { get; set; }

        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the set keyboard key should be in the down position before the OnButtonTimeout event should be invoked.
        /// </summary>
        public double Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the time elapsed should be reset when re-enabled.
        /// </summary>
        public bool ResetOnEnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the time elapsed will be reset when the key has been released.
        /// </summary>
        public bool ResetOnKeyRelease { get; set; }

        /// <summary>
        /// Sets the reset mode.  If set to the default of auto, then the time elapsed will reset back to 0 automatically once the timeout has been reached.
        /// NOTE: If set to manual mode, then ResetOnEnable and ResetOnButtonRelease settings are ignored.
        /// </summary>
        public ResetType ResetMode { get; set; }

        /// <summary>
        /// Gets a value indicating if the timeout has expired.
        /// </summary>
        public bool TimeoutExpired => KeyDownElapsedMS >= Timeout;

        /// <summary>
        /// Gets the amount of time in milliseconds that the key has been in the down position.
        /// </summary>
        public double KeyDownElapsedMS { get; private set; }

        /// <summary>
        /// Gets the amount of time in seconds that the button has been in the down position.
        /// </summary>
        public float ButtonDownElapsedSeconds => (float)KeyDownElapsedMS / 1000f;

        /// <summary>
        /// Gets or sets a value indicating if the wather will be enabled or disabled.
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                //If the value is going from false to true, reset the time elapsed if the
                //ResetOnEnable setting is true
                if (ResetOnEnable && ResetMode == ResetType.Auto)
                {
                    if (value && !_enabled)
                    {
                        KeyDownElapsedMS = 0;
                    }
                }
                
                _enabled = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Update the watcher state.
        /// </summary>
        /// <param name="engineTime">The engine time info.</param>
        public override void Update(EngineTime engineTime)
        {
            //If disabled, do not watch the key
            if (!Enabled) return;

            //Update the keyboard input which keeps the state of the keyboard up to date
            UpdateBegin();

            //Get the current state of the key
            _currentKeyState = IsKeyDown(Key);

            //Check to see if the key is pressed
            if (IsKeyDown(Key))
            {
                //Update the time elapsed since last update
                KeyDownElapsedMS += engineTime.ElapsedEngineTime.TotalMilliseconds;

                //If the set time in milliseconds has elapsed
                if (KeyDownElapsedMS >= Timeout)
                {
                    OnKeyTimeout?.Invoke(this, new EventArgs());

                    //If the reset mode is set to auto, reset the time elapsed
                    if (ResetMode == ResetType.Auto)
                        KeyDownElapsedMS = 0; //Reset to start over
                }
            }

            //If the current state is up and the previous was down, and the reset on key release is enabled,
            //then reset the elapsed time
            if (!_currentKeyState && _previousKeyState && ResetOnKeyRelease && ResetMode == ResetType.Auto)
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (ResetMode == ResetType.Auto)
                    KeyDownElapsedMS = 0;
            }

            //Update the previous stae of the keyboard
            UpdateEnd();

            //Set the previous state to the current state
            _previousKeyState = _currentKeyState;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Resets the time elapsed.
        /// </summary>
        public void Reset()
        {
            KeyDownElapsedMS = 0;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ScorpionEngine.Utils;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Watches a specified key and invokes events when 
    /// </summary>
    public class KeyboardWatcher_New : IUpdatable
    {
        #region Events
        /// <summary>
        /// Occurs when the key has reached its hit count.
        /// </summary>
        public event EventHandler OnKeyHitCountReached;

        /// <summary>
        /// Occurs when the key has been pressed for a set amount of time.
        /// </summary>
        public event EventHandler OnKeyTimeout;

        /// <summary>
        /// Occurs when the given key combo has been pressed.
        /// </summary>
        public event EventHandler OnKeyComboPressed;
        #endregion

        #region Fields
        private KeyboardInput _keyboardInput;
        private Counter _counter;//The counter used to keep track of how many times a key has been hit
        private readonly Stopwatch _stopWatch = new Stopwatch();//Keeps track of how long comboKeys have been pressed
        private bool _enabled;
        private bool _currentKeyState;
        private bool _previousKeyState;
        private InputKeys[] _originalKeyList;//The list of comboKeys to be pressed to invoke the event
        private Dictionary<InputKeys, bool> _currentPressedKeys;//Holds the list of comboKeys and there down states
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        protected KeyboardWatcher_New(bool enabled = true)
        {
            Init(10, InputKeys.None, null, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher_New(List<InputKeys> comboKeys, bool enabled = true)
        {
            Init(-1, InputKeys.None, comboKeys, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher_New(int hitCountMax, InputKeys key, bool enabled = true)
        {
            Init(hitCountMax, key, null, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="timeout">Sets the time in milliseconds that the given key should be pressed before the OnKeyTimout event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher_New(int hitCountMax, InputKeys key, int timeout, bool enabled = true)
        {
            Init(hitCountMax, key, null, timeout, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher_New(int hitCountMax, InputKeys key, List<InputKeys> comboKeys, bool enabled = true)
        {
            Init(hitCountMax, key, comboKeys, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="timeout">Sets the time in milliseconds that the given key should be pressed before the OnKeyTimout event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher_New(int hitCountMax, InputKeys key, List<InputKeys> comboKeys, int timeout, bool enabled = true)
        {
            Init(hitCountMax, key, comboKeys, timeout, enabled);
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets or sets the maximum amount that the hit counter will count up to before the OnButtonHitCountReached event will be fired.
        /// </summary>
        public int HitCountMax => _counter.Max;

        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public InputKeys Key { get; set; }

        /// <summary>
        /// Gets the current amount of times the key has been hit.
        /// </summary>
        public int CurrentHitCount => _counter.Value;

        /// <summary>
        /// Gets the current percentage of hits the key has been hit.
        /// </summary>
        public int CurrentHitCountPercentage => (int)((CurrentHitCount / (float)HitCountMax) * 100f);

        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the set keyboard key should be in the down position before the OnButtonTimeout event should be invoked.
        /// </summary>
        public int Timeout { get; set; }

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
        public double KeyDownElapsedMS => _stopWatch.ElapsedMilliseconds;

        /// <summary>
        /// Gets the amount of time in seconds that the button has been in the down position.
        /// </summary>
        public float KeyDownElapsedSeconds => (float)KeyDownElapsedMS / 1000f;

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
                //If the value is going from false to true, reset the time elapsed if the
                //ResetOnEnable setting is true
                if (ResetOnEnable && ResetMode == ResetType.Auto)
                {
                    //If the enabled state is going from false to true
                    if (value && ! _enabled)
                        Reset();
                }

                _enabled = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Update the watcher state.
        /// </summary>
        /// <param name="engineTime">The engine time info.</param>
        public void Update(EngineTime engineTime)
        {
            //If disabled, exit
            if (! Enabled) return;

            _keyboardInput.UpdateCurrentState();

            //Get the current state of the key
            _currentKeyState = IsKeyDown(Key);

            #region Hit Count Code
            //If the hit count is -1 or the key is set to None, ignore count processing
            if (_counter != null || Key == InputKeys.None)
            {
                if (IsKeyPressed(Key))
                {
                    //If the max is reached, invoke the OnButtonHitCountReached event and reset it back to 0
                    if (CurrentHitCount == HitCountMax)
                    {
                        OnKeyHitCountReached?.Invoke(this, new EventArgs());

                        //Reset the current hits back to 0
                        _counter?.Reset();
                    }
                    else
                    {
                        _counter?.Count(); //Increment the current hit count
                    }
                }
            }
            #endregion

            #region Timing Code
            //If the timeout is -1, skip the key down timeout code
            if (Timeout != - 1)
            {
                //Check to see if the key is pressed
                if (IsKeyDown(Key))
                {
                    _stopWatch.Start();

                    //If the set time in milliseconds has elapsed
                    if (KeyDownElapsedMS >= Timeout)
                    {
                        //If the reset mode is set to auto, reset the time elapsed
                        if (ResetMode == ResetType.Auto)
                            Reset();

                        OnKeyTimeout?.Invoke(this, new EventArgs());
                    }
                }
            }

            //If the current state is up and the previous was down, and the reset on key release is enabled,
            //then reset the elapsed time
            if (!_currentKeyState && _previousKeyState && ResetOnKeyRelease && ResetMode == ResetType.Auto)
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (ResetMode == ResetType.Auto)
                    Reset();
            }
            #endregion

            #region Key Combo Code
            //If the key combo list is not null
            if (_currentPressedKeys != null)
            {
                //Set the state of all of the pressed keys
                foreach (var k in _originalKeyList)
                {
                    //Set the key down state of the current key
                    _currentPressedKeys[k] = IsKeyDown(k);
                }

                //If all of the keys are pressed down
                if (_currentPressedKeys.All(key => key.Value))
                    OnKeyComboPressed?.Invoke(this, new EventArgs());
            }
            #endregion

            _keyboardInput.UpdatePreviousState();
            _previousKeyState = _currentKeyState;
        }

        /// <summary>
        /// Resets the time elapsed.
        /// </summary>
        public void Reset()
        {
            _stopWatch.Reset();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        private bool IsKeyDown(InputKeys key)
        {
            return _keyboardInput.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        private bool IsKeyUp(InputKeys key)
        {
            return _keyboardInput.IsKeyUp(key);
        }

        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        private bool IsKeyPressed(InputKeys key)
        {
            return _keyboardInput.IsKeyPressed(key);
        }

        /// <summary>
        /// Initializes the KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="timeout">Sets the time in milliseconds that the given key should be pressed before the OnKeyTimout event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        private void Init(int hitCountMax, InputKeys key, List<InputKeys> comboKeys, int timeout, bool enabled = true)
        {
            _keyboardInput = new KeyboardInput();

            //Create the counter
            _counter = hitCountMax == -1 ? null : new Counter(1, hitCountMax, 1);

            Key = key;

            #region Process the combo keys
            //If the combo keys are null, skip combo key setup
            if (comboKeys != null)
            {
                //Create the original key list array
                _originalKeyList = new InputKeys[comboKeys.ToList().Count];

                //Create the current pressed keys dictionary
                _currentPressedKeys = new Dictionary<InputKeys, bool>();

                //Add all of the keys to the combo keys list dictionary
                for (var i = 0; i < comboKeys.Count; i++)
                {
                    //Add the key to the dictionary
                    _currentPressedKeys.Add(comboKeys[i], false);

                    //Set the original key list
                    _originalKeyList[i] = comboKeys[i];
                }
            }
            #endregion

            Timeout = timeout;

            Enabled = enabled;

            ResetOnKeyRelease = true;

            ResetMode = ResetType.Auto;
        }
        #endregion
    }
}
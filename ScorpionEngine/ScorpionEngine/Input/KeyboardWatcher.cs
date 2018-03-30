using System.Collections.Generic;
using System.Linq;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Watches a specified key and invokes events when 
    /// </summary>
    public class KeyboardWatcher : InputWatcher, IUpdatable
    {
        #region Fields
        private KeyboardInput _keyboardInput;
        private Dictionary<InputKeys, bool> _currentPressedKeys;//Holds the list of comboKeys and there down states
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher(bool enabled = true)
        {
            Init(10, InputKeys.None, null, -1, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher(List<InputKeys> comboKeys, bool enabled = true)
        {
            Init(-1, InputKeys.None, comboKeys, -1, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher(int hitCountMax, InputKeys key, bool enabled = true)
        {
            Init(hitCountMax, key, null, -1, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="keyDownTimeOut">Sets the time in milliseconds that the given key should be pressed before the OnKeyDownTimeOut event will be invoked.</param>
        /// <param name="keyReleaseTimeOut">Sets the time in milliseconds that the given key should be released before the OnKeyReleaseTimeout event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher(int hitCountMax, InputKeys key, int keyDownTimeOut, int keyReleaseTimeOut, bool enabled = true)
        {
            Init(hitCountMax, key, null, keyDownTimeOut, keyReleaseTimeOut, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher(int hitCountMax, InputKeys key, List<InputKeys> comboKeys, bool enabled = true)
        {
            Init(hitCountMax, key, comboKeys, -1, -1, enabled);
        }

        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="keyDownTimeOut">Sets the time in milliseconds that the given key should be pressed before the OnKeyDownTimeOut event will be invoked.</param>
        /// <param name="keyReleaseTimeOut">Sets the time in milliseconds that the given key should be released before the OnKeyReleaseTimeout event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher(int hitCountMax, InputKeys key, List<InputKeys> comboKeys, int keyDownTimeOut, int keyReleaseTimeOut, bool enabled = true)
        {
            Init(hitCountMax, key, comboKeys, keyDownTimeOut, keyReleaseTimeOut, enabled);
        }
        #endregion

        #region Props
        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public InputKeys Key { get; set; }

        /// <summary>
        /// Gets or sets the list of combo keys.
        /// </summary>
        public List<InputKeys> ComboKeys
        {
            get
            {
                return _currentPressedKeys.Keys.ToList();
            }
            set
            {
                CreateCurrentPressedKeys(value?.ToArray());
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

            //Update the current state of the keyboard
            _keyboardInput.UpdateCurrentState();

            //Update the key down timer to keep track of how much time that the key has been in the down position
            _inputDownTimer.Update(engineTime);

            //Update the key release timer to keep track of how much time that the key has been release since it was last pressed
            _inputReleasedTimer.Update(engineTime);

            //Get the current state of the key
            _curState = _keyboardInput.IsKeyDown(Key);

            #region Hit Count Code
            if (_keyboardInput.IsKeyPressed(Key))
            {
                //If the max is reached, invoke the OnButtonHitCountReached event and reset it back to 0
                if (_counter.Value == HitCountMax)
                {
                    InvokeOnInputHitCountReached();

                    //If the reset mode is set to auto, reset the hit counter
                    if(HitCountResetMode == ResetType.Auto)
                        _counter?.Reset();
                }
                else
                {
                    _counter?.Count(); //Increment the current hit count
                }
            }
            #endregion

            #region Timing Code
            //Check to see if the key release timeout has elapsed, if so invoke the OnKeyReleaseTimeOut event
            if (_inputReleasedTimer.ElapsedMS >= InputReleasedTimeout)
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                    //Reset the timer
                    _inputReleasedTimer.Reset();

                //Invoke the event
                InvokeOnInputReleaseTimeOut();
            }

            //Check to see if the key is pressed
            if (_keyboardInput.IsKeyDown(Key))
            {
                //Stop and reset the key release timer
                _inputReleasedTimer.Reset();

                _inputDownTimer.Start();

                //If the set time in milliseconds has elapsed
                if (InputDownElapsedMS >= InputDownTimeOut)
                {
                    //If the reset mode is set to auto, reset the time elapsed
                    if (DownElapsedResetMode == ResetType.Auto)
                        _inputDownTimer.Reset();

                    InvokeOnInputDownTimeOut();
                }
            }

            //Reset the elapsed time if the current state is up and the previous was down, and the reset on key release is enabled
            if (!_curState && _prevState)
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                    _inputDownTimer.Reset();

                //Start the key release timer
                _inputReleasedTimer.Start();
            }
            #endregion

            #region Key Combo Code
            //If the key combo list is not null
            if (_currentPressedKeys != null)
            {
                //Holds the list of keys from the pressed keys dictionary
                var keys = new List<InputKeys>(_currentPressedKeys.Keys);

                //Set the state of all of the pressed keys
                foreach (var key in keys)
                {
                    _currentPressedKeys[key] = _keyboardInput.IsKeyDown(key);
                }

                //If all of the keys are pressed down
                if (_currentPressedKeys.All(key => key.Value))
                    InvokeOnInputComboPressed();
            }
            #endregion

            _keyboardInput.UpdatePreviousState();

            _prevState = _curState;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes the KeyboardWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the key will be hit before invoking an event.</param>
        /// <param name="key">The key to watch.</param>
        /// <param name="comboKeys">The list of comboKeys to press in combination to invoke the event.</param>
        /// <param name="keyDownTimeOut">Sets the time in milliseconds that the given key should be pressed before the OnKeyTimout event will be invoked.</param>
        /// <param name="keyReleaseTimeOut">Sets the time in milliseconds that the given key should be released before the OnKeyReleaseTimeout event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        private void Init(int hitCountMax, InputKeys key, List<InputKeys> comboKeys, int keyDownTimeOut, int keyReleaseTimeOut, bool enabled = true)
        {
            _keyboardInput = new KeyboardInput();

            Key = key;

            //If the combo keys are null, skip the combo key setup
            if (comboKeys != null)
                CreateCurrentPressedKeys(comboKeys.ToArray());

            if (hitCountMax > 0) HitCountMax = hitCountMax;

            InputDownTimeOut = keyDownTimeOut;

            InputReleasedTimeout = keyReleaseTimeOut;

            Enabled = enabled;

            DownElapsedResetMode = ResetType.Auto;
            ReleasedElapsedResetMode = ResetType.Auto;

            HitCountResetMode = ResetType.Auto;
        }

        /// <summary>
        /// Creates the list of pressed keys from the given list of keys
        /// </summary>
        /// <param name="keys">The list of combo keys.</param>
        private void CreateCurrentPressedKeys(IList<InputKeys> keys)
        {
            //If the combo keys are null, skip combo key setup
            if (keys != null)
            {
                //Create the current pressed keys dictionary
                _currentPressedKeys = new Dictionary<InputKeys, bool>();

                //Add all of the keys to the combo keys list dictionary
                foreach (var key in keys)
                {
                    //If the key has not already been added
                    if(!_currentPressedKeys.ContainsKey(key))
                        //Add the key to the dictionary
                        _currentPressedKeys.Add(key, false);
                }
            }
        }
        #endregion
    }
}
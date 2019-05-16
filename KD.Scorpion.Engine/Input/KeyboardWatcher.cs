using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KDScorpionEngine.Input
{
    /// <summary>
    /// Watches a specified key and invokes events for particular states of the keyboard keys.
    /// </summary>
    public class KeyboardWatcher : IInputWatcher, IUpdatable
    {
        #region Public Event Handlers
        public event EventHandler OnInputComboPressed;
        public event EventHandler OnInputDownTimeOut;
        public event EventHandler OnInputHitCountReached;
        public event EventHandler OnInputReleasedTimeOut;
        #endregion


        #region Fields
        private IKeyboard _keyboard;
        private Dictionary<KeyCodes, bool> _currentPressedKeys;//Holds the list of comboKeys and there down states
        protected Counter _counter;//Keeps track of the hit count of an input
        protected bool _curState;//The current state of the set input
        protected bool _prevState;//The previous state of the set input
        protected StopWatch _keyDownTimer;//Keeps track of how long the set input has been in the down position
        protected StopWatch _keyReleasedTimer;//Keeps track of how long the set input has been in the up position since it was in the down position
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public KeyboardWatcher(bool enabled)
        {
            _keyboard = PluginSystem.EnginePlugins.LoadPlugin<IKeyboard>();
            Enabled = enabled;
            ComboKeys = new List<KeyCodes>();

            //Setup stop watches
            _counter = new Counter(0, 10, 1);
            _keyDownTimer = new StopWatch(1000);
            _keyDownTimer.OnTimeElapsed += _keyDownTimer_OnTimeElapsed;
            _keyDownTimer.Start();

            _keyReleasedTimer = new StopWatch(1000);
            _keyReleasedTimer.OnTimeElapsed += _keyUpTimer_OnTimeElapsed;
            _keyReleasedTimer.Start();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the list of combo keys.
        /// </summary>
        public List<KeyCodes> ComboKeys
        {
            get => _currentPressedKeys.Keys.ToList();
            set => CreateCurrentPressedKeys(value.ToArray());
        }

        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public KeyCodes Key { get; set; } = KeyCodes.None;

        public int CurrentHitCount => _counter.Value;

        public int CurrentHitCountPercentage => (int)(CurrentHitCount / (float)HitCountMax * 100f);

        public ResetType DownElapsedResetMode { get; set; } = ResetType.Auto;

        public bool Enabled { get; set; }

        public int HitCountMax
        {
            get => _counter.Max;
            set => _counter.Max = value;
        }

        public ResetType HitCountResetMode { get; set; } = ResetType.Auto;

        public int InputDownElapsedMS => _keyDownTimer.ElapsedMS;

        public float InputDownElapsedSeconds => _keyDownTimer.ElapsedSeconds;

        public int InputDownTimeOut
        {
            get => _keyDownTimer.TimeOut;
            set => _keyDownTimer.TimeOut = value;
        }

        public int InputReleasedElapsedMS => _keyReleasedTimer.ElapsedMS;

        public float InputReleasedElapsedSeconds => _keyReleasedTimer.ElapsedSeconds;

        public int InputReleasedTimeout
        {
            get => _keyReleasedTimer.TimeOut;
            set => _keyReleasedTimer.TimeOut = value;
        }

        public ResetType ReleasedElapsedResetMode { get; set; } = ResetType.Auto;
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
            _keyboard.UpdateCurrentState();

            //Update the key down timer to keep track of how much time that the key has been in the down position
            _keyDownTimer.Update(engineTime);

            //Update the key release timer to keep track of how much time that the key has been in the
            //up position since its release
            _keyReleasedTimer.Update(engineTime);

            //Get the current state of the key
            _curState = _keyboard.IsKeyDown(Key);


            #region Hit Count Code
            if (_keyboard.IsKeyPressed(Key))
            {
                //If the max is reached, invoke the OnButtonHitCountReached event and reset it back to 0
                if (_counter != null && _counter.Value == HitCountMax)
                {
                    OnInputHitCountReached?.Invoke(this, new EventArgs());

                    //If the reset mode is set to auto, reset the hit counter
                    if (HitCountResetMode == ResetType.Auto)
                        _counter.Reset();
                }
                else
                {
                    _counter?.Count(); //Increment the current hit count
                }
            }
            #endregion


            #region Timing Code
            //As long as the button is down, continue to keep the button release timer reset to 0
            if (_keyboard.IsKeyDown(Key))
                _keyReleasedTimer.Reset();

            //If the button is not pressed down and the button was pressed down last frame,
            //reset the input down timer and start the button release timer.
            if (!_curState && _prevState)
            {
                _keyDownTimer.Reset();
                _keyReleasedTimer.Start();
            }
            #endregion


            #region Key Combo Code
            //If the key combo list is not null
            if (_currentPressedKeys != null)
            {
                //Holds the list of keys from the pressed keys dictionary
                var keys = new List<KeyCodes>(_currentPressedKeys.Keys);

                //Set the state of all of the pressed keys
                foreach (var key in keys)
                {
                    _currentPressedKeys[key] = _keyboard.IsKeyDown(key);
                }

                //If all of the keys are pressed down
                if (_currentPressedKeys.Count > 0 && _currentPressedKeys.All(key => key.Value))
                    OnInputComboPressed?.Invoke(this, new EventArgs());
            }
            #endregion

            _keyboard.UpdatePreviousState();

            _prevState = _curState;
        }
        #endregion


        #region Private Event Methods
        private void _keyUpTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (_keyboard.IsKeyUp(Key))
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                    _keyReleasedTimer.Reset();

                OnInputReleasedTimeOut?.Invoke(this, new EventArgs());
            }
        }


        private void _keyDownTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (_keyboard.IsKeyDown(Key))
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                    _keyDownTimer.Reset();

                //Invoke the event
                OnInputDownTimeOut?.Invoke(this, new EventArgs());
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Creates the list of pressed keys from the given list of keys
        /// </summary>
        /// <param name="keys">The list of combo keys.</param>
        private void CreateCurrentPressedKeys(IList<KeyCodes> keys)
        {
            //If the combo keys are null, skip combo key setup
            if (keys != null)
            {
                //Create the current pressed keys dictionary
                _currentPressedKeys = new Dictionary<KeyCodes, bool>();

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
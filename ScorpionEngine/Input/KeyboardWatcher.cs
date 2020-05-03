using KDScorpionEngine.Utils;
using Raptor;
using Raptor.Input;
using Raptor.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KDScorpionEngine.Input
{
    /// <summary>
    /// Watches a keyboard key for various events and behaviors such is how many times a key is pressed,
    /// how long it is held down or how long it has been released.  Various events will be triggered when
    /// these behaviours occur.
    /// </summary>
    public class KeyboardWatcher : IInputWatcher, IUpdatable
    {
        #region Public Event Handlers
        /// <summary>
        /// Occurs when the combo key setup has been pressed.
        /// </summary>
        public event EventHandler OnInputComboPressed;

        /// <summary>
        /// Occurs when the set keyboard key has been held in the down position for a set amount of time.
        /// </summary>
        public event EventHandler OnInputDownTimeOut;

        /// <summary>
        /// Occurs when the set keyboard key has been hit a set amount of times.
        /// </summary>
        public event EventHandler OnInputHitCountReached;

        /// <summary>
        /// Occurs when the set keyboard key has been released from the down position for a set amount of time.
        /// </summary>
        public event EventHandler OnInputReleasedTimeOut;
        #endregion


        #region Private Fields
        private readonly Keyboard _keyboard;
        private Dictionary<KeyCode, bool> _currentPressedKeys;//Holds the list of comboKeys and there down states
        protected Counter _counter;//Keeps track of the hit count of an input
        protected bool _curState;//The current state of the set input
        protected bool _prevState;//The previous state of the set input
        protected StopWatch _keyDownTimer;//Keeps track of how long the set input has been in the down position
        protected StopWatch _keyReleasedTimer;//Keeps track of how long the set input has been in the up position since it was in the down position
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="KeyboardWatcher"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="keyboard">The mocked keyboard to inject for testing purposes.</param>
        internal KeyboardWatcher(IKeyboard keyboard)
        {
            _keyboard = new Keyboard(keyboard);
            Setup(true);
        }


        /// <summary>
        /// Creates an instance of KeyboardWatcher.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher when created.</param>
        [ExcludeFromCodeCoverage]
        public KeyboardWatcher(bool enabled)
        {
            _keyboard = new Keyboard();
            Setup(enabled);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets a value indicating if the <see cref="KeyboardWatcher"/> is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the list of combo keys.
        /// </summary>
        public List<KeyCode> ComboKeys
        {
            get => _currentPressedKeys.Keys.ToList();
            set => CreateCurrentPressedKeys(value.ToArray());
        }

        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public KeyCode Key { get; set; } = KeyCode.None;

        /// <summary>
        /// Gets current amount of times that the set key has been hit.
        /// </summary>
        public int CurrentHitCount => _counter.Value;

        /// <summary>
        /// Gets the current hit percentage that the key has been hit out of the total number of maximum tims to be hit.
        /// </summary>
        public int CurrentHitCountPercentage => (int)(CurrentHitCount / (float)HitCountMax * 100f);

        /// <summary>
        /// Gets or sets the reset mode that the watcher will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the amount of time the key is in the down position.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        public ResetType DownElapsedResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Gets or sets the maximum amount of times that the set keyboard key should be hit before 
        /// invoking the <see cref="OnInputHitCountReached"/> event is reached.
        /// </summary>
        public int HitCountMax
        {
            get => _counter.Max;
            set => _counter.Max = value;
        }

        /// <summary>
        /// Gets or sets the reset mode that the watcher's hit count will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the hit count.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        public ResetType HitCountResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the key has been held in the down position.
        /// </summary>
        public int InputDownElapsedMS => _keyDownTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the key has been held in the down position.
        /// </summary>
        public float InputDownElapsedSeconds => _keyDownTimer.ElapsedSeconds;

        /// <summary>
        /// The amount of time in milliseconds that the key should be held down before invoking the <see cref="OnInputDownTimeOut"/> event.
        /// </summary>
        public int InputDownTimeOut
        {
            get => _keyDownTimer.TimeOut;
            set => _keyDownTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the key has been released and is in the up position.
        /// </summary>
        public int InputReleasedElapsedMS => _keyReleasedTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the key has been released and is in the up position.
        /// </summary>
        public float InputReleasedElapsedSeconds => _keyReleasedTimer.ElapsedSeconds;

        /// <summary>
        /// The amount of time in milliseconds that the key should be released to the up position
        /// after being released from the down position before invoking the <see cref="OnInputReleasedTimeOut"/> event.
        /// </summary>
        public int InputReleasedTimeout
        {
            get => _keyReleasedTimer.TimeOut;
            set => _keyReleasedTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets or sets the reset mode that the watcher's key released functionality will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the key being released.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        public ResetType ReleasedElapsedResetMode { get; set; } = ResetType.Auto;
        #endregion


        #region Public Methods
        /// <summary>
        /// Update the watcher state.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
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
                //If the max is reached, invoke the OnInputHitCountReached event and reset it back to 0
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
            //As long as the key is down, continue to keep the key release timer reset to 0
            if (_keyboard.IsKeyDown(Key))
                _keyReleasedTimer.Reset();

            //If the key is not pressed down and the key was pressed down last frame,
            //reset the input down timer and start the key release timer.
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
                var keys = new List<KeyCode>(_currentPressedKeys.Keys);

                //Set the state of all of the pressed keys
                keys.ForEach(k => _currentPressedKeys[k] = _keyboard.IsKeyDown(k));
                
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
        /// <summary>
        /// Occurs when the key has been held down for a set amount of time.
        /// </summary>
        private void KeyUpTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (_keyboard.IsKeyUp(Key))
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                    _keyReleasedTimer.Reset();

                OnInputReleasedTimeOut?.Invoke(this, new EventArgs());
            }
        }


        /// <summary>
        /// Occurs when the key has been released from the down position for a set amount of time.
        /// </summary>
        private void KeyDownTimer_OnTimeElapsed(object sender, EventArgs e)
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
        /// Sets up the <see cref="KeyboardWatcher"/>.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher when created.</param>
        private void Setup(bool enabled)
        {
            Enabled = enabled;
            ComboKeys = new List<KeyCode>();

            //Setup stop watches
            _counter = new Counter(0, 10, 1);
            _keyDownTimer = new StopWatch(1000);
            _keyDownTimer.OnTimeElapsed += KeyDownTimer_OnTimeElapsed;
            _keyDownTimer.Start();

            _keyReleasedTimer = new StopWatch(1000);
            _keyReleasedTimer.OnTimeElapsed += KeyUpTimer_OnTimeElapsed;
            _keyReleasedTimer.Start();
        }


        /// <summary>
        /// Creates the list of pressed keys from the given list of keys
        /// </summary>
        /// <param name="keys">The list of combo keys.</param>
        private void CreateCurrentPressedKeys(IList<KeyCode> keys)
        {
            //If the combo keys are null, skip combo key setup
            if (keys != null)
            {
                //Create the current pressed keys dictionary
                _currentPressedKeys = new Dictionary<KeyCode, bool>();

                //Add all of the keys to the combo keys list dictionary
                keys.ToList().ForEach(k =>
                {
                    //If the key has not already been added
                    if (!_currentPressedKeys.ContainsKey(k))
                        _currentPressedKeys.Add(k, false);
                });
            }
        }
        #endregion
    }
}
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Utils;
using PluginSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KDScorpionEngine.Input
{
    /// <summary>
    /// Watches a mouse button and invokes an event when the button is pressed a set amount of times.
    /// </summary>
    public class MouseWatcher : IInputWatcher, IUpdatable
    {
        #region Public Event Handlers
        public event EventHandler OnInputComboPressed;
        public event EventHandler OnInputDownTimeOut;
        public event EventHandler OnInputHitCountReached;
        public event EventHandler OnInputReleasedTimeOut;
        #endregion


        #region Fields
        private IMouse _mouse;
        private Dictionary<InputButton, bool> _currentPressedButtons;//Holds the list of combo buttons and there down states
        private StopWatch _buttonDownTimer;//Keeps track of how long the set input has been in the down position
        private StopWatch _buttonReleaseTimer;//Keeps track of how long the set input has been in the up position
        private Counter _counter;//Keeps track of the hit count of an input
        private bool _curState;//The current state of the set input
        private bool _prevState;//The previous state of the set input
        #endregion


        #region Constructor
        /// <summary>
        /// Creates an instance of MouseWatcher.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public MouseWatcher(bool enabled)
        {
            _mouse = Plugins.PluginFactory.CreateMouse();

            Enabled = enabled;
            ComboButtons = new List<InputButton>();

            //Setup stop watches
            _counter = new Counter(0, 10, 1);
            _buttonDownTimer = new StopWatch(1000);
            _buttonDownTimer.OnTimeElapsed += _buttonDownTimer_OnTimeElapsed;
            _buttonDownTimer.Start();

            _buttonReleaseTimer = new StopWatch(1000);
            _buttonReleaseTimer.OnTimeElapsed += _buttonReleasedTimer_OnTimeElapsed;
            _buttonReleaseTimer.Start();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the list of combo buttons.
        /// </summary>
        public List<InputButton> ComboButtons
        {
            get { return _currentPressedButtons.Keys.ToList(); }
            set
            {
                CreateCurrentPressedButtons(value);
            }
        }

        /// <summary>
        /// Gets or sets the button to watch.
        /// </summary>
        public InputButton Button { get; set; } = InputButton.None;

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

        public int InputDownElapsedMS => _buttonDownTimer.ElapsedMS;

        public float InputDownElapsedSeconds => _buttonReleaseTimer.ElapsedSeconds;

        public int InputDownTimeOut
        {
            get => _buttonDownTimer.TimeOut;
            set => _buttonDownTimer.TimeOut = value;
        }

        public int InputReleasedElapsedMS => _buttonReleaseTimer.ElapsedMS;

        public float InputReleasedElapsedSeconds => _buttonReleaseTimer.ElapsedSeconds;

        public int InputReleasedTimeout
        {
            get => _buttonReleaseTimer.TimeOut;
            set => _buttonReleaseTimer.TimeOut = value;
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
            if (!Enabled) return;

            //Update the current state of the mouse
            _mouse.UpdateCurrentState();

            //Update the mouse button down timer to keep track of how much time that the button has been in the down position
            _buttonDownTimer.Update(engineTime);

            //Update the mouse button release timer to keep track of how much time that the button has been in the
            //up position since its release
            _buttonReleaseTimer.Update(engineTime);

            //Get the current state of the button
            _curState = _mouse.IsButtonDown((int)Button);


            #region Hit Count Code
            //If the counter is not null
            if (_mouse.IsButtonPressed((int)Button))
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
            //As long as the button is down, continue to keep the button release timer reset to 0
            if (_mouse.IsButtonDown((int)Button))
                _buttonReleaseTimer.Reset();

            //If the button is not pressed down and the button was pressed down last frame,
            //reset the input down timer and start the button release timer.
            if (!_curState && _prevState)
            {
                _buttonDownTimer.Reset();
                _buttonReleaseTimer.Start();
            }
            #endregion


            #region Button Combo Code
            //If the button combo list is not null
            if (_currentPressedButtons != null)
            {
                //Holds the list of keys from the pressed buttons dictionary
                var buttons = new List<InputButton>(_currentPressedButtons.Keys);

                //Set the state of all of the pressed buttons
                foreach (var button in buttons)
                {
                    _currentPressedButtons[button] = _mouse.IsButtonDown((int)button);
                }

                //If all of the buttons are pressed down
                if (_currentPressedButtons.Count > 0 && _currentPressedButtons.All(button => button.Value))
                    OnInputComboPressed?.Invoke(this, new EventArgs());
            }
            #endregion

            _mouse.UpdatePreviousState();

            _prevState = _curState;
        }
        #endregion


        #region Private Event Methods
        private void _buttonDownTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (_mouse.IsButtonDown((int)Button))
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                    _buttonDownTimer.Reset();

                OnInputDownTimeOut?.Invoke(this, new EventArgs());
            }
        }


        private void _buttonReleasedTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (_mouse.IsButtonUp((int)Button))
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                    _buttonReleaseTimer.Reset();

                //Invoke the event
                OnInputReleasedTimeOut?.Invoke(this, new EventArgs());
            }
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Creates the list of pressed buttons from the given list of buttons.
        /// </summary>
        /// <param name="buttons">The list of combo buttons.</param>
        private void CreateCurrentPressedButtons(IList<InputButton> buttons)
        {
            //If the combo buttons are null, skip combo button setup
            if (buttons != null)
            {
                //Create the current pressed buttons dictionary
                _currentPressedButtons = new Dictionary<InputButton, bool>();

                //Add all of the buttons to the combo buttons list dictionary
                foreach (var button in buttons)
                {
                    //If the button has not alredy been added
                    if(!_currentPressedButtons.ContainsKey(button))
                        //Add the button to the dictionary
                        _currentPressedButtons.Add(button, false);
                }
            }
        }
        #endregion
    }
}
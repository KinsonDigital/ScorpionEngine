using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScorpionEngine.Input
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
        private Dictionary<InputButton, bool> _currentPressedButtons;//Holds the list of combo buttons and there down states
        protected StopWatch _inputDownTimer;//Keeps track of how long the set input has been in the down position
        protected StopWatch _inputReleasedTimer;//Keeps track of how long the set input has been in the up positi
        protected Counter _counter;//Keeps track of the hit count of an input
        protected bool _curState;//The current state of the set input
        protected bool _prevState;//The previous state of the set input
        #endregion


        #region Constructor
        /// <summary>
        /// Creates an instance of MouseWatcher.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public MouseWatcher(bool enabled = true)
        {
            InternalMouse = PluginSystem.EnginePlugins.LoadPlugin<IMouse>();
            Init(10, InputButton.None, null, -1, -1, enabled);
        }


        /// <summary>
        /// Creates an instance of MouseWatcher.
        /// </summary>
        /// <param name="comboButtons">The list of combo buttons to press in combination to invoke the event.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public MouseWatcher(List<InputButton> comboButtons, bool enabled = true)
        {
            InternalMouse = PluginSystem.EnginePlugins.LoadPlugin<IMouse>();
            Init(-1, InputButton.None, comboButtons, -1, -1, enabled);
        }


        /// <summary>
        /// Creates a new instance of MouseWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the button will be hit before invoking an event.</param>
        /// <param name="button">The button to watch.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public MouseWatcher(int hitCountMax, InputButton button, bool enabled = true)
        {
            InternalMouse = PluginSystem.EnginePlugins.LoadPlugin<IMouse>();
            Init(hitCountMax, button, null, -1, -1, enabled);
        }


        /// <summary>
        /// Creates a new instance of MouseWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the button will be hit before invoking an event.</param>
        /// <param name="button">The button to watch.</param>
        /// <param name="buttonDownTimeOut">The amount of time in milliseconds the button should be in the down position before invoking the OnInputTimeOut event.</param>
        /// <param name="buttonReleaseTimeOut">Sets the time in milliseconds that the given button should be released before the OnInputReleasedTimeOut event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public MouseWatcher(int hitCountMax, InputButton button, int buttonDownTimeOut, int buttonReleaseTimeOut, bool enabled = true)
        {
            InternalMouse = PluginSystem.EnginePlugins.LoadPlugin<IMouse>();
            Init(hitCountMax, button, null,  buttonDownTimeOut, buttonReleaseTimeOut, enabled);
        }


        /// <summary>
        /// Creates an instance of MouseWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the button will be hit before invoking an event.</param>
        /// <param name="button">The button to watch.</param>
        /// <param name="comboButtons">The list of combo buttons to press in combination to invoke the event.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public MouseWatcher(int hitCountMax, InputButton button, List<InputButton> comboButtons, bool enabled = true)
        {
            InternalMouse = PluginSystem.EnginePlugins.LoadPlugin<IMouse>();
            Init(hitCountMax, button, comboButtons, -1, -1, enabled);
        }


        /// <summary>
        /// Creates an instance of MouseWatcher.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the button will be hit before invoking an event.</param>
        /// <param name="button">The button to watch.</param>
        /// <param name="comboButtons">The list of combo buttons to press in combination to invoke the event.</param>
        /// <param name="buttonDownTimeOut">Sets the time in milliseconds that the given button should be pressed before the OnInputDownTimeOut event will be invoked.</param>
        /// <param name="buttonReleaseTimeOut">Sets the time in milliseconds that the given button should be released before the OnInputReleaseTimeout event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        public MouseWatcher(int hitCountMax, InputButton button, List<InputButton> comboButtons, int buttonDownTimeOut, int buttonReleaseTimeOut, bool enabled = true)
        {
            InternalMouse = PluginSystem.EnginePlugins.LoadPlugin<IMouse>();
            Init(hitCountMax, button, comboButtons, buttonDownTimeOut, buttonReleaseTimeOut, enabled);
        }
        #endregion


        #region Props
        internal IMouse InternalMouse { get; set; }

        /// <summary>
        /// Gets or sets the list of combo buttons.
        /// </summary>
        public List<InputButton> ComboButtons
        {
            get { return _currentPressedButtons.Keys.ToList(); }
            set
            {
                CreateCurrentPressedButtons(value?.ToArray());
            }
        }

        /// <summary>
        /// Gets or sets the button to watch.
        /// </summary>
        public InputButton Button { get; set; }

        public int CurrentHitCount { get; }

        public int CurrentHitCountPercentage { get; }

        public ResetType DownElapsedResetMode { get; set; }

        public bool Enabled { get; set; }

        public int HitCountMax { get; set; }

        public ResetType HitCountResetMode { get; set; }

        public int InputDownElapsedMS { get; }

        public float InputDownElapsedSeconds { get; }

        public int InputDownTimeOut { get; set; }

        public int InputReleasedElapsedMS { get; }

        public float InputReleasedElapsedSeconds { get; }

        public int InputReleasedTimeout { get; set; }

        public ResetType ReleasedElapsedResetMode { get; set; }

        public bool ResetHitCountOnEnable { get; set; }

        public bool ResetHitCountOnInputRelease { get; set; }

        public bool ResetTimeOnEnable { get; set; }

        public bool TimeoutExpired { get; }
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
            InternalMouse.UpdateCurrentState();

            //Update the mouse button down timer to keep track of how much time that the button has been in the down position
            _inputDownTimer.Update(engineTime);

            //Update the mouse button release timer to keep track of how much time that the button has been in the
            //up position since its release
            _inputReleasedTimer.Update(engineTime);

            //Get the current state of the button
            _curState = InternalMouse.IsButtonDown((int)Button);

            #region Hit Count Code
            //If the counter is not null
            if (_counter != null)
            {
                if (InternalMouse.IsButtonPressed((int)Button))
                {
                    //If the max is reached, invoke the OnInputHitCountReached event and reset it back to 0
                    if (_counter.Value == HitCountMax)
                    {
                        OnInputHitCountReached?.Invoke(this, new EventArgs());

                        //If the reset mode is set to auto, reset the hit counter
                        if (HitCountResetMode == ResetType.Auto)
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
            //Check to see if the button release buttonDownTimeOut has elapsed, if so invoke the OnInputReleasedTimeOut event
            if (_inputReleasedTimer.ElapsedMS >= InputReleasedTimeout)
            {
                //If the reset mode is set to auto, reset the time elapsed
                if(ReleasedElapsedResetMode == ResetType.Auto)
                    //Reset the timer
                    _inputReleasedTimer.Reset();

                //Invoke the event
                OnInputReleasedTimeOut?.Invoke(this, new EventArgs());
            }

            //Check to see if the button is pressed
            if (InternalMouse.IsButtonDown((int)Button))
            {
                _inputReleasedTimer.Reset();

                _inputDownTimer.Start();

                //If the set time in milliseconds has elapsed
                if (InputDownElapsedMS >= InputDownTimeOut)
                {
                    //If the reset mode is set to auto, reset the time elapsed
                    if (DownElapsedResetMode == ResetType.Auto)
                        _inputDownTimer.Reset();

                    OnInputDownTimeOut?.Invoke(this, new EventArgs());
                }
            }

            //If the current state is up and the previous was down, and the reset on button release is enabled,
            //then reset the elapsed time
            if (!_curState && _prevState)
            {
                //If the reset mode is set to auto, reset the time elapsed
                _inputDownTimer.Reset();

                //Start the button release timer
                _inputReleasedTimer.Start();
            }
            #endregion

            #region Button Combo Code
            //If the button combo list is not null
            if (_currentPressedButtons != null)
            {
                //Holds the list of keys from the pressed buttons dictionary
                var buttons = new List<InputButton>(_currentPressedButtons.Keys);

                //Set the state of all of the pressed buttons
                foreach (var key in buttons)
                {
                    _currentPressedButtons[key] = InternalMouse.IsButtonDown((int)key);
                }

                //If all of the buttons are pressed down
                if (_currentPressedButtons.All(button => button.Value))
                    OnInputComboPressed?.Invoke(this, new EventArgs());
            }
            #endregion

            InternalMouse.UpdatePreviousState();

            _prevState = _curState;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Initializes the MouseWatcher settings.
        /// </summary>
        /// <param name="hitCountMax">The total amount of times the button will be hit before invoking an event.</param>
        /// <param name="button">The button to watch.</param>
        /// <param name="comboButtons">The list of combo buttons to press in combination to invoke the event.</param>
        /// <param name="buttonDownTimeOut">The amount of time in milliseconds the button should be in the down position before invoking the OnInputTimeOut event.</param>
        /// <param name="buttonReleaseTimeOut">Sets the time in milliseconds that the given button should be released before the OnInputReleasedTimeOut event will be invoked.</param>
        /// <param name="enabled">Set to true or false to enable or disable the watcher.</param>
        private void Init(int hitCountMax, InputButton button, List<InputButton> comboButtons,  int buttonDownTimeOut, int buttonReleaseTimeOut, bool enabled = true)
        {
            Button = button;

            //If the combo buttons are null, skip combo button setup
            if (comboButtons != null)
                CreateCurrentPressedButtons(comboButtons.ToArray());

            if (hitCountMax > 0) HitCountMax = hitCountMax;

            InputDownTimeOut = buttonDownTimeOut;

            InputReleasedTimeout = buttonReleaseTimeOut;

            Enabled = enabled;

            DownElapsedResetMode = ResetType.Auto;
            ReleasedElapsedResetMode = ResetType.Auto;
            HitCountResetMode = ResetType.Auto;
        }


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
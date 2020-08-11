// <copyright file="MouseWatcher.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Input
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using KDScorpionEngine.Utils;
    using Raptor;
    using Raptor.Input;
    using Raptor.Plugins;

    /// <summary>
    /// Watches a mouse button for various events and behaviors such is how many times a button is pressed,
    /// how long it is held down or how long it has been released.  Various events will be triggered when
    /// these behaviours occur.
    /// </summary>
    public class MouseWatcher : IInputWatcher, IUpdatable
    {
        /// <summary>
        /// Occurs when the combo button setup has been pressed.
        /// </summary>
        public event EventHandler OnInputComboPressed;

        /// <summary>
        /// Occurs when the set mouse button has been held in the down position for a set amount of time.
        /// </summary>
        public event EventHandler OnInputDownTimeOut;

        /// <summary>
        /// Occurs when the set mouse button has been hit a set amount of times.
        /// </summary>
        public event EventHandler OnInputHitCountReached;

        /// <summary>
        /// Occurs when the set mouse button has been released from the down position for a set amount of time.
        /// </summary>
        public event EventHandler OnInputReleasedTimeOut;

        private readonly Mouse _mouse;
        private Dictionary<InputButton, bool> _currentPressedButtons;//Holds the list of combo buttons and there down states
        private StopWatch _buttonDownTimer;//Keeps track of how long the set input has been in the down position
        private StopWatch _buttonReleaseTimer;//Keeps track of how long the set input has been in the up position
        private Counter _counter;//Keeps track of the hit count of an input
        private bool _curState;//The current state of the set input
        private bool _prevState;//The previous state of the set input

        /// <summary>
        /// Creates a new instance of <see cref="MouseWatcher"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="enabled">True if the watcher should be enabled.</param>
        /// <param name="mouse">The mouse to inject.</param>
        internal MouseWatcher(bool enabled, IMouse mouse)
        {
            _mouse = new Mouse(mouse);
            Setup(enabled);
        }

        /// <summary>
        /// Creates an instance of <see cref="MouseWatcher"/>.
        /// </summary>
        /// <param name="enabled">Set to true to enable the watcher.</param>
        [ExcludeFromCodeCoverage]
        public MouseWatcher(bool enabled)
        {
            _mouse = new Mouse();
            Setup(enabled);
        }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="MouseWatcher"/> is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the list of combo buttons.
        /// </summary>
        public List<InputButton> ComboButtons
        {
            get => _currentPressedButtons.Keys.ToList();
            set => CreateCurrentPressedButtons(value);
        }

        /// <summary>
        /// Gets or sets the button to watch.
        /// </summary>
        public InputButton Button { get; set; } = InputButton.None;

        /// <summary>
        /// Gets current amount of times that the set button has been hit.
        /// </summary>
        public int CurrentHitCount => _counter.Value;

        /// <summary>
        /// Gets the current hit percentage that the button has been hit out of the total number of maximum tims to be hit.
        /// </summary>
        public int CurrentHitCountPercentage => (int)(CurrentHitCount / (float)HitCountMax * 100f);

        /// <summary>
        /// Gets or sets the reset mode that the watcher will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the amount of time the button is in the down position.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        public ResetType DownElapsedResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Gets or sets the maximum amount of times that the set mouse button should be hit before 
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
        /// Gets the amount of time in milliseconds that has elapsed that the button has been held in the down position.
        /// </summary>
        public int InputDownElapsedMS => _buttonDownTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the button has been held in the down position.
        /// </summary>
        public float InputDownElapsedSeconds => _buttonReleaseTimer.ElapsedSeconds;

        /// <summary>
        /// The amount of time in milliseconds that the button should be held down before invoking the <see cref="OnInputDownTimeOut"/> event.
        /// </summary>
        public int InputDownTimeOut
        {
            get => _buttonDownTimer.TimeOut;
            set => _buttonDownTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the button has been released and is in the up position.
        /// </summary>
        public int InputReleasedElapsedMS => _buttonReleaseTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the button has been released and is in the up position.
        /// </summary>
        public float InputReleasedElapsedSeconds => _buttonReleaseTimer.ElapsedSeconds;

        /// <summary>
        /// The amount of time in milliseconds that the button should be released to the up position
        /// after being released from the down position before invoking the <see cref="OnInputReleasedTimeOut"/> event.
        /// </summary>
        public int InputReleasedTimeout
        {
            get => _buttonReleaseTimer.TimeOut;
            set => _buttonReleaseTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets or sets the reset mode that the watcher's button released functionality will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the button being released.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        public ResetType ReleasedElapsedResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Updates the <see cref="MouseWatcher"/>.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
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
            _curState = _mouse.IsButtonDown(Button);

            #region Hit Count Code
            //If the counter is not null
            if (_mouse.IsButtonPressed(Button))
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
            if (_mouse.IsButtonDown(Button))
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
                buttons.ForEach(b => _currentPressedButtons[b] = _mouse.IsButtonDown(b));
                
                //If all of the buttons are pressed down
                if (_currentPressedButtons.Count > 0 && _currentPressedButtons.All(button => button.Value))
                    OnInputComboPressed?.Invoke(this, new EventArgs());
            }
            #endregion

            _mouse.UpdatePreviousState();

            _prevState = _curState;
        }

        /// <summary>
        /// Occurs when the button has been held down for a set amount of time.
        /// </summary>
        private void ButtonDownTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (_mouse.IsButtonDown(Button))
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                    _buttonDownTimer.Reset();

                OnInputDownTimeOut?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs when the button has been released from the down position for a set amount of time.
        /// </summary>
        private void ButtonReleasedTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (_mouse.IsButtonUp(Button))
            {
                //If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                    _buttonReleaseTimer.Reset();

                //Invoke the event
                OnInputReleasedTimeOut?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Sets up the <see cref="MouseWatcher"/>.
        /// </summary>
        /// <param name="enabled">If true, the mouse watcher will be enabled.</param>
        private void Setup(bool enabled)
        {
            Enabled = enabled;
            ComboButtons = new List<InputButton>();

            //Setup stop watches
            _counter = new Counter(0, 10, 1);
            _buttonDownTimer = new StopWatch(1000);
            _buttonDownTimer.OnTimeElapsed += ButtonDownTimer_OnTimeElapsed;
            _buttonDownTimer.Start();

            _buttonReleaseTimer = new StopWatch(1000);
            _buttonReleaseTimer.OnTimeElapsed += ButtonReleasedTimer_OnTimeElapsed;
            _buttonReleaseTimer.Start();
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
                buttons.ToList().ForEach(b =>
                {
                    //If the button has not alredy been added
                    if (!_currentPressedButtons.ContainsKey(b))
                        _currentPressedButtons.Add(b, false);
                });
            }
        }
    }
}

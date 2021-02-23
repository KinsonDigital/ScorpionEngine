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
    using Raptor.Input;

    /// <summary>
    /// Watches a mouse button for various events and behaviors such is how many times a button is pressed,
    /// how long it is held down or how long it has been released.  Various events will be triggered when
    /// these behaviours occur.
    /// </summary>
    public class MouseWatcher : IInputWatcher, IUpdatableObject
    {
        private Dictionary<MouseButton, bool> currentPressedButtons; // Holds the list of combo buttons and there down states
        private StopWatch buttonDownTimer; // Keeps track of how long the set input has been in the down position
        private StopWatch buttonReleaseTimer; // Keeps track of how long the set input has been in the up position
        private Counter counter; // Keeps track of the hit count of an input
        private bool curState; // The current state of the set input
        private MouseState previousMouseState;
        private bool prevState; // The previous state of the set input
        private MouseState currentMouseState;
        private readonly IMouse mouse;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseWatcher"/> class.
        /// </summary>
        /// <param name="enabled">Set to true to enable the watcher.</param>
        /// <param name="mouse">Manages mouse input.</param>
        [ExcludeFromCodeCoverage]
        public MouseWatcher(bool enabled, IMouse mouse)
        {
            Setup(enabled);
            this.mouse = mouse;
        }

        /// <summary>
        /// Occurs when the combo button setup has been pressed.
        /// </summary>
        public event EventHandler<EventArgs>? OnInputComboPressed;

        /// <summary>
        /// Occurs when the set mouse button has been held in the down position for a set amount of time.
        /// </summary>
        public event EventHandler<EventArgs>? OnInputDownTimeOut;

        /// <summary>
        /// Occurs when the set mouse button has been hit a set amount of times.
        /// </summary>
        public event EventHandler<EventArgs>? OnInputHitCountReached;

        /// <summary>
        /// Occurs when the set mouse button has been released from the down position for a set amount of time.
        /// </summary>
        public event EventHandler<EventArgs>? OnInputReleasedTimeOut;

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="MouseWatcher"/> is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the list of combo buttons.
        /// </summary>
        public List<MouseButton> ComboButtons
        {
            get => this.currentPressedButtons.Keys.ToList();
            set => CreateCurrentPressedButtons(value);
        }

        /// <summary>
        /// Gets or sets the button to watch.
        /// </summary>
        public MouseButton Button { get; set; } = MouseButton.None;

        /// <summary>
        /// Gets current amount of times that the set button has been hit.
        /// </summary>
        public int CurrentHitCount => this.counter.Value;

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
            get => this.counter.Max;
            set => this.counter.Max = value;
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
        public int InputDownElapsedMS => this.buttonDownTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the button has been held in the down position.
        /// </summary>
        public float InputDownElapsedSeconds => this.buttonReleaseTimer.ElapsedSeconds;

        /// <summary>
        /// The amount of time in milliseconds that the button should be held down before invoking the <see cref="OnInputDownTimeOut"/> event.
        /// </summary>
        public int InputDownTimeOut
        {
            get => this.buttonDownTimer.TimeOut;
            set => this.buttonDownTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the button has been released and is in the up position.
        /// </summary>
        public int InputReleasedElapsedMS => this.buttonReleaseTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the button has been released and is in the up position.
        /// </summary>
        public float InputReleasedElapsedSeconds => this.buttonReleaseTimer.ElapsedSeconds;

        /// <summary>
        /// The amount of time in milliseconds that the button should be released to the up position
        /// after being released from the down position before invoking the <see cref="OnInputReleasedTimeOut"/> event.
        /// </summary>
        public int InputReleasedTimeout
        {
            get => this.buttonReleaseTimer.TimeOut;
            set => this.buttonReleaseTimer.TimeOut = value;
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
        /// <param name="gameTime">The game engine time.</param>
        public void Update(GameTime gameTime)
        {
            // If disabled, exit
            if (!Enabled)
            {
                return;
            }

            // Update the current state of the mouse
            this.currentMouseState = this.mouse.GetMouseState();

            // Update the mouse button down timer to keep track of how much time that the button has been in the down position
            this.buttonDownTimer.Update(gameTime);

            // Update the mouse button release timer to keep track of how much time that the button has been in the
            // up position since its release
            this.buttonReleaseTimer.Update(gameTime);

            // Get the current state of the button
            this.curState = this.currentMouseState.GetButtonState(Button);

            // Hit Count Code
            // If the counter is not null
            if (this.currentMouseState.GetButtonState(Button) && this.previousMouseState.GetButtonState(Button) is false)
            {
                // If the max is reached, invoke the OnInputHitCountReached event and reset it back to 0
                if (this.counter != null && this.counter.Value == HitCountMax)
                {
                    OnInputHitCountReached?.Invoke(this, new EventArgs());

                    // If the reset mode is set to auto, reset the hit counter
                    if (HitCountResetMode == ResetType.Auto)
                    {
                        this.counter.Reset();
                    }
                }
                else
                {
                    this.counter?.Count(); // Increment the current hit count
                }
            }

            // Timing Code
            // As long as the button is down, continue to keep the button release timer reset to 0
            if (this.currentMouseState.GetButtonState(Button))
            {
                this.buttonReleaseTimer.Reset();
            }

            // If the button is not pressed down and the button was pressed down last frame,
            // reset the input down timer and start the button release timer.
            if (!this.curState && this.prevState)
            {
                this.buttonDownTimer.Reset();
                this.buttonReleaseTimer.Start();
            }

            // Button Combo Code
            // If the button combo list is not null
            if (this.currentPressedButtons != null)
            {
                //TODO: This needs to be figured out

                //// Holds the list of keys from the pressed buttons dictionary
                //var buttons = new List<MouseButton>(this.currentPressedButtons.Keys);

                //// Set the state of all of the pressed buttons
                //buttons.ForEach(b => this.currentPressedButtons[b] = this.currentMouseState.SetButtonState(b));

                //// If all of the buttons are pressed down
                //if (this.currentPressedButtons.Count > 0 && this.currentPressedButtons.All(button => button.Value))
                //{
                //    OnInputComboPressed?.Invoke(this, new EventArgs());
                //}
            }

            this.previousMouseState = this.currentMouseState;

            this.prevState = this.curState;
        }

        /// <summary>
        /// Occurs when the button has been held down for a set amount of time.
        /// </summary>
        private void ButtonDownTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (this.currentMouseState.GetButtonState(Button))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                {
                    this.buttonDownTimer.Reset();
                }

                OnInputDownTimeOut?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs when the button has been released from the down position for a set amount of time.
        /// </summary>
        private void ButtonReleasedTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (this.currentMouseState.GetButtonState(Button) is false)
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                {
                    this.buttonReleaseTimer.Reset();
                }

                // Invoke the event
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
            ComboButtons = new List<MouseButton>();

            // Setup stop watches
            this.counter = new Counter(0, 10, 1);
            this.buttonDownTimer = new StopWatch(1000);
            this.buttonDownTimer.OnTimeElapsed += ButtonDownTimer_OnTimeElapsed;
            this.buttonDownTimer.Start();

            this.buttonReleaseTimer = new StopWatch(1000);
            this.buttonReleaseTimer.OnTimeElapsed += ButtonReleasedTimer_OnTimeElapsed;
            this.buttonReleaseTimer.Start();
        }

        /// <summary>
        /// Creates the list of pressed buttons from the given list of buttons.
        /// </summary>
        /// <param name="buttons">The list of combo buttons.</param>
        private void CreateCurrentPressedButtons(IList<MouseButton> buttons)
        {
            // If the combo buttons are null, skip combo button setup
            if (buttons != null)
            {
                // Create the current pressed buttons dictionary
                this.currentPressedButtons = new Dictionary<MouseButton, bool>();

                // Add all of the buttons to the combo buttons list dictionary
                buttons.ToList().ForEach(b =>
                {
                    // If the button has not alredy been added
                    if (!this.currentPressedButtons.ContainsKey(b))
                    {
                        this.currentPressedButtons.Add(b, false);
                    }
                });
            }
        }
    }
}

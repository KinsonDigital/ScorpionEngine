// <copyright file="KeyboardWatcher.cs" company="KinsonDigital">
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
    /// Watches a keyboard key for various events and behaviors such is how many times a key is pressed,
    /// how long it is held down or how long it has been released.  Various events will be triggered when
    /// these behaviors occur.
    /// </summary>
    public class KeyboardWatcher : IInputWatcher, IUpdatable
    {
        private readonly Keyboard keyboard;
        private Dictionary<KeyCode, bool> currentPressedKeys; // Holds the list of comboKeys and there down states
        protected Counter counter; // Keeps track of the hit count of an input
        protected bool curState; // The current state of the set input
        protected bool prevState; // The previous state of the set input
        protected StopWatch keyDownTimer; // Keeps track of how long the set input has been in the down position
        protected StopWatch keyReleasedTimer; // Keeps track of how long the set input has been in the up position since it was in the down position

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardWatcher"/> class.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher when created.</param>
        [ExcludeFromCodeCoverage]
        public KeyboardWatcher(bool enabled)
        {
            this.keyboard = new Keyboard();
            Setup(enabled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardWatcher"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard to inject for testing purposes.</param>
        internal KeyboardWatcher(IKeyboard keyboard)
        {
            this.keyboard = new Keyboard(keyboard);
            Setup(true);
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="KeyboardWatcher"/> is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the list of combo keys.
        /// </summary>
        public List<KeyCode> ComboKeys
        {
            get => this.currentPressedKeys.Keys.ToList();
            set => CreateCurrentPressedKeys(value.ToArray());
        }

        /// <summary>
        /// Gets or sets the key to watch.
        /// </summary>
        public KeyCode Key { get; set; } = KeyCode.None;

        /// <summary>
        /// Gets current amount of times that the set key has been hit.
        /// </summary>
        public int CurrentHitCount => this.counter.Value;

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
        /// Gets the amount of time in milliseconds that has elapsed that the key has been held in the down position.
        /// </summary>
        public int InputDownElapsedMS => this.keyDownTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the key has been held in the down position.
        /// </summary>
        public float InputDownElapsedSeconds => this.keyDownTimer.ElapsedSeconds;

        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the key should be held down before invoking the <see cref="OnInputDownTimeOut"/> event.
        /// </summary>
        public int InputDownTimeOut
        {
            get => this.keyDownTimer.TimeOut;
            set => this.keyDownTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the key has been released and is in the up position.
        /// </summary>
        public int InputReleasedElapsedMS => this.keyReleasedTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the key has been released and is in the up position.
        /// </summary>
        public float InputReleasedElapsedSeconds => this.keyReleasedTimer.ElapsedSeconds;

        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the key should be released to the up position
        /// after being released from the down position before invoking the <see cref="OnInputReleasedTimeOut"/> event.
        /// </summary>
        public int InputReleasedTimeout
        {
            get => this.keyReleasedTimer.TimeOut;
            set => this.keyReleasedTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets or sets the reset mode that the watcher's key released functionality will operate in.
        /// <see cref="ResetType.Auto"/> will automatically reset the watcher for watching the key being released.
        /// <see cref="ResetType.Manual"/> will only be reset if manually done so.
        /// </summary>
        public ResetType ReleasedElapsedResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Update the watcher state.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public void Update(EngineTime engineTime)
        {
            // If disabled, exit
            if (!Enabled) return;

            // Update the current state of the keyboard
            this.keyboard.UpdateCurrentState();

            // Update the key down timer to keep track of how much time that the key has been in the down position
            this.keyDownTimer.Update(engineTime);

            // Update the key release timer to keep track of how much time that the key has been in the
            // up position since its release
            this.keyReleasedTimer.Update(engineTime);

            // Get the current state of the key
            this.curState = this.keyboard.IsKeyDown(Key);

            // Hit Count Code
            if (this.keyboard.IsKeyPressed(Key))
            {
                // If the max is reached, invoke the OnInputHitCountReached event and reset it back to 0
                if (this.counter != null && this.counter.Value == HitCountMax)
                {
                    OnInputHitCountReached?.Invoke(this, new EventArgs());

                    // If the reset mode is set to auto, reset the hit counter
                    if (HitCountResetMode == ResetType.Auto)
                        this.counter.Reset();
                }
                else
                {
                    this.counter?.Count(); // Increment the current hit count
                }
            }

            // Timing Code
            // As long as the key is down, continue to keep the key release timer reset to 0
            if (this.keyboard.IsKeyDown(Key))
                this.keyReleasedTimer.Reset();

            // If the key is not pressed down and the key was pressed down last frame,
            // reset the input down timer and start the key release timer.
            if (!this.curState && this.prevState)
            {
                this.keyDownTimer.Reset();
                this.keyReleasedTimer.Start();
            }

            // Key Combo Code
            // If the key combo list is not null
            if (this.currentPressedKeys != null)
            {
                // Holds the list of keys from the pressed keys dictionary
                var keys = new List<KeyCode>(this.currentPressedKeys.Keys);

                // Set the state of all of the pressed keys
                keys.ForEach(k => this.currentPressedKeys[k] = this.keyboard.IsKeyDown(k));

                // If all of the keys are pressed down
                if (this.currentPressedKeys.Count > 0 && this.currentPressedKeys.All(key => key.Value))
                    OnInputComboPressed?.Invoke(this, new EventArgs());
            }

            this.keyboard.UpdatePreviousState();

            this.prevState = this.curState;
        }

        /// <summary>
        /// Occurs when the key has been held down for a set amount of time.
        /// </summary>
        private void KeyUpTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (this.keyboard.IsKeyUp(Key))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                    this.keyReleasedTimer.Reset();

                OnInputReleasedTimeOut?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs when the key has been released from the down position for a set amount of time.
        /// </summary>
        private void KeyDownTimer_OnTimeElapsed(object sender, EventArgs e)
        {
            if (this.keyboard.IsKeyDown(Key))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                    this.keyDownTimer.Reset();

                // Invoke the event
                OnInputDownTimeOut?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Sets up the <see cref="KeyboardWatcher"/>.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher when created.</param>
        private void Setup(bool enabled)
        {
            Enabled = enabled;
            ComboKeys = new List<KeyCode>();

            // Setup stop watches
            this.counter = new Counter(0, 10, 1);
            this.keyDownTimer = new StopWatch(1000);
            this.keyDownTimer.OnTimeElapsed += KeyDownTimer_OnTimeElapsed;
            this.keyDownTimer.Start();

            this.keyReleasedTimer = new StopWatch(1000);
            this.keyReleasedTimer.OnTimeElapsed += KeyUpTimer_OnTimeElapsed;
            this.keyReleasedTimer.Start();
        }

        /// <summary>
        /// Creates the list of pressed keys from the given list of keys.
        /// </summary>
        /// <param name="keys">The list of combo keys.</param>
        private void CreateCurrentPressedKeys(IList<KeyCode> keys)
        {
            // If the combo keys are null, skip combo key setup
            if (keys != null)
            {
                // Create the current pressed keys dictionary
                this.currentPressedKeys = new Dictionary<KeyCode, bool>();

                // Add all of the keys to the combo keys list dictionary
                keys.ToList().ForEach(k =>
                {
                    // If the key has not already been added
                    if (!this.currentPressedKeys.ContainsKey(k))
                        this.currentPressedKeys.Add(k, false);
                });
            }
        }
    }
}

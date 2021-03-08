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
    using Raptor.Input;

    /// <summary>
    /// Watches a keyboard key for various events and behaviors such is how many times a key is pressed,
    /// how long it is held down or how long it has been released.  Various events will be triggered when
    /// these behaviors occur.
    /// </summary>
    public class KeyboardWatcher : IInputWatcher, IUpdatableObject, IDisposable
    {
        private readonly IGameInput<KeyCode, KeyboardState> keyboard;
        private readonly IStopWatch keyDownTimer; // Keeps track of how long the set input has been in the down position
        private readonly IStopWatch keyReleaseTimer; // Keeps track of how long the set input has been in the up position since it was in the down position
        private readonly ICounter counter; // Keeps track of the hit count of an input
        private readonly Dictionary<KeyCode, bool> currentPressedKeys = new Dictionary<KeyCode, bool>(); // Holds the list of comboKeys and there down states
        private bool curState; // The current state of the set input
        private KeyboardState previousKeyboardState;
        private bool prevState; // The previous state of the set input
        private KeyboardState currentKeyboardState;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardWatcher"/> class.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher when created.</param>
        /// <param name="keyboard">Manages the keyboard.</param>
        /// <param name="keyDownTimer">Tracks how much time a keyboard is in the down position.</param>
        /// <param name="keyReleaseTimer">Tracks how much time a keyboard key is in the up position.</param>
        /// <param name="counter">Keeps track of keyboard usage.</param>
        [ExcludeFromCodeCoverage]
        public KeyboardWatcher(
            bool enabled,
            IGameInput<KeyCode, KeyboardState> keyboard,
            IStopWatch keyDownTimer,
            IStopWatch keyReleaseTimer,
            ICounter counter)
        {
            this.keyboard = keyboard;
            this.keyDownTimer = keyDownTimer;
            this.keyReleaseTimer = keyReleaseTimer;
            this.counter = counter;

            Setup(enabled);
        }

        /// <summary>
        /// Occurs when the combo key setup has been pressed.
        /// </summary>
        public event EventHandler<EventArgs>? InputComboPressed;

        /// <summary>
        /// Occurs when the set keyboard key has been held in the down position for a set amount of time.
        /// </summary>
        public event EventHandler<EventArgs>? InputDownTimedOut;

        /// <summary>
        /// Occurs when the set keyboard key has been hit a set amount of times.
        /// </summary>
        public event EventHandler<EventArgs>? InputHitCountReached;

        /// <summary>
        /// Occurs when the set keyboard key has been released from the down position for a set amount of time.
        /// </summary>
        public event EventHandler<EventArgs>? InputReleaseTimedOut;

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
        public KeyCode Key { get; set; } = KeyCode.Unknown;

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
        /// <remarks>The default setting is <see cref="ResetType.Auto"/>.</remarks>
        public ResetType DownElapsedResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Gets or sets the maximum amount of times that the set keyboard key should be hit before
        /// invoking the <see cref="InputHitCountReached"/> event is reached.
        /// </summary>
        /// <remarks>Default value is 10.</remarks>
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
        /// Gets or sets the amount of time in milliseconds that the key should be held down before invoking the <see cref="DownTimeOut"/> event.
        /// </summary>
        public int DownTimeOut
        {
            get => this.keyDownTimer.TimeOut;
            set => this.keyDownTimer.TimeOut = value;
        }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed that the key has been released and is in the up position.
        /// </summary>
        public int InputReleasedElapsedMS => this.keyReleaseTimer.ElapsedMS;

        /// <summary>
        /// Gets the amount of time in seconds that has elapsed that the key has been released and is in the up position.
        /// </summary>
        public float InputReleasedElapsedSeconds => this.keyReleaseTimer.ElapsedSeconds;

        /// <summary>
        /// Gets or sets the amount of time in milliseconds that the key should be released to the up position
        /// after being released from the down position before invoking the <see cref="InputReleaseTimedOut"/> event.
        /// </summary>
        public int ReleaseTimeOut
        {
            get => this.keyReleaseTimer.TimeOut;
            set => this.keyReleaseTimer.TimeOut = value;
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
        /// <param name="gameTime">The game engine time.</param>
        public void Update(GameTime gameTime)
        {
            // If disabled, exit
            if (!Enabled)
            {
                return;
            }

            // Update the current state of the keyboard
            this.currentKeyboardState = this.keyboard.GetState();

            // Update the key down timer to keep track of how much time that the key has been in the down position
            this.keyDownTimer.Update(gameTime);

            // Update the key release timer to keep track of how much time that the key has been in the
            // up position since its release
            this.keyReleaseTimer.Update(gameTime);

            // Get the current state of the key
            this.curState = this.currentKeyboardState.IsKeyDown(Key);

            // Hit Count
            if (this.currentKeyboardState.IsKeyUp(Key) && this.previousKeyboardState.IsKeyDown(Key))
            {
                this.counter.Count(); // Increment the current hit count
            }

            // Timing Code
            // As long as the key is down, continue to keep the key release timer reset to 0
            if (this.currentKeyboardState.IsKeyDown(Key))
            {
                this.keyReleaseTimer.Reset();
            }

            // If the key is not pressed down and the key was pressed down last frame,
            // reset the input down timer and start the key release timer.
            if (!this.curState && this.prevState)
            {
                this.keyDownTimer.Reset();
                this.keyReleaseTimer.Start();
            }

            // Key Combo Code
            // If the key combo list is not null
            if (this.currentPressedKeys != null)
            {
                // Holds the list of keys from the pressed keys dictionary
                var keys = new List<KeyCode>(this.currentPressedKeys.Keys);

                // Set the state of all of the pressed keys
                keys.ForEach(k => this.currentPressedKeys[k] = this.currentKeyboardState.IsKeyDown(k));

                // If all of the keys are pressed down
                if (this.currentPressedKeys.Count > 0 && this.currentPressedKeys.All(key => key.Value))
                {
                    InputComboPressed?.Invoke(this, new EventArgs());
                }
            }

            this.previousKeyboardState = this.currentKeyboardState;

            this.prevState = this.curState;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.counter.MaxReachedWhenIncrementing -= Counter_MaxReachedWhenIncrementing;
                    this.keyDownTimer.TimeElapsed -= KeyDownTimer_TimeElapsed;
                    this.keyReleaseTimer.TimeElapsed -= KeyUpTimer_TimeElapsed;
                }

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Occurs when the total number of times the <see cref="Key"/> has been pressed
        /// the same amount of times of <see cref="HitCountMax"/>.
        /// </summary>
        private void Counter_MaxReachedWhenIncrementing(object? sender, EventArgs e)
        {
            InputHitCountReached?.Invoke(this, new EventArgs());

            // If the reset mode is set to auto, reset the hit counter
            if (HitCountResetMode == ResetType.Auto)
            {
                this.counter.Reset();
            }
        }

        /// <summary>
        /// Occurs when the key has been released from the down position for a set amount of time.
        /// </summary>
        private void KeyDownTimer_TimeElapsed(object? sender, EventArgs e)
        {
            if (this.currentKeyboardState.IsKeyDown(Key))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                {
                    this.keyDownTimer.Reset();
                }

                // Invoke the event
                InputDownTimedOut?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs when the key has been held down for a set amount of time.
        /// </summary>
        private void KeyUpTimer_TimeElapsed(object? sender, EventArgs e)
        {
            if (this.currentKeyboardState.IsKeyUp(Key))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                {
                    this.keyReleaseTimer.Reset();
                }

                InputReleaseTimedOut?.Invoke(this, new EventArgs());
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

            this.counter.MaxReachedWhenIncrementing += Counter_MaxReachedWhenIncrementing;

            // Setup stop watches
            this.keyDownTimer.TimeOut = 1000;
            this.keyDownTimer.TimeElapsed += KeyDownTimer_TimeElapsed;
            this.keyDownTimer.Start();

            this.keyReleaseTimer.TimeOut = 1000;
            this.keyReleaseTimer.TimeElapsed += KeyUpTimer_TimeElapsed;
            this.keyReleaseTimer.Start();
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
                // Add all of the keys to the combo keys list dictionary
                keys.ToList().ForEach(k =>
                {
                    // If the key has not already been added
                    if (!this.currentPressedKeys.ContainsKey(k))
                    {
                        this.currentPressedKeys.Add(k, false);
                    }
                });
            }
        }
    }
}

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
    public class KeyboardWatcher : GameInputWatcher<KeyCode>, IDisposable
    {
        private readonly IGameInput<KeyCode, KeyboardState> keyboard;
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardWatcher"/> class.
        /// </summary>
        /// <param name="enabled">Set to true or false to enable or disable the watcher when created.</param>
        /// <param name="keyboard">Manages the keyboard.</param>
        /// <param name="inputDownTimer">Tracks how much time a keyboard is in the down position.</param>
        /// <param name="inputReleaseTimer">Tracks how much time a keyboard key is in the up position.</param>
        /// <param name="counter">Keeps track of keyboard usage.</param>
        [ExcludeFromCodeCoverage]
        public KeyboardWatcher(
            IGameInput<KeyCode, KeyboardState> keyboard,
            IStopWatch inputDownTimer,
            IStopWatch inputReleaseTimer,
            ICounter counter)
                : base(inputDownTimer, inputReleaseTimer, counter)
        {
            this.keyboard = keyboard;

            Input = KeyCode.Unknown;
            Counter.MaxReachedWhenIncrementing += Counter_MaxReachedWhenIncrementing;

            // Setup stop watches
            InputDownTimer.TimeOut = 1000;
            InputDownTimer.TimeElapsed += KeyDownTimer_TimeElapsed;
            InputDownTimer.Start();

            InputReleaseTimer.TimeOut = 1000;
            InputReleaseTimer.TimeElapsed += KeyUpTimer_TimeElapsed;
            InputReleaseTimer.Start();
        }

        /// <summary>
        /// Update the watcher state.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public override void Update(GameTime gameTime)
        {
            // If disabled, exit
            if (!Enabled)
            {
                return;
            }

            // Update the current state of the keyboard
            this.currentKeyboardState = this.keyboard.GetState();

            // Update the key down timer to keep track of how much time that the key has been in the down position
            InputDownTimer.Update(gameTime);

            // Update the key release timer to keep track of how much time that the key has been in the
            // up position since its release
            InputReleaseTimer.Update(gameTime);

            // Get the current state of the key
            CurrentState = this.currentKeyboardState.IsKeyDown(Input);

            // Hit Count
            if (this.currentKeyboardState.IsKeyUp(Input) && this.previousKeyboardState.IsKeyDown(Input))
            {
                Counter.Count(); // Increment the current hit count
            }

            // Timing Code
            // As long as the key is down, continue to keep the key release timer reset to 0
            if (CurrentState)
            {
                InputReleaseTimer.Reset();
                OnInputDown();
            }

            // If the key is not pressed down and the key was pressed down last frame,
            // reset the input down timer and start the key release timer.
            if (!CurrentState && PreviousState)
            {
                InputDownTimer.Reset();
                InputReleaseTimer.Start();
                OnInputReleased();
            }

            // Key Combo Code
            // If the key combo list is not null
            if (CurrentPressedInputs != null)
            {
                // Holds the list of keys from the pressed keys dictionary
                var keys = new List<KeyCode>(CurrentPressedInputs.Keys);

                // Set the state of all of the pressed keys
                keys.ForEach(k => CurrentPressedInputs[k] = this.currentKeyboardState.IsKeyDown(k));

                var downKeys = CurrentPressedInputs.All(key => key.Value);

                // If all of the keys are pressed down
                if (CurrentPressedInputs.Count > 0 && CurrentPressedInputs.All(key => key.Value))
                {
                    OnInputComboPressed();
                }
            }

            this.previousKeyboardState = this.currentKeyboardState;

            PreviousState = CurrentState;
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
                    Counter.MaxReachedWhenIncrementing -= Counter_MaxReachedWhenIncrementing;
                    InputDownTimer.TimeElapsed -= KeyDownTimer_TimeElapsed;
                    InputReleaseTimer.TimeElapsed -= KeyUpTimer_TimeElapsed;
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
            OnInputHitCountReached();

            // If the reset mode is set to auto, reset the hit counter
            if (HitCountResetMode == ResetType.Auto)
            {
                Counter.Reset();
            }
        }

        /// <summary>
        /// Occurs when the key has been released from the down position for a set amount of time.
        /// </summary>
        private void KeyDownTimer_TimeElapsed(object? sender, EventArgs e)
        {
            if (this.currentKeyboardState.IsKeyDown(Input))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                {
                    InputDownTimer.Reset();
                }

                OnInputDownTimedOut();
            }
        }

        /// <summary>
        /// Occurs when the key has been held down for a set amount of time.
        /// </summary>
        private void KeyUpTimer_TimeElapsed(object? sender, EventArgs e)
        {
            if (this.currentKeyboardState.IsKeyUp(Input))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                {
                    InputReleaseTimer.Reset();
                }

                OnInputReleaseTimedOut();
            }
        }
    }
}

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
    public class MouseWatcher : GameInputWatcher<MouseButton>, IDisposable
    {
        private readonly IGameInput<MouseButton, MouseState> gameInput;
        private MouseState previousMouseState;
        private MouseState currentMouseState;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseWatcher"/> class.
        /// </summary>
        /// <param name="enabled">Set to true to enable the watcher.</param>
        /// <param name="gameInput">Manages mouse input.</param>
        /// <param name="inputDownTimer">Keeps track of how long the set button has been in the down position.</param>
        /// <param name="inputReleaseTimer">Keeps track of how long the set button has been in the up position.</param>
        /// <param name="counter">Keeps track of mouse input usage.</param>
        [ExcludeFromCodeCoverage]
        public MouseWatcher(
            IGameInput<MouseButton, MouseState> gameInput,
            IStopWatch inputDownTimer,
            IStopWatch inputReleaseTimer,
            ICounter counter)
                : base(
                      inputDownTimer,
                      inputReleaseTimer,
                      counter)

        {
            this.gameInput = gameInput;

            Input = MouseButton.None;

            Counter.MaxReachedWhenIncrementing += Counter_MaxReachedWhenIncrementing;

            // Setup stop watches
            InputDownTimer.TimeOut = 1000;
            InputDownTimer.TimeElapsed += ButtonDownTimer_OnTimeElapsed;
            InputDownTimer.Start();

            InputReleaseTimer.TimeOut = 1000;
            InputReleaseTimer.TimeElapsed += ButtonReleasedTimer_OnTimeElapsed;
            InputReleaseTimer.Start();
        }

        /// <summary>
        /// Updates the <see cref="MouseWatcher"/>.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public override void Update(GameTime gameTime)
        {
            // If disabled, exit
            if (!Enabled)
            {
                return;
            }

            //TODO: Move some of this update logic to the base class.
            // Don't do this until the code is refactored and tests are put into place first

            // Update the current state of the mouse
            this.currentMouseState = this.gameInput.GetState();

            // Update the mouse button down timer to keep track of how much time that the button has been in the down position
            InputDownTimer.Update(gameTime);

            // Update the mouse button release timer to keep track of how much time that the button has been in the
            // up position since its release
            InputReleaseTimer.Update(gameTime);

            // Get the current state of the button
            CurrentState = this.currentMouseState.GetButtonState(Input);

            // Hit Count Code
            // If the counter is not null
            if (this.currentMouseState.GetButtonState(Input) && this.previousMouseState.GetButtonState(Input) is false)
            {
                Counter.Count(); // Increment the current hit count
            }

            // Timing Code
            // As long as the button is down, continue to keep the button release timer reset to 0
            if (this.currentMouseState.GetButtonState(Input))
            {
                InputReleaseTimer.Reset();
            }

            // If the button is not pressed down and the button was pressed down last frame,
            // reset the input down timer and start the button release timer.
            if (!CurrentState && PreviousState)
            {
                InputDownTimer.Reset();
                InputReleaseTimer.Start();
            }

            // Button Combo Code
            // If the button combo list is not null
            if (CurrentPressedInputs != null)
            {
                // Holds the list of keys from the pressed buttons dictionary
                var buttons = new List<MouseButton>(CurrentPressedInputs.Keys);

                // Set the state of all of the pressed buttons
                buttons.ForEach(b => CurrentPressedInputs[b] = this.currentMouseState.GetButtonState(b));

                // If all of the buttons are pressed down
                if (CurrentPressedInputs.Count > 0 && CurrentPressedInputs.All(button => button.Value))
                {
                    OnInputComboPressed();
                }
            }

            this.previousMouseState = this.currentMouseState;

            PreviousState = CurrentState;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    Counter.MaxReachedWhenIncrementing -= Counter_MaxReachedWhenIncrementing;
                    InputDownTimer.TimeElapsed -= ButtonDownTimer_OnTimeElapsed;
                    InputReleaseTimer.TimeElapsed -= ButtonReleasedTimer_OnTimeElapsed;
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Occurs when the max hit count for the <see cref="Input"/> has been reached.
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
        /// Occurs when the button has been held down for a set amount of time.
        /// </summary>
        private void ButtonDownTimer_OnTimeElapsed(object? sender, EventArgs e)
        {
            if (this.currentMouseState.GetButtonState(Input))
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (DownElapsedResetMode == ResetType.Auto)
                {
                    InputDownTimer.Reset();
                }

                OnInputDownTimedOut();
            }
        }

        /// <summary>
        /// Occurs when the button has been released from the down position for a set amount of time.
        /// </summary>
        private void ButtonReleasedTimer_OnTimeElapsed(object? sender, EventArgs e)
        {
            if (this.currentMouseState.GetButtonState(Input) is false)
            {
                // If the reset mode is set to auto, reset the time elapsed
                if (ReleasedElapsedResetMode == ResetType.Auto)
                {
                    InputReleaseTimer.Reset();
                }

                OnInputReleaseTimedOut();
            }
        }
    }
}

// <copyright file="GameInputWatcher.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Input
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KDScorpionEngine.Utils;

    /// <summary>
    /// Watches game input for various events and behaviors such is how many times a button is pressed,
    /// how long it is held down, or how long it has been released.
    /// </summary>
    /// <typeparam name="TInputs">The type of input.</typeparam>
    /// <remarks>
    ///     Various events will be triggered when these behaviours occur.
    /// </remarks>
    public abstract class GameInputWatcher<TInputs> : IInputWatcher<TInputs>, IUpdatableObject
        where TInputs : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInputWatcher{TInputs}"/> class.
        /// </summary>
        /// <param name="inputDownTimer">Keeps track of how long an input is in the down state.</param>
        /// <param name="inputReleaseTimer">Keeps track of how long the input is in the released state.</param>
        /// <param name="counter">Keeps track of the how many times the <see cref="Input"/> has been pressed.</param>
        /// <remarks>
        ///     A pressed state represents an input that has been pressed into the down position and then released.
        /// </remarks>
        public GameInputWatcher(
            IStopWatch inputDownTimer,
            IStopWatch inputReleaseTimer,
            ICounter counter)
        {
            InputDownTimer = inputDownTimer;
            InputReleaseTimer = inputReleaseTimer;
            Counter = counter;
        }

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? InputComboPressed;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? InputDown;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? InputReleased;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? InputDownTimedOut;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? InputHitCountReached;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? InputReleaseTimedOut;

        /// <inheritdoc/>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the combination of inputs that all must be in down state
        /// for the <see cref="InputComboPressed"/> event will be invoked.
        /// </summary>
        public List<TInputs> ComboInputs
        {
            get => CurrentPressedInputs.Keys.ToList();
            set => CreateCurrentPressedButtons(value);
        }

        /// <inheritdoc/>
        public TInputs Input { get; set; }

        /// <inheritdoc/>
        public int CurrentHitCount => Counter.Value;

        /// <inheritdoc/>
        public int CurrentHitCountPercentage => (int)(CurrentHitCount / (float)HitCountMax * 100f);

        /// <inheritdoc/>
        public ResetType DownElapsedResetMode { get; set; } = ResetType.Auto;

        /// <inheritdoc/>
        public int HitCountMax
        {
            get => Counter.Max;
            set => Counter.Max = value;
        }

        /// <inheritdoc/>
        public ResetType HitCountResetMode { get; set; } = ResetType.Auto;

        /// <inheritdoc/>
        public int InputDownElapsedMS => InputDownTimer.ElapsedMS;

        /// <inheritdoc/>
        public float InputDownElapsedSeconds => InputDownTimer.ElapsedSeconds;

        /// <inheritdoc/>
        public int DownTimeOut
        {
            get => InputDownTimer.TimeOut;
            set => InputDownTimer.TimeOut = value;
        }

        /// <inheritdoc/>
        public int InputReleasedElapsedMS => InputReleaseTimer.ElapsedMS;

        /// <inheritdoc/>
        public float InputReleasedElapsedSeconds => InputReleaseTimer.ElapsedSeconds;

        /// <inheritdoc/>
        public int ReleaseTimeOut
        {
            get => InputReleaseTimer.TimeOut;
            set => InputReleaseTimer.TimeOut = value;
        }

        /// <inheritdoc/>
        public ResetType ReleasedElapsedResetMode { get; set; } = ResetType.Auto;

        /// <summary>
        /// Gets or sets the list of inputs and their down state.
        /// </summary>
        protected Dictionary<TInputs, bool> CurrentPressedInputs { get; set; } = new Dictionary<TInputs, bool>();

        /// <summary>
        /// Gets the counter that keeps track of how many times an input has been pressed.
        /// </summary>
        protected ICounter Counter { get; private set; }

        /// <summary>
        /// Gets the timer used to keep track of how long an input has been pressed down.
        /// </summary>
        protected IStopWatch InputDownTimer { get; private set; }

        /// <summary>
        /// Gets a timer used to keep track of how long an input has been released.
        /// </summary>
        protected IStopWatch InputReleaseTimer { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current state of the <see cref="Input"/> property.
        /// </summary>
        /// <remarks>
        ///     True if the <see cref="Input"/> is in the down state.
        /// </remarks>
        protected bool CurrentState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the previous state of the <see cref="Input"/> property.
        /// </summary>
        /// <remarks>
        ///     True if the <see cref="Input"/> is in the down state.
        /// </remarks>
        protected bool PreviousState { get; set; }

        /// <inheritdoc/>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Invokes the <see cref="InputDownTimedOut"/> event.
        /// </summary>
        protected void OnInputDownTimedOut() => InputDownTimedOut?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes the <see cref="InputDown"/> event.
        /// </summary>
        protected void OnInputDown() => InputDown?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes the <see cref="InputReleaseTimedOut"/> event.
        /// </summary>
        protected void OnInputReleaseTimedOut() => InputReleaseTimedOut?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes the <see cref="InputComboPressed"/> event.
        /// </summary>
        protected void OnInputComboPressed() => InputComboPressed?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes the <see cref="InputReleased"/> event.
        /// </summary>
        protected void OnInputReleased() => InputReleased?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Invokes the <see cref="InputHitCountReached"/> event.
        /// </summary>
        protected void OnInputHitCountReached() => InputHitCountReached?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Creates the list of pressed inputs from the given list of inputs.
        /// </summary>
        /// <param name="inputs">The list of combo inputs.</param>
        private void CreateCurrentPressedButtons(IList<TInputs> inputs)
        {
            // If the combo inputs are null, skip combo input setup
            if (inputs != null)
            {
                // Add all of the inputs to the combo inputs list dictionary
                inputs.ToList().ForEach(b =>
                {
                    // If the input has not alredy been added
                    if (!CurrentPressedInputs.ContainsKey(b))
                    {
                        CurrentPressedInputs.Add(b, false);
                    }
                });
            }
        }
    }
}

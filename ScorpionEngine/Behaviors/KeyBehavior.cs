// <copyright file="KeyBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;
    using Raptor.Input;

    /// <summary>
    /// The behavior of a single key on a keyboard.
    /// </summary>
    public class KeyBehavior : IBehavior
    {
        private readonly IGameInput<KeyCode, KeyboardState> keyboard;
        private int timeElapsed; // The time elapsed since last frame
        private KeyboardState currentState;
        private KeyboardState previousKeyboardState;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBehavior"/> class.
        /// </summary>
        /// <param name="key">The assigned keyboard key of the behavior.</param>
        /// <param name="keyboard">Manages keyboard input.</param>
        /// <param name="enabled">True to enable the behavior.</param>
        public KeyBehavior(KeyCode key, IGameInput<KeyCode, KeyboardState> keyboard, bool enabled = true)
        {
            this.keyboard = keyboard;
            Key = key;
            Enabled = enabled;
            ID = Guid.NewGuid();
        }

        /// <summary>
        /// Occurs when the <see cref="Key"/> has been pressed.
        /// </summary>
        public event EventHandler<KeyEventArgs>? KeyDownEvent;

        /// <summary>
        /// Occurs when the <see cref="Key"/> has been released.
        /// </summary>
        public event EventHandler<KeyEventArgs>? KeyUpEvent;

        /// <summary>
        /// Occurs when the <see cref="Key"/> has first been fully pressed down then released.
        /// </summary>
        public event EventHandler<KeyEventArgs>? KeyPressEvent;

        /// <summary>
        /// Gets or sets the key for the behavior.
        /// </summary>
        public KeyCode Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the behavior is enabled.
        /// </summary>
        /// <remarks>
        ///     If disabled, the update method will not update the behavior or fire events.
        /// </remarks>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the type of behavior of the KeyBehavior.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     KeyDownContinuous will fire the <see cref="KeyDownEvent"/> as long as the <see cref="Key"/> is pressed down.
        /// </para>
        ///
        /// <para>
        ///     OnceOnKeyPress will fire the <see cref="Key"/> only one time when the <see cref="Key"/> is pressed down.
        /// </para>
        ///
        /// <para>
        ///     OnceOnKeyRelease will fire the <see cref="Key"/> only one time when the <see cref="Key"/> is released.
        /// </para>
        /// </remarks>
        public KeyBehaviorType BehaviorType { get; set; } = KeyBehaviorType.OnceOnDown;

        /// <summary>
        /// Gets or sets the amount of time for a <see cref="KeyDownEvent"/> or <see cref="KeyUpEvent"/> event to be fired.
        /// </summary>
        public int TimeDelay { get; set; } = 1000;

        /// <summary>
        /// Gets a value indicating whether the <see cref="KeyDownEvent"/> should always be invoked.
        /// </summary>
        /// <remarks>
        ///     If set to true, the KeyDownEvent will be invoked no matter
        ///     what the <see cref="BehaviorType"/> is set to.
        /// </remarks>
        public bool AlwaysInvokeKeyDownEvent { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="KeyUpEvent"/> should always be invoked.
        /// The KeyUpEvent will be invoked no matter what <see cref="BehaviorType"/> is set.
        /// </summary>
        public bool AlwaysInvokeKeyUpEvent { get; } = true;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Key"/> is currently in the down position.
        /// </summary>
        public bool IsDown => this.currentState.IsKeyDown(Key);

        /// <inheritdoc/>
        public string Name { get; set; } = nameof(KeyBehavior);

        /// <inheritdoc/>
        public Guid ID { get; private set; }

        /// <summary>
        /// Updates the key behavior.
        /// </summary>
        /// <param name="gameTime">The game engine time.</param>
        public void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            this.timeElapsed += gameTime.CurrentFrameElapsed;

            this.currentState = this.keyboard.GetState();

            // Invoke the KeyDown or KeyUp events depending on the setup behavior
            switch (BehaviorType)
            {
                case KeyBehaviorType.KeyDownContinuous: // Fire the KeyDownEvent as long as the key is being pressed
                    // If any of the assigned key have been pressed
                    if (this.currentState.IsKeyDown(Key))
                    {
                        KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                    }

                    break;
                case KeyBehaviorType.OnceOnDown: // Fire the KeyDownEvent only once after it is pressed
                    // Prevent the KeyDownEvent from being triggered twice if the AlwaysInvokeKeyDownEvent is enabled
                    if (!AlwaysInvokeKeyDownEvent)
                    {
                        if (this.currentState.IsKeyDown(Key))
                        {
                            KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                        }
                    }

                    break;
                case KeyBehaviorType.OnceOnRelease:
                    // Prevent the KeyUpEvent from being triggered twice if the AlwaysInvokeKeyUpEvent is enabled
                    if (AlwaysInvokeKeyUpEvent)
                    {
                        if (this.currentState.IsKeyUp(Key))
                        {
                            KeyUpEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                        }
                    }

                    break;
                case KeyBehaviorType.OnKeyDownTimeDelay:
                    // If the time has passed the set delay time, fire the KeyDownEvent
                    if (this.timeElapsed >= TimeDelay)
                    {
                        if (this.currentState.IsKeyDown(Key))
                        {
                            KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                        }

                        // Reset the time elapsed
                        this.timeElapsed = 0;
                    }

                    break;
                case KeyBehaviorType.OnKeyReleaseTimeDelay:
                    // If the time has passed the set delay time, fire the KeyPressedEvent
                    if (this.timeElapsed >= TimeDelay)
                    {
                        if (this.currentState.IsKeyUp(Key))
                        {
                            KeyUpEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                        }

                        // Reset the time elapsed
                        this.timeElapsed = 0;
                    }

                    break;
                default:
                    throw new Exception($"Invalid '{nameof(KeyBehaviorType)}' of value '{(int)BehaviorType}'.");
            }

            this.previousKeyboardState = this.currentState;
        }
    }
}

// <copyright file="KeyBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Behaviors
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Raptor.Input;

    /// <summary>
    /// The behavior of a single key on a keyboard.
    /// </summary>
    public class KeyBehavior : IBehavior
    {
        private int timeElapsed; // The time elapsed since last frame
        private KeyboardState currentState;
        private KeyboardState previousKeyboardState;
        private readonly IKeyboard keyboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBehavior"/> class.
        /// </summary>
        /// <param name="key">The assigned keyboard key of the behavior.</param>
        /// <param name="keyboard">Manages keyboard input.</param>
        [ExcludeFromCodeCoverage]
        public KeyBehavior(KeyCode key, IKeyboard keyboard, bool enabled = false)
        {
            this.keyboard = keyboard;
            Setup(key, enabled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBehavior"/> class.
        /// </summary>
        /// <param name="keyboard">The keyboard to inject.</param>
        internal KeyBehavior()
        {
            Setup(KeyCode.X, true);
        }

        /// <summary>
        /// Occurs when a key has been pressed.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyDownEvent;

        /// <summary>
        /// Occurs when a key has been released.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUpEvent;

        /// <summary>
        /// Occurs when a key has first been fully pressed down then released.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyPressEvent;

        /// <summary>
        /// Gets or sets the key for the behavior.
        /// </summary>
        public KeyCode Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the behavior is enabled.  If disabled, the update method will not update the behavior or fire events.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the type of behavior of the KeyBehavior.
        /// KeyDownContinuous will fire the key down event as long as the key is pressed down.
        /// OnceOnKeyPress will fire the key only one time when the key is pressed down.
        /// OnceOnKeyRelease will fire the key only one time when the key is released.
        /// </summary>
        public KeyBehaviorType BehaviorType { get; set; } = KeyBehaviorType.OnceOnDown;

        /// <summary>
        /// Gets or sets the amount of time for a KeyDownEvent or KeyRelease event to be fired.
        /// </summary>
        public int TimeDelay { get; set; } = 1000;

        /// <summary>
        /// Gets or sets a value indicating if the KeyDownEvent should always be invoked.
        /// The KeyDownEvent will be invoked no matter what KeyBehaviorType is set.
        /// </summary>
        public bool AlwaysInvokeKeyDownEvent { get; }

        /// <summary>
        /// Gets or sets a value indicating if the KeyUpEvent should always be invoked.
        /// The KeyUpEvent will be invoked no matter what KeyBehaviorType is set.
        /// </summary>
        public bool AlwaysInvokeKeyUpEvent { get; }

        /// <summary>
        /// Returns a value indicating if the key is currently in the down position.
        /// </summary>
        public bool IsDown => this.currentState.IsKeyDown(Key);

        /// <summary>
        /// Gets or sets the name of the <see cref="KeyBehavior"/>.  The default
        /// name will be 'KeyBehavior'.
        /// </summary>
        public string Name { get; set; } = nameof(KeyBehavior);
        public Guid ID { get; set; }

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

            #region Button Behavior Code
            // Invoke the KeyDown or KeyUp events depending on the setup behavior
            switch (BehaviorType)
            {
                case KeyBehaviorType.KeyDownContinuous:// Fire the KeyDownEvent as long as the key is being pressed
                    // If any of the assigned key have been pressed
                    if (this.currentState.IsKeyDown(Key))
                    {
                        KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                    }

                    break;
                case KeyBehaviorType.OnceOnDown:// Fire the KeyDownEvent only once after it is pressed
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
                    if (!AlwaysInvokeKeyUpEvent)
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
                    throw new Exception($"Invalid '{nameof(KeyBehaviorType)}' of value '{(int)BehaviorType}'");
            }
            #endregion

            this.previousKeyboardState = this.currentState;
        }

        /// <summary>
        /// Sets up the <see cref="KeyBehavior"/>.
        /// </summary>
        /// <param name="key">The key to set to the behavior.</param>
        /// <param name="enabled">Enabled if set to true.</param>
        private void Setup(KeyCode key, bool enabled)
        {
            Key = key;
            Enabled = enabled;
        }
    }
}

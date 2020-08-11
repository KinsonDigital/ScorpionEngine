// <copyright file="KeyBehavior.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using Raptor;
using Raptor.Input;
using Raptor.Plugins;

namespace KDScorpionEngine.Behaviors
{
    /// <summary>
    /// The behavior of a single key on a keyboard.
    /// </summary>
    public class KeyBehavior : IBehavior
    {
        #region Events
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
        #endregion


        #region Private Fields
        private int _timeElapsed;//The time elapsed since last frame
        private Keyboard _keyboard;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="KeyBehavior"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="keyboard">The keyboard to inject.</param>
        internal KeyBehavior(IKeyboard keyboard)
        {
            _keyboard = new Keyboard(keyboard);
            Setup(KeyCode.X, true);
        }


        /// <summary>
        /// Creates a new <see cref="KeyBehavior"/>.
        /// </summary>
        /// <param name="key">The assigned keyboard key of the behavior.</param>
        [ExcludeFromCodeCoverage]
        public KeyBehavior(KeyCode key, bool enabled = false) => Setup(key, enabled);
        #endregion


        #region Props
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
        public bool IsDown => _keyboard.IsKeyDown(Key);

        /// <summary>
        /// Gets or sets the name of the <see cref="KeyBehavior"/>.  The default
        /// name will be 'KeyBehavior'.
        /// </summary>
        public string Name { get; set; } = nameof(KeyBehavior);
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the key behavior.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public void Update(EngineTime engineTime)
        {
            if (!Enabled) return;

            _timeElapsed += engineTime.ElapsedEngineTime.Milliseconds;

            _keyboard.UpdateCurrentState();

            #region Button Behavior Code
            //Invoke the KeyDown or KeyUp events depending on the setup behavior
            switch (BehaviorType)
            {
                case KeyBehaviorType.KeyDownContinuous://Fire the KeyDownEvent as long as the key is being pressed
                    //If any of the assigned key have been pressed
                    if (_keyboard.IsKeyDown(Key))
                        KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                    break;
                case KeyBehaviorType.OnceOnDown://Fire the KeyDownEvent only once after it is pressed
                    //Prevent the KeyDownEvent from being triggered twice if the AlwaysInvokeKeyDownEvent is enabled
                    if (! AlwaysInvokeKeyDownEvent)
                    {
                        if(_keyboard.IsKeyPressed(Key))
                            KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                    }
                    break;
                case KeyBehaviorType.OnceOnRelease:
                    //Prevent the KeyUpEvent from being triggered twice if the AlwaysInvokeKeyUpEvent is enabled
                    if (!AlwaysInvokeKeyUpEvent)
                    {
                        if (_keyboard.IsKeyUp(Key))
                            KeyUpEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));
                    }
                    break;
                case KeyBehaviorType.OnKeyDownTimeDelay:
                    //If the time has passed the set delay time, fire the KeyDownEvent
                    if (_timeElapsed >= TimeDelay)
                    {
                        if (_keyboard.IsKeyDown(Key))
                            KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));

                        //Reset the time elapsed
                        _timeElapsed = 0;
                    }
                    break;
                case KeyBehaviorType.OnKeyReleaseTimeDelay:
                    //If the time has passed the set delay time, fire the KeyPressedEvent
                    if (_timeElapsed >= TimeDelay)
                    {
                        if (_keyboard.IsKeyUp(Key))
                            KeyUpEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));

                        //Reset the time elapsed
                        _timeElapsed = 0;
                    }
                    break;
                case KeyBehaviorType.OnAnyKeyPress:
                    //If any keys at all have been released
                    if (_keyboard.GetCurrentPressedKeys().Length > 0)
                        KeyPressEvent?.Invoke(this, new KeyEventArgs(_keyboard.GetCurrentPressedKeys()));
                    break;
                default:
                    throw new Exception($"Invalid '{nameof(KeyBehaviorType)}' of value '{(int)BehaviorType}'");
            }
            #endregion

            _keyboard.UpdatePreviousState();
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the <see cref="KeyBehavior"/>.
        /// </summary>
        /// <param name="key">The key to set to the behavior.</param>
        /// <param name="enabled">Enabled if set to true.</param>
        private void Setup(KeyCode key, bool enabled)
        {
            _keyboard = _keyboard ?? new Keyboard();

            Key = key;
            Enabled = enabled;
        }
        #endregion
    }
}
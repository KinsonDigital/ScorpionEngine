using System;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;

namespace KDScorpionEngine.Behaviors
{
    /// <summary>
    /// Represents behavior of a key that can be pressed on the keyboard.
    /// </summary>
    public class KeyBehavior : IBehavior
    {
        #region Events
        /// <summary>
        /// Invoked when a key has been pressed.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyDownEvent;

        /// <summary>
        /// Invoked when a key has been released.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUpEvent;

        public event EventHandler<KeyEventArgs> KeyPressEvent;
        #endregion


        #region Fields
        private int _timeElapsed;//The engineTime elapsed since last frame
        private Keyboard _keyboard;
        #endregion


        #region Constructors
        internal KeyBehavior(IKeyboard keyboard)
        {
            _keyboard = new Keyboard(keyboard);

            Setup(KeyCodes.X, true);
        }


        /// <summary>
        /// Creates a new key behavior.
        /// </summary>
        /// <param name="key">The assigned keyboard key of the behavior.</param>
        public KeyBehavior(KeyCodes key, bool enabled = false) => Setup(key, enabled);
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the key for the behavior.
        /// </summary>
        public KeyCodes Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the behavior is enabled.  If disabled, the update method will not update the behavior or fire events.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the type of behavior of the KeyBehavior.
        /// KeyDownContinuous will fire the key pressed event as long as the key is pressed down.
        /// OnceOnKeyPress will fire the key only one engineTime when the key is pressed down.
        /// OnceOnKeyRelease will fire the key only one engineTime when the key is released.
        /// </summary>
        public KeyBehaviorType BehaviorType { get; set; } = KeyBehaviorType.OnceOnDown;

        /// <summary>
        /// Gets or sets the engineTime delay for a KeyDownEvent or KeyRelease event to be fired.
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

        public string Name { get; set; } = nameof(KeyBehavior);
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the key behavior.
        /// </summary>
        /// <param name="engineTime">The game engineTime of the current frame.</param>
        public void Update(EngineTime engineTime)
        {
            if (!Enabled) return;

            _timeElapsed += engineTime.ElapsedEngineTime.Milliseconds;

            _keyboard.UpdateCurrentState();

            #region button Behavior Code
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
                    //If the engineTime has passed the set delay engineTime, fire the KeyDownEvent
                    if (_timeElapsed >= TimeDelay)
                    {
                        if (_keyboard.IsKeyDown(Key))
                            KeyDownEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));

                        //Reset the engineTime elapsed
                        _timeElapsed = 0;
                    }
                    break;
                case KeyBehaviorType.OnKeyReleaseTimeDelay:
                    //If the engineTime has passed the set delay engineTime, fire the KeyPressedEvent
                    if (_timeElapsed >= TimeDelay)
                    {
                        if (_keyboard.IsKeyUp(Key))
                            KeyUpEvent?.Invoke(this, new KeyEventArgs(new[] { Key }));

                        //Reset the engineTime elapsed
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
        private void Setup(KeyCodes key, bool enabled)
        {
            if (_keyboard == null)
                _keyboard = new Keyboard();

            Key = key;
            Enabled = enabled;
        }
        #endregion
    }
}
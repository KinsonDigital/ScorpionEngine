using System;
using Microsoft.Xna.Framework.Input;
using ScorpionEngine.Input;
using ScorpionEngine.Utils;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;

namespace ScorpionEngine
{
    /// <summary>
    /// Represents behavior of a key that can be pressed on the keyboard.
    /// </summary>
    public class KeyBehavior
    {
        #region Events
        /// <summary>
        /// Invoked when a key has been pressed.
        /// </summary>
        public event EventHandler<Input.KeyEventArgs> KeyDownEvent;

        /// <summary>
        /// Invoked when a key has been released.
        /// </summary>
        public event EventHandler<Input.KeyEventArgs> KeyUpEvent; 
        #endregion

        #region Fields
        private InputKeys _key;//The key that is used for the behavior
        private bool _enabled;//True if the key behavior is enabled
        private int _timeDelay;//The engineTime delay in milliseconds
        private int _timeElapsed = 1000;//The engineTime elapsed since last frame
        private KeyboardState _currentKeyState;//The current key state of the current frame
        private KeyboardState _prevKeyState;//The previous key state from the last frame
        private KeyBehaviorType _behaviorType = KeyBehaviorType.OnceOnPress;//The way that the KeyBehavior will behave
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new key behavior.
        /// </summary>
        /// <param name="enabled">Set to true to enable the behavior.</param>
        public KeyBehavior(bool enabled = false)
        {
            _key = Key;
            _enabled = enabled;
        }

        /// <summary>
        /// Creates a new key behavior.
        /// </summary>
        /// <param name="key">The assigned keyboard key of the behavior.</param>
        /// <param name="enabled">Set to true to enable the behavior.</param>
        public KeyBehavior(InputKeys key, bool enabled = false)
        {
            _key = key;
            _enabled = enabled;

            AlwaysInvokeKeyDownEvent = false;//REMOVE THIS
            AlwaysInvokeKeyUpEvent = false;//REMOVE THIS
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the key for the behavior.
        /// </summary>
        public InputKeys Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating if the behavior is enabled.  If disabled, the update method will not update the behavior or fire events.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Gets or sets the type of behavior of the KeyBehavior.
        /// KeyDownContinuous will fire the key pressed event as long as the key is pressed down.
        /// OnceOnKeyPress will fire the key only one engineTime when the key is pressed down.
        /// OnceOnKeyRelease will fire the key only one engineTime when the key is released.
        /// </summary>
        public KeyBehaviorType BehaviorType
        {
            get { return _behaviorType; }
            set { _behaviorType = value; }
        }

        /// <summary>
        /// Gets or sets the engineTime delay for a KeyDownEvent or KeyRelease event to be fired.
        /// </summary>
        public int TimeDelay
        {
            get { return _timeDelay; }
            set { _timeDelay = value; }
        }

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
        /// Returns a value indicating if the key has been released.
        /// </summary>
        public bool IsPressed => _currentKeyState.IsKeyDown((Keys)_key);
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the key behavior.
        /// </summary>
        /// <param name="engineTime">The game engineTime of the current frame.</param>
        public void Update(EngineTime engineTime)
        {
            if (!_enabled) return;

            //Get the current keyboard state
            _currentKeyState = Keyboard.GetState();

            #region button Behavior Code
            //Invoke the KeyDown or KeyUp events depending on the setup behavior
            switch (_behaviorType)
            {
                case KeyBehaviorType.KeyDownContinuous://Fire the KeyDownEvent as long as the key is being pressed
                    //If any of the assigned key have been pressed
                    if (_currentKeyState.IsKeyDown((Keys) _key))
                        KeyDownEvent?.Invoke(this, new Input.KeyEventArgs(new[] { _key }));
                    break;
                case KeyBehaviorType.OnceOnPress://Fire the KeyDownEvent only once after it is pressed
                    //Prevent the KeyDownEvent from being triggered twice if the AlwaysInvokeKeyDownEvent is enabled
                    if (! AlwaysInvokeKeyDownEvent)
                    {
                        if (_currentKeyState.IsKeyDown((Keys) _key) && ! _prevKeyState.IsKeyDown((Keys) _key))
                            KeyDownEvent?.Invoke(this, new Input.KeyEventArgs(new[] { _key }));
                    }
                    break;
                case KeyBehaviorType.OnceOnRelease:
                    //Prevent the KeyUpEvent from being triggered twice if the AlwaysInvokeKeyUpEvent is enabled
                    if (! AlwaysInvokeKeyUpEvent)
                    {
                        if (! _currentKeyState.IsKeyDown((Keys) _key) && _prevKeyState.IsKeyDown((Keys) _key))
                            KeyUpEvent?.Invoke(this, new Input.KeyEventArgs(new[] { _key }));
                    }
                    break;
                case KeyBehaviorType.OnKeyPressedTimeDelay:
                    //If the engineTime has passed the set delay engineTime, fire the KeyPressedEvent
                    if (_timeElapsed >= _timeDelay)
                    {
                        if (_currentKeyState.IsKeyUp((Keys) _key) && ! _prevKeyState.IsKeyUp((Keys) _key))
                            KeyUpEvent?.Invoke(this, new Input.KeyEventArgs(new[] { _key }));

                        //Reset the engineTime elapsed
                        _timeElapsed = 0;
                    }
                    break;
                case KeyBehaviorType.OnKeyReleaseTimeDelay:
                    //Update the engineTime passed
                    _timeElapsed += engineTime.ElapsedEngineTime.Milliseconds;

                    //If the engineTime has passed the set delay engineTime, fire the KeyPressedEvent
                    if (_timeElapsed >= _timeDelay)
                    {
                        if (_currentKeyState.IsKeyDown((Keys)_key) && !_prevKeyState.IsKeyDown((Keys)_key))
                            KeyDownEvent?.Invoke(this, new Input.KeyEventArgs(new[] { _key }));

                        //Reset the engineTime elapsed
                        _timeElapsed = 0;
                    }
                    break;
                case KeyBehaviorType.OnAnyKeyPress:
                    //If any keys at all have been released
                    if (_currentKeyState.GetPressedKeys().Length > 0)
                        KeyDownEvent?.Invoke(this, new Input.KeyEventArgs(Tools.ToInputKeys(_currentKeyState.GetPressedKeys())));
                    break;
                case KeyBehaviorType.OnAnyKeyRelease:
                    //If there is no keys currently pressed and previous keys were pressed
                    if(_currentKeyState.GetPressedKeys().Length == 0 && _prevKeyState.GetPressedKeys().Length > 0)
                        KeyUpEvent?.Invoke(this, new Input.KeyEventArgs(Tools.ToInputKeys(_prevKeyState.GetPressedKeys())));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            #endregion

            _prevKeyState = _currentKeyState;
        }
        #endregion
    }
}
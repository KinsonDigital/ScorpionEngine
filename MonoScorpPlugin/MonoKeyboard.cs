using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoGameKeyboard = Microsoft.Xna.Framework.Input.Keyboard;
using KDScorpionCore.Plugins;
using KDScorpionCore.Input;
using System.Linq;

namespace MonoScorpPlugin
{
    /// <summary>
    /// Used to check the keyboard for input.
    /// </summary>
    public class MonoKeyboard : IKeyboard
    {
        #region Events
        /// <summary>
        /// Occurs when a key has been pressed to the down position.
        /// </summary>
        public event EventHandler<EventArgs> OnKeyDown;

        /// <summary>
        /// Occurs when a key has been released to the up position.
        /// </summary>
        public event EventHandler<EventArgs> OnKeyUp;
        #endregion


        #region Fields
        private KeyboardState _currentState;//The current state of the keyboard to compare to the previous state
        private KeyboardState _previousState;//The previous state of the keyboard to compare to the current state
        #endregion


        #region Props
        /// <summary>
        /// Gets a value indicating if the caps lock key is on.
        /// </summary>
        public bool CapsLockOn => _currentState.CapsLock;

        /// <summary>
        /// Gets a value indicating if the numlock key is on.
        /// </summary>
        public bool NumLockOn => _currentState.NumLock;

        /// <summary>
        /// Gets a value indicating if the left shift key is being pressed.
        /// </summary>
        public bool IsLeftShiftDown => _currentState.IsKeyDown(Keys.LeftShift);

        /// <summary>
        /// Gets a value indicating if the right shift key is being pressed.
        /// </summary>
        public bool IsRightShiftDown => _currentState.IsKeyDown(Keys.RightShift);

        /// <summary>
        /// Gets a value indicating if the left control key is being pressed down.
        /// </summary>
        public bool IsLeftCtrlDown => _currentState.IsKeyDown(Keys.LeftControl);

        /// <summary>
        /// Gets a value indicating if the right control key is being pressed down.
        /// </summary>
        public bool IsRightCtrlDown => _currentState.IsKeyDown(Keys.RightControl);

        /// <summary>
        /// Gets a value indicating if the left alt key is being pressed down.
        /// </summary>
        public bool IsLeftAltDown => _currentState.IsKeyDown(Keys.LeftAlt);

        /// <summary>
        /// Gets a value indicating if the right alt key is being pressed down.
        /// </summary>
        public bool IsRightAltDown => _currentState.IsKeyDown(Keys.RightAlt);
        #endregion


        #region Public Methods
        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        public void UpdateCurrentState()
        {
            //Get the state of the keyboard and save it as the current state
            _currentState = MonoGameKeyboard.GetState();

            //If any key has been pressed, invoke the OnKeyDown event
            if (_currentState.GetPressedKeys().Length > 0)
            {
                //Invoke the OnKeyDown event and send the list of keys that are pressed down
                OnKeyDown?.Invoke(this, new EventArgs());
            }
            else if (_currentState.GetPressedKeys().Length == 0 && _previousState.GetPressedKeys().Length > 0)
            {
                //If any keys have been released, invoke the OnKeyUp event

                //Invoke the OnKeyUp event and send the list of keys that are pressed down
                OnKeyUp?.Invoke(this, new EventArgs());
            }
        }


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        public void UpdatePreviousState()
        {
            //Update the previous state
            _previousState = _currentState;
        }


        /// <summary>
        /// Gets a value indicating if any of the keys are in the down position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysDown()
        {
            return _currentState.GetPressedKeys().Length > 0;
        }


        /// <summary>
        /// Returns a value indicating if any of the given key codes are being held down.
        /// </summary>
        /// <param name="keys">The list of key codes to check.</param>
        /// <returns></returns>
        public bool IsAnyKeyDown(KeyCodes[] keys)
        {
            foreach (var key in keys)
            {
                if (IsKeyDown(key))
                    return true;
            }


            return false;
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(KeyCodes key) => _currentState.IsKeyDown((Keys)key);


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(KeyCodes key) => _currentState.IsKeyUp((Keys)key);


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(KeyCodes key)
        {
            return _currentState.IsKeyUp((Keys)key) && _previousState.IsKeyDown((Keys)key);
        }


        /// <summary>
        /// Gets the list of currently pressed keys.
        /// </summary>
        /// <returns></returns>
        public KeyCodes[] GetCurrentPressedKeys()
        {
            return (from k in _currentState.GetPressedKeys() select (KeyCodes)k).ToArray();
        }


        /// <summary>
        /// Gets the list of previously pressed keys.
        /// </summary>
        /// <returns></returns>
        public KeyCodes[] GetPreviousPressedKeys()
        {
            return (from k in _previousState.GetPressedKeys() select (KeyCodes)k).ToArray();
        }


        public void InjectData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }


        public object GetData(string dataType)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

using System;
using Microsoft.Xna.Framework.Input;
using ScorpionEngine.Input;

namespace MonoDriver
{
    /// <summary>
    /// Used to check the keyboard for input.
    /// </summary>
    public class KeyboardInput
    {
        #region Events
        /// <summary>
        /// Occurs when a key has been pressed to the down position.
        /// </summary>
        public event EventHandler<KeyEventArgs> OnKeyDown;

        /// <summary>
        /// Occurs when a key has been released to the up position.
        /// </summary>
        public event EventHandler<KeyEventArgs> OnKeyUp;
        #endregion


        #region Fields
        private KeyboardState _currentState;//The current state of the keyboard to compare to the previous state
        private KeyboardState _previousState;//The previous state of the keyboard to compare to the current state
        #endregion


        #region Public Methods
        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        public void UpdateCurrentState()
        {
            //Get the state of the keyboard and save it as the current state
            _currentState = Keyboard.GetState();

            //If any key has been pressed, invoke the OnKeyDown event
            if (_currentState.GetPressedKeys().Length > 0)
            {
                //Invoke the OnKeyDown event and send the list of keys that are pressed down
                OnKeyDown?.Invoke(this, new KeyEventArgs(Tools.ToInputKeys(_currentState.GetPressedKeys())));
            }
            else if (_currentState.GetPressedKeys().Length == 0 && _previousState.GetPressedKeys().Length > 0)
            {
                //If any keys have been released, invoke the OnKeyUp event

                //Invoke the OnKeyUp event and send the list of keys that are pressed down
                OnKeyUp?.Invoke(this, new KeyEventArgs(Tools.ToInputKeys(_previousState.GetPressedKeys())));
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
        /// Returns true if any keys have been pressed.  This means a key was first put into the down position, then released to the up position.
        /// </summary>
        /// <returns></returns>
        public bool IsAnyKeysPressed()
        {
            return _currentState.GetPressedKeys().Length == 0 && _previousState.GetPressedKeys().Length > 0;
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(InputKeys key)
        {
            return _currentState.IsKeyDown((Keys)key);
        }


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(InputKeys key)
        {
            return _currentState.IsKeyUp((Keys)key);
        }


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(InputKeys key)
        {
            return _currentState.IsKeyUp((Keys) key) && _previousState.IsKeyDown((Keys) key);
        }



        //TODO: Add Method Docs
        public InputKeys[] GetCurrentPressedKeys()
        {
            return Tools.ToInputKeys(_currentState.GetPressedKeys());
        }


        //TODO: Add Method Docs
        public InputKeys[] GetPreviousPressedKeys()
        {
            return Tools.ToInputKeys(_previousState.GetPressedKeys());
        }
        #endregion
    }
}

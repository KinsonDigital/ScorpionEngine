using System;
using Microsoft.Xna.Framework.Input;
using ScorpionCore.Plugins;

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


        /// Returns true if any keys have been pressed.  This means a key was first put into the down position, then released to the up position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysPressed()
        {
            return _currentState.GetPressedKeys().Length > 0;
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(int key)
        {
            return _currentState.IsKeyDown((Keys)key);
        }


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(int key)
        {
            return _currentState.IsKeyUp((Keys)key);
        }


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(int key)
        {
            return _currentState.IsKeyUp((Keys) key) && _previousState.IsKeyDown((Keys) key);
        }


        /// <summary>
        /// Gets the list of currently pressed keys.
        /// </summary>
        /// <returns></returns>
        public int[] GetCurrentPressedKeys()
        {
            var result = _currentState.GetPressedKeys();

            var isLetterDown = _currentState.IsKeyDown(Keys.F);

            if (isLetterDown)
            {
                var stop = true;
            }
            return Tools.ToInputKeyCodes(_currentState.GetPressedKeys());
        }


        /// <summary>
        /// Gets the list of previously pressed keys.
        /// </summary>
        /// <returns></returns>
        public int[] GetPreviousPressedKeys()
        {
            return Tools.ToInputKeyCodes(_previousState.GetPressedKeys());
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

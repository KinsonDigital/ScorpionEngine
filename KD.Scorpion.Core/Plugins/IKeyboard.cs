namespace KDScorpionCore.Plugins
{
    public interface IKeyboard : IPlugin
    {
        #region Props
        /// <summary>
        /// Gets a value indicating if the caps lock key is on.
        /// </summary>
        bool CapsLockOn { get; }

        /// <summary>
        /// Gets a value indicating if the numlock key is on.
        /// </summary>
        bool NumLockOn { get; }
        #endregion


        #region Methods
        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        void UpdateCurrentState();


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        void UpdatePreviousState();


        /// <summary>
        /// Returns a value indicating if any keys are in the down position.
        /// </summary>
        /// <returns></returns>
        bool AreAnyKeysDown();


        /// <summary>
        /// Returns true if any keys have been pressed.  This means a key was first put into the down position, then released to the up position.
        /// </summary>
        /// <returns></returns>
        bool AreAnyKeysPressed();


        /// <summary>
        /// Returns a value indicating if any of the numpad number keys were pressed.
        /// </summary>
        /// <returns></returns>
        bool AnyNumpadNumbersKeysDown();


        /// <summary>
        /// Returns a value indicating if any of the given key codes are being held down.
        /// </summary>
        /// <param name="keys">The list of key codes to check.</param>
        /// <returns></returns>
        bool IsAnyKeyDown(int[] keys);


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        bool IsKeyDown(int key);


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        bool IsKeyUp(int key);


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        bool IsKeyPressed(int key);


        /// <summary>
        /// Returns all of the currently pressed keys on the keyboard.
        /// </summary>
        /// <returns></returns>
        int[] GetCurrentPressedKeys();


        /// <summary>
        /// Returns all of the previously pressed keys from the last frame.
        /// </summary>
        /// <returns></returns>
        int[] GetPreviousPressedKeys();


        /// <summary>
        /// Returns a value indicating if a letter on the keyboard was pressed.
        /// </summary>
        /// <returns></returns>
        bool WasLetterPressed();
        #endregion
    }
}

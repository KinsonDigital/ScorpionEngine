namespace ScorpionCore.Plugins
{
    public interface IKeyboard : IPlugin
    {
        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        void UpdateCurrentState();


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        void UpdatePreviousState();


        /// <summary>
        /// Returns true if any keys have been pressed.  This means a key was first put into the down position, then released to the up position.
        /// </summary>
        /// <returns></returns>
        bool AreAnyKeysPressed();


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
    }
}

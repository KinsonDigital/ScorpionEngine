using System;
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
        #region Private Fields
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
        /// Gets a value indicating if the left shift key is being held down.
        /// </summary>
        public bool IsLeftShiftDown => _currentState.IsKeyDown(Keys.LeftShift);

        /// <summary>
        /// Gets a value indicating if the right shift key is being held down.
        /// </summary>
        public bool IsRightShiftDown => _currentState.IsKeyDown(Keys.RightShift);

        /// <summary>
        /// Gets a value indicating if the left control key is being held down.
        /// </summary>
        public bool IsLeftCtrlDown => _currentState.IsKeyDown(Keys.LeftControl);

        /// <summary>
        /// Gets a value indicating if the right control key is being held down.
        /// </summary>
        public bool IsRightCtrlDown => _currentState.IsKeyDown(Keys.RightControl);

        /// <summary>
        /// Gets a value indicating if the left alt key is being held down.
        /// </summary>
        public bool IsLeftAltDown => _currentState.IsKeyDown(Keys.LeftAlt);

        /// <summary>
        /// Gets a value indicating if the right alt key is being held down.
        /// </summary>
        public bool IsRightAltDown => _currentState.IsKeyDown(Keys.RightAlt);
        #endregion


        #region Public Methods
        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        public void UpdateCurrentState() => _currentState = MonoGameKeyboard.GetState();


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        public void UpdatePreviousState() => _previousState = _currentState;


        /// <summary>
        /// Returns a value indicating if any keys are in the down position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysDown() => _currentState.GetPressedKeys().Length > 0;


        /// <summary>
        /// Returns a value indicating if any of the given key codes are being held in the down position.
        /// </summary>
        /// <param name="keys">The list of key codes to check.</param>
        /// <returns></returns>
        public bool AreKeysDown(KeyCodes[] keys) => keys.Any(k => IsKeyDown(k));


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public bool IsKeyDown(KeyCodes key) => _currentState.IsKeyDown((Keys)key);


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public bool IsKeyUp(KeyCodes key) => _currentState.IsKeyUp((Keys)key);


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public bool IsKeyPressed(KeyCodes key) => _currentState.IsKeyUp((Keys)key) && _previousState.IsKeyDown((Keys)key);


        /// <summary>
        /// Returns all of the currently pressed keys of the keyboard for the current frame.
        /// </summary>
        /// <returns></returns>
        public KeyCodes[] GetCurrentPressedKeys() => (from k in _currentState.GetPressedKeys() select (KeyCodes)k).ToArray();


        /// <summary>
        /// Returns all of the previously pressed keys of the keyborad from the last frame.
        /// </summary>
        /// <returns></returns>
        public KeyCodes[] GetPreviousPressedKeys() => (from k in _previousState.GetPressedKeys() select (KeyCodes)k).ToArray();


        /// <summary>
        /// Injects any arbitrary data into the plugin for use.  Must be a class.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class => throw new NotImplementedException();


        /// <summary>
        /// Gets the data as the given type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="option">Used to pass in options for the <see cref="GetData{T}(int)"/> implementation to process.</param>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>(int option) where T : class => throw new NotImplementedException();
        #endregion
    }
}

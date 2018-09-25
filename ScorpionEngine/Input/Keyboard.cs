using ScorpionCore;
using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Tracks the state of the keys on keyboard.
    /// </summary>
    public class Keyboard : IKeyboard
    {
        private IKeyboard _internalKeyboard;


        /// <summary>
        /// Creates a new instance of <see cref="Keyboard"/> for tracking keyboard events.
        /// </summary>
        public Keyboard()
        {
            _internalKeyboard = PluginLoader.GetPluginByType<IKeyboard>();
        }


        /// <summary>
        /// Returns all of the currently pressed keys on the keyboard.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetCurrentPressedKeys()
        {
            return _internalKeyboard.GetCurrentPressedKeys();
        }


        /// <summary>
        /// Returns all of the previously pressed keys from the last frame.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetPreviousPressedKeys()
        {
            return _internalKeyboard.GetPreviousPressedKeys();
        }


        /// <summary>
        /// Gets any arbitrary data needed for use.
        /// </summary>
        /// <typeparam name="T">The type of data to get.</typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : class
        {
            return _internalKeyboard.GetData<T>();
        }


        /// <summary>
        /// Inject any arbitrary data into the plugin for use.
        /// </summary>
        /// <typeparam name="T">The type of data to inject.</typeparam>
        /// <param name="data">The data to inject.</param>
        public void InjectData<T>(T data) where T : class
        {
            _internalKeyboard.InjectData<T>(data);
        }


        /// <summary>
        /// Returns true if any keys have been pressed.  This means a key was first put into the down position, then released to the up position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysPressed()
        {
            return _internalKeyboard.AreAnyKeysPressed();
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(InputKeys key)
        {
            return _internalKeyboard.IsKeyDown(key);
        }


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(InputKeys key)
        {
            return _internalKeyboard.IsKeyUp(key);
        }


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(InputKeys key)
        {
            return _internalKeyboard.IsKeyPressed(key);
        }


        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        public void UpdateCurrentState()
        {
            _internalKeyboard.UpdateCurrentState();
        }


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        public void UpdatePreviousState()
        {
            _internalKeyboard.UpdatePreviousState();
        }
    }
}

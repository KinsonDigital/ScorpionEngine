using ScorpionCore;
using ScorpionCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Tracks the state of the keys on keyboard.
    /// </summary>
    public class Keyboard
    {
        private IKeyboard _internalKeyboard;


        #region Constructors
        internal Keyboard(IKeyboard keyboard)
        {
            _internalKeyboard = keyboard;
        }


        /// <summary>
        /// Creates a new instance of <see cref="Keyboard"/> for tracking keyboard events.
        /// </summary>
        public Keyboard()
        {
            _internalKeyboard = PluginSystem.EnginePlugins.LoadPlugin<IKeyboard>();
        }
        #endregion


        /// <summary>
        /// Returns all of the currently pressed keys on the keyboard.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetCurrentPressedKeys()
        {
            return (from k in _internalKeyboard.GetCurrentPressedKeys()
                   select (InputKeys)k).ToArray();
        }


        /// <summary>
        /// Returns all of the previously pressed keys from the last frame.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetPreviousPressedKeys()
        {
            return (from k in _internalKeyboard.GetPreviousPressedKeys()
                    select (InputKeys)k).ToArray();
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
            return _internalKeyboard.IsKeyDown((int)key);
        }


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(InputKeys key)
        {
            return _internalKeyboard.IsKeyUp((int)key);
        }


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(InputKeys key)
        {
            return _internalKeyboard.IsKeyPressed((int)key);
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

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
        #region Constructors
        internal Keyboard(IKeyboard keyboard)
        {
            InternalKeyboard = keyboard;
        }


        /// <summary>
        /// Creates a new instance of <see cref="Keyboard"/> for tracking keyboard events.
        /// </summary>
        public Keyboard()
        {
            InternalKeyboard = PluginSystem.EnginePlugins.LoadPlugin<IKeyboard>();
        }
        #endregion


        #region Props
        internal IKeyboard InternalKeyboard { get; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns all of the currently pressed keys on the keyboard.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetCurrentPressedKeys()
        {
            return (from k in InternalKeyboard.GetCurrentPressedKeys()
                   select (InputKeys)k).ToArray();
        }


        /// <summary>
        /// Returns all of the previously pressed keys from the last frame.
        /// </summary>
        /// <returns></returns>
        public InputKeys[] GetPreviousPressedKeys()
        {
            return (from k in InternalKeyboard.GetPreviousPressedKeys()
                    select (InputKeys)k).ToArray();
        }


        /// <summary>
        /// Returns true if any keys have been pressed.  This means a key was first put into the down position, then released to the up position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysPressed()
        {
            return InternalKeyboard.AreAnyKeysPressed();
        }


        /// <summary>
        /// Returns true if the given key is in the down position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyDown(InputKeys key)
        {
            return InternalKeyboard.IsKeyDown((int)key);
        }


        /// <summary>
        /// Returns true if the given key is in the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyUp(InputKeys key)
        {
            return InternalKeyboard.IsKeyUp((int)key);
        }


        /// <summary>
        /// Returns true if the given key has been put into the down position then released to the up position.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns></returns>
        public bool IsKeyPressed(InputKeys key)
        {
            return InternalKeyboard.IsKeyPressed((int)key);
        }


        /// <summary>
        /// Update the current state of the keyboard.
        /// </summary>
        public void UpdateCurrentState()
        {
            InternalKeyboard.UpdateCurrentState();
        }


        /// <summary>
        /// Update the previous state of the keyboard.
        /// </summary>
        public void UpdatePreviousState()
        {
            InternalKeyboard.UpdatePreviousState();
        }
        #endregion
    }
}

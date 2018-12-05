using ScorpionCore.Plugins;
using System.Collections.Generic;
using System.Linq;

namespace ScorpionCore.Input
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
        /// <summary>
        /// The internal keyboard plugin implementation.
        /// </summary>
        internal IKeyboard InternalKeyboard { get; }

        /// <summary>
        /// Gets a value indicating if the caps lock key is on.
        /// </summary>
        public bool CapsLockOn => InternalKeyboard.CapsLockOn;

        /// <summary>
        /// Gets a value indicating if the numlock key is on.
        /// </summary>
        public bool NumLockOn => InternalKeyboard.NumLockOn;
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
        /// Gets a value indicating if any keys are in the down position.
        /// </summary>
        /// <returns></returns>
        public bool AreAnyKeysDown()
        {
            return InternalKeyboard.AreAnyKeysDown();
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
        /// Returns a value indicating if any of the given key codes are being held down.
        /// </summary>
        /// <param name="keys">The list of key codes to check.</param>
        /// <returns></returns>
        public bool IsAnyKeyDown(InputKeys[] keys)
        {
            var keyCodes = new List<int>();

            foreach (var key in keys)
            {
                keyCodes.Add((int)key);
            }


            return InternalKeyboard.IsAnyKeyDown(keyCodes.ToArray());
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
        /// Returns a value indicating if any of the shift keys are being pressed down.
        /// </summary>
        /// <returns></returns>
        public bool IsAnyShiftKeyDown()
        {
            return IsKeyDown(InputKeys.LeftShift) || IsKeyDown(InputKeys.RightShift);
        }


        /// <summary>
        /// Returns a value indicating if the any of the delete keys ahve been fully pressed.
        /// </summary>
        /// <returns></returns>
        public bool IsDeleteKeyPressed()
        {
            return IsKeyPressed(InputKeys.Delete) || (IsAnyShiftKeyDown() && IsKeyPressed(InputKeys.Decimal));
        }


        /// <summary>
        /// Returns a value indicating if the backspace key has been fully pressed.
        /// </summary>
        /// <returns></returns>
        public bool IsBackspaceKeyPressed()
        {
            return IsKeyPressed(InputKeys.Back);
        }


        /// <summary>
        /// Returns a value indicating if a letter on the keyboard was pressed.
        /// </summary>
        /// <returns></returns>
        public bool WasLetterPressed()
        {
            return InternalKeyboard.WasLetterPressed();
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

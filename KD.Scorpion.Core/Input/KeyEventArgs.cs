using System;

namespace KDScorpionCore.Input
{
    /// <summary>
    /// Holds information about the keys down event.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        #region Props
        /// <summary>
        /// Gets the keys that was pressed.
        /// </summary>
        public KeyCodes[] Keys { get; set; }
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of KeyEventArgs.
        /// </summary>
        /// <param name="keys">The key that has to do with the event.</param>
        public KeyEventArgs(KeyCodes[] keys)
        {
            Keys = keys;
        }
        #endregion
    }
}

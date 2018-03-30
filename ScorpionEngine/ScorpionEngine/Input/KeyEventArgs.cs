using System;

namespace ScorpionEngine.Input
{
    /// <summary>
    /// Holds information about the keys down event.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the keys that was pressed.
        /// </summary>
        public InputKeys[] Keys { get; set; }
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of KeyEventArgs.
        /// </summary>
        /// <param name="keys">The key that has to do with the event.</param>
        public KeyEventArgs(InputKeys[] keys)
        {
            Keys = keys;
        }
        #endregion
    }
}
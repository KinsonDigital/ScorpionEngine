using System;

namespace KDScorpionCore.Input
{
    /// <summary>
    /// Holds information about the keys down event.
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the state of the mouse.
        /// </summary>
        public MouseInputState State { get; }
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of MouseEventArgs.
        /// </summary>
        /// <param name="state">The state of the mouse.</param>
        public MouseEventArgs(MouseInputState state)
        {
            State = state;
        }
        #endregion
    }
}

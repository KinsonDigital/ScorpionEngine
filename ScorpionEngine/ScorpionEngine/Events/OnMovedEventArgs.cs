using System;

namespace ScorpionEngine.Events
{
    /// <summary>
    /// Contains information about when an entity has moved.
    /// </summary>
    public class OnMovedEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the old position of the entity before it moved.
        /// </summary>
        public Vector OldPosition { get; set; }

        /// <summary>
        /// Gets the new position of the entity after it moved.
        /// </summary>
        public Vector NewPosition { get; set; }
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of OnEntityMovedEventArgs.
        /// </summary>
        /// <param name="oldPos">The old position of the entity.</param>
        /// <param name="newPos">The new posistion of the entity.</param>
        public OnMovedEventArgs(Vector oldPos, Vector newPos)
        {
            OldPosition = oldPos;
            NewPosition = newPos;
        }
        #endregion
    }
}
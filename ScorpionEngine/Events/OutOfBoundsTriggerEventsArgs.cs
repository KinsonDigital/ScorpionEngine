using System;
using ScorpionEngine.Objects;

namespace ScorpionEngine.Events
{
    /// <summary>
    /// Holds information about the entity that went out of bounds.
    /// </summary>
    public class OutOfBoundsTriggerEventsArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the entity that caused the trigger.
        /// </summary>
        public GameObject TriggerSource { get; private set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of OutOfBoundsTriggerEventsArgs.
        /// </summary>
        /// <param name="entity">The entity that caused the out of bounds trigger.</param>
        public OutOfBoundsTriggerEventsArgs(GameObject entity)
        {
            TriggerSource = entity;
        }
        #endregion
    }
}

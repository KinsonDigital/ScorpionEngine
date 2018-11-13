using System;
using System.Diagnostics.CodeAnalysis;
using ScorpionEngine.Entities;

namespace ScorpionEngine.Events
{
    /// <summary>
    /// Holds information about the entity that went out of bounds.
    /// </summary>
    //TODO: Get this working and setup unit tests.  Wait for when you create a game for testing
    [ExcludeFromCodeCoverage]
    public class OutOfBoundsTriggerEventsArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Gets the entity that caused the trigger.
        /// </summary>
        public Entity TriggerSource { get; private set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of OutOfBoundsTriggerEventsArgs.
        /// </summary>
        /// <param name="entity">The entity that caused the out of bounds trigger.</param>
        public OutOfBoundsTriggerEventsArgs(Entity entity)
        {
            TriggerSource = entity;
        }
        #endregion
    }
}

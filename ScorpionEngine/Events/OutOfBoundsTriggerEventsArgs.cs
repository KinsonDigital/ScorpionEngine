using System;
using System.Diagnostics.CodeAnalysis;
using KDScorpionEngine.Entities;

namespace KDScorpionEngine.Events
{
    /// <summary>
    /// Holds information about the entity that has gone out of bounds.
    /// </summary>
    //TODO: Get this working and setup unit tests.  Wait for when you create a game for testing
    [ExcludeFromCodeCoverage]
    public class OutOfBoundsTriggerEventsArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of OutOfBoundsTriggerEventsArgs.
        /// </summary>
        /// <param name="entity">The entity that caused the out of bounds trigger.</param>
        public OutOfBoundsTriggerEventsArgs(Entity entity) => TriggerSource = entity;
        #endregion


        #region Props
        /// <summary>
        /// Gets the entity that caused the trigger.
        /// </summary>
        public Entity TriggerSource { get; private set; }
        #endregion
    }
}

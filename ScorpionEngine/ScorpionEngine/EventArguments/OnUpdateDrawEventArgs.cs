using System;

namespace ScorpionEngine.EventArguments
{
    /// <summary>
    /// Holds information about the OnUpdate and OnDraw events.
    /// </summary>
    public class OnUpdateDrawEventArgs : EventArgs
    {
        /// <summary>
        /// Holds elapsed time information of when the game loop last ran.
        /// </summary>
        public EngineTime EngineTime { get; set; }

        #region Constructors

        /// <summary>
        /// Creates a new instance of OnUpdateDrawEventArgs.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public OnUpdateDrawEventArgs(EngineTime engineTime)
        {
            EngineTime = engineTime;
        }

        #endregion
    }
}
using System;

namespace ScorpionCore
{
    /// <summary>
    /// Holds information about the OnUpdate and OnDraw events.
    /// </summary>
    public class OnUpdateEventArgs : EventArgs
    {
        #region Properties
        /// <summary>
        /// Holds elapsed time information of when the game loop last ran.
        /// </summary>
        public IEngineTiming EngineTime { get; set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of OnUpdateDrawEventArgs.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public OnUpdateEventArgs(IEngineTiming engineTime)
        {
            EngineTime = engineTime;
        }
        #endregion
    }
}

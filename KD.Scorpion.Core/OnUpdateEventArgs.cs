﻿using System;

namespace KDScorpionCore
{
    /// <summary>
    /// Holds information about the <see cref="IEngineEvents.OnUpdate"/> event.
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
        /// Creates a new instance of <see cref="OnUpdateEventArgs"/>.
        /// </summary>
        /// <param name="engineTime">The game engine time.</param>
        public OnUpdateEventArgs(IEngineTiming engineTime)
        {
            EngineTime = engineTime;
        }
        #endregion
    }
}

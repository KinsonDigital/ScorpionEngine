using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Events
{
    /// <summary>
    /// Information about when a complete stack of frames finishes.
    /// </summary>
    public class FrameStackFinishedEventArgs
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="FrameStackFinishedEventArgs"/>.
        /// </summary>
        /// <param name="totalFramesRan">The total amount of frames that ran in the last frame stack.</param>
        public FrameStackFinishedEventArgs(int totalFramesRan)
        {
            TotalFramesRan = totalFramesRan;
        }
        #endregion


        #region Props
        public int TotalFramesRan { get; set; }
        #endregion
    }
}

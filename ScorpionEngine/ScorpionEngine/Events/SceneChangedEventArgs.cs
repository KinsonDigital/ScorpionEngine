using ScorpionEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.EventArguments
{
    /// <summary>
    /// Holds information about the 
    /// </summary>
    public class SceneChangedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SceneChangedEventArgs"/>.
        /// </summary>
        public SceneChangedEventArgs(string previousSceneName, string currentSceneName)
        {
            PreviousScene = previousSceneName;
            CurrentScene = currentSceneName;
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the previous scene that was running.
        /// </summary>
        public string PreviousScene { get; private set; }

        /// <summary>
        /// Gets the current scene that is now running.
        /// </summary>
        public string CurrentScene { get; private set; }
        #endregion
    }
}

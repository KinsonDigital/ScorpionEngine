// <copyright file="SceneChangedEventArgs.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System;

namespace KDScorpionEngine.Events
{
    /// <summary>
    /// Holds information about when a scene changes.
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
        /// Gets the current scene that is running.
        /// </summary>
        public string CurrentScene { get; private set; }
        #endregion
    }
}

// <copyright file="SceneNotFoundException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Exceptions
{
    using System;

    /// <summary>
    /// Thrown if a scene ID already exists.
    /// </summary>
    public class SceneNotFoundException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="SceneNotFoundException"/>.
        /// </summary>
        /// <param name="sceneId">The ID of the scene that already exists.</param>
        public SceneNotFoundException(int sceneId) : base($"The scene with the ID of '{sceneId}' was not found.") { }

        /// <summary>
        /// Creates a new instance of <see cref="SceneNotFoundException"/>.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public SceneNotFoundException(string message) : base(message) { }
        #endregion
    }
}

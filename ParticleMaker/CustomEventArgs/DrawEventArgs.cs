using Microsoft.Xna.Framework;
using System;

namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about a draw event.
    /// </summary>
    public class DrawEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="DrawEventArgs"/>.
        /// </summary>
        /// <param name="gameTime">The game time that has passed since the last frame.</param>
        public DrawEventArgs(GameTime gameTime) => GameTime = gameTime;
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the game time.
        /// </summary>
        public GameTime GameTime { get; }
        #endregion
    }
}

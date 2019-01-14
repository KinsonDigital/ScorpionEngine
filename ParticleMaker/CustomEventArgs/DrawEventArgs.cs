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
        public DrawEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
        #endregion


        #region Props
        public GameTime GameTime { get; }
        #endregion
    }
}

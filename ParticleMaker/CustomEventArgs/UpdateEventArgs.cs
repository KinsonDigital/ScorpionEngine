using Microsoft.Xna.Framework;
using System;

namespace ParticleMaker.CustomEventArgs
{
    /// <summary>
    /// Holds information about an update event.
    /// </summary>
    public class UpdateEventArgs : EventArgs
    {
        #region Constructors
        public UpdateEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
        #endregion


        #region Props
        public GameTime GameTime { get; }
        #endregion
    }
}

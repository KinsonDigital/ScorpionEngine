using System;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine.Utils
{
    public class DebugFont
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of DebugFont.
        /// </summary>
        public DebugFont()
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets the sprite font for the engine to draw.
        /// </summary>
        internal SpriteFont Font { get; }
        #endregion
    }
}
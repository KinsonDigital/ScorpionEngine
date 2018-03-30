using System;
using Microsoft.Xna.Framework.Graphics;

namespace ScorpionEngine.Utils
{
    public class DebugFont
    {
        #region Fields
        private SpriteFont _spriteFont;//The spritefont for the engine to draw
        #endregion

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
        internal SpriteFont Font
        {
            get { return _spriteFont; }
        }
        #endregion

    }
}

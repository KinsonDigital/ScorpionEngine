using ScorpionEngine.Content;
using SysColor = System.Drawing.Color;
using SysFont = System.Drawing.Font;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Text that can be drawn to the screen.
    /// </summary>
    public class TextObject : MovableObject
    {
        #region Fields
        private SysColor _foreColor = SysColor.Black;
        private SysColor _backColor = SysColor.FromArgb(0, 0, 0, 0);
        private SysFont _font;
        private string _text;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of TextEntity.
        /// </summary>
        /// <param name="text">The text of the font texture.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The color of the background behind the text.</param>
        public TextObject(string text, SysFont font, SysColor foreColor, SysColor backColor)
        {
            _text = text;
            _font = font;
            _foreColor = foreColor;
            _backColor = backColor;
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets the text of this TextEntity.
        /// </summary>
        public string Text
        {
            get { return _text; }

            set
            { 
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public SysColor ForeColor
        {
            get { return _foreColor; }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the back color of the text.
        /// </summary>
        public SysColor BackColor
        {
            get { return _backColor; }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the font of the text entity.
        /// </summary>
        public SysFont TextFont
        {
            get { return _font; }
            set
            {
            }
        }
        #endregion
    }
}

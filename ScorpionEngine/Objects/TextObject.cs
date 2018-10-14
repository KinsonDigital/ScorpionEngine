using ScorpionEngine.Content;
using ScorpionEngine.Physics;
using SysColor = System.Drawing.Color;
using SysFont = System.Drawing.Font;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Text that can be drawn to the screen.
    /// </summary>
    public class TextObject : DynamicEntity
    {
        #region Fields
        private SysColor _foreColor = SysColor.Black;
        private SysColor _backColor = SysColor.FromArgb(0, 0, 0, 0);
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
        public TextObject(string text, SysColor foreColor, SysColor backColor, Vector position) : base(position)
        {
            _text = text;
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
        #endregion
    }
}

using System.Drawing;
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
        private Color _foreColor = Color.Black;
        private Color _backColor = Color.FromArgb(0, 0, 0, 0);
        private Font _font;
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
        public TextObject(string text, SysFont font, SysColor foreColor, SysColor backColor) : base("")
        {
            _text = text;
            _font = font;
            _foreColor = foreColor;
            _backColor = backColor;

            _texture = ContentLoader.LoadFontTexture(text, font, foreColor, backColor);
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
                //If the incoming text is different then the current text
                var isDiff = (_text != value) ? true : false;

                _text = value; 

                //If the text has changed, recreate the text entity texture
                if (isDiff)
                    _texture = ContentLoader.LoadFontTexture(_text, _font, _foreColor, _backColor);
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                //If the incoming forecolor is different then the current text
                var isDiff = (_foreColor != value) ? true : false;

                _foreColor = value;

                //If the forecolor has changed, recreate the text entity texture
                if (isDiff)
                    _texture = ContentLoader.LoadFontTexture(_text, _font, _foreColor, _backColor);
            }
        }

        /// <summary>
        /// Gets or sets the back color of the text.
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                //If the incoming forecolor is different then the current text
                var isDiff = (_foreColor != value) ? true : false;

                _backColor = value;

                //If the backcolor has changed, recreate the text entity texture
                if (isDiff)
                    _texture = ContentLoader.LoadFontTexture(_text, _font, _foreColor, _backColor);
            }
        }

        /// <summary>
        /// Gets or sets the font of the text entity.
        /// </summary>
        public Font TextFont
        {
            get { return _font; }
            set
            {
                //If the font has changed, recreate the text entity texture
                if (_font.Equals(value))
                {
                    _font = value;
                    _texture = ContentLoader.LoadFontTexture(_text, _font, _foreColor, _backColor);
                }
            }
        }
        #endregion
    }
}

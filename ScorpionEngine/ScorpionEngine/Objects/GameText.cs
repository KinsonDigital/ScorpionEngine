using System.Drawing;

namespace ScorpionEngine.Objects
{
    /// <summary>
    /// Represents some text that can be drawn and manipulated on the screen.
    /// </summary>
    public class GameText : ControllableObject
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of GameText.
        /// </summary>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="textFont">The font of the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The backcolor of the text to be drawn.</param>
        public GameText(string text, Font textFont, Color foreColor, Color backColor)
        {
            Text = text;
            TextFont = textFont;
            Forecolor = foreColor;
            Backcolor = backColor;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the font of the text.
        /// </summary>
        public Font TextFont { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color Forecolor { get; set; }

        /// <summary>
        /// Gets or sets the backcolor of the text.
        /// </summary>
        public Color Backcolor { get; set; }

        #endregion
    }
}

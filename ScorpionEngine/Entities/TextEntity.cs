using ScorpionCore;
using ScorpionEngine.Physics;
using SysColor = System.Drawing.Color;

namespace ScorpionEngine.Entities
{
    /// <summary>
    /// Text that can be drawn to the screen.
    /// </summary>
    public class TextEntity : Entity
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of TextEntity.
        /// </summary>
        /// <param name="text">The text of the font texture.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The color of the background behind the text.</param>
        public TextEntity(string text, SysColor foreColor, SysColor backColor, Vector position) : base(new Vector[0], position: position)
        {
            Text = text;
            ForeColor = foreColor;
            BackColor = backColor;
        }
        #endregion


        #region Properties
        /// <summary>
        /// Gets or sets the text of this TextEntity.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public SysColor ForeColor { get; set; } = SysColor.Black;

        /// <summary>
        /// Gets or sets the back color of the text.
        /// </summary>
        public SysColor BackColor { get; set; } = SysColor.FromArgb(0, 0, 0, 0);
        #endregion
    }
}

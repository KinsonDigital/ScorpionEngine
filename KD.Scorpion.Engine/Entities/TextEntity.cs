using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngine.Entities
{
    /// <summary>
    /// Text that can be drawn to the screen.
    /// </summary>
    public class TextEntity : Entity
    {
        #region Constructors
        internal TextEntity(IPhysicsBody body) : base(body) => Setup(string.Empty, new GameColor(255, 0, 0, 0), new GameColor(0, 0, 0, 0));


        /// <summary>
        /// Creates a new instance of TextEntity.
        /// </summary>
        /// <param name="text">The text of the font texture.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The color of the background behind the text.</param>
        [ExcludeFromCodeCoverage]
        public TextEntity(string text, GameColor foreColor, GameColor backColor, Vector position) : base(new Vector[0], position: position) =>
            Setup(text, foreColor, backColor);
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the text of this TextEntity.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public GameColor ForeColor { get; set; } = new GameColor(255, 0, 0, 0);

        /// <summary>
        /// Gets or sets the back color of the text.
        /// </summary>
        public GameColor BackColor { get; set; } = new GameColor(0, 0, 0, 0);
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the <see cref="TextEntity"/>.
        /// </summary>
        /// <param name="text">The text of the font texture.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The color of the background behind the text.</param>
        private void Setup(string text, GameColor foreColor, GameColor backColor)
        {
            Text = text;
            ForeColor = foreColor;
            BackColor = backColor;
        }
        #endregion
    }
}

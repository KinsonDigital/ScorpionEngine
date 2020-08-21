// <copyright file="TextEntity.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Entities
{
    using System.Diagnostics.CodeAnalysis;
    using System.Numerics;
    using Raptor.Graphics;
    using Raptor.Plugins;

    /// <summary>
    /// Text that can be drawn to the screen.
    /// </summary>
    public class TextEntity : Entity
    {
        /// <summary>
        /// Creates a new instance of <see cref="TextEntity"/>.
        /// USED FOR UNIT TESTING.
        /// </summary>
        /// <param name="body">The physics body to inject.</param>
        internal TextEntity(IPhysicsBody body)
            : base(body) => Setup(string.Empty, new GameColor(255, 0, 0, 0), new GameColor(0, 0, 0, 0));

        /// <summary>
        /// Creates a new instance of <see cref="TextEntity"/>.
        /// </summary>
        /// <param name="text">The text of entity.</param>
        /// <param name="foreColor">The color of the text.</param>
        /// <param name="backColor">The color of the background behind the text.</param>
        [ExcludeFromCodeCoverage]
        public TextEntity(string text, GameColor foreColor, GameColor backColor, Vector2 position)
            : base(System.Array.Empty<Vector2>(), position: position) =>
            Setup(text, foreColor, backColor);

        /// <summary>
        /// Gets or sets the text.
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
    }
}

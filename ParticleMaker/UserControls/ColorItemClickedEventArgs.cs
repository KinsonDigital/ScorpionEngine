using System;
using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Holds information about when a user clicks a <see cref="ColorItem"/> with the mouse.
    /// </summary>
    public class ColorItemClickedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ColorItemClickedEventArgs"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="ColorItem"/> that was clicked.</param>
        /// <param name="color">The color of the item that was clicked.</param>
        public ColorItemClickedEventArgs(int id, Color color)
        {
            Id = id;
            Color = color;
        }
        #endregion


        #region Props
        /// <summary>
        /// The ID of the <see cref="ColorItem"/>.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The <see cref="Color"/> of the <see cref="ColorItem"/>.
        /// </summary>
        public Color Color { get; private set; }
        #endregion
    }
}

using System.Windows.Media;

namespace ParticleMaker
{
    /// <summary>
    /// Represents a color with a unique ID.
    /// </summary>
    public class ColorItem
    {
        #region Props
        /// <summary>
        /// The unique ID of the color item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The color brush of the item.
        /// </summary>
        public SolidColorBrush ColorBrush { get; set; }
        #endregion


        #region Public Methods
        /// <summary>
        /// Returns the string representation of the <see cref="ColorItem"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"R:{ColorBrush.Color.R}, G:{ColorBrush.Color.G}, B:{ColorBrush.Color.B}";
        }
        #endregion
    }
}

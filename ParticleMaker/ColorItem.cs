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
            return $"R:{ColorBrush.Color.R}, G:{ColorBrush.Color.G}, B:{ColorBrush.Color.B}, A:{ColorBrush.Color.A}";
        }


        /// <summary>
        /// Returns a value indicating if the given obj and this object are equal.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var objToCompare = (ColorItem)obj;

            if (objToCompare == null)
                return false;

            var colorMatches = objToCompare.ColorBrush.Color.A == ColorBrush.Color.A &&
                               objToCompare.ColorBrush.Color.R == ColorBrush.Color.R &&
                               objToCompare.ColorBrush.Color.G == ColorBrush.Color.G &&
                               objToCompare.ColorBrush.Color.B == ColorBrush.Color.B;


            return objToCompare.Id == Id && colorMatches;
        }
        #endregion
    }
}

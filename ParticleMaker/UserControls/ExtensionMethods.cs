using System.Windows.Media;

namespace ParticleMaker.UserControls
{
    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Methods
        /// <summary>
        /// Returns the color negative of the given <paramref name="color"/>.
        /// </summary>
        /// <param name="color">The color to convert to a negative.</param>
        /// <returns></returns>
        public static SolidColorBrush ToNegativeBrush(this Color color)
        {
            var negativeForecolor = Color.FromArgb(255, (byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));


            return new SolidColorBrush(negativeForecolor);
        }


        /// <summary>
        /// Returns the color negative of the given <paramref name="brush"/>.
        /// </summary>
        /// <param name="brush">The brush to convert to a negative.</param>
        /// <returns></returns>
        public static SolidColorBrush ToNegative(this SolidColorBrush brush) => brush.Color.ToNegativeBrush();
        #endregion
    }
}

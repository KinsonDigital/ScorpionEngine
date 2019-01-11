using KDScorpionCore.Graphics;
using System.Windows.Media;

namespace ParticleMaker
{
    /// <summary>
    /// Provides various extension methods for use throughout the application.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Fields
        private const float PI = 3.1415926535897931f;
        #endregion


        #region Methods
        /// <summary>
        /// Converts the given degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        /// <returns></returns>
        public static float ToRadians(this float degrees)
        {
            return degrees * PI / 180f;
        }


        /// <summary>
        /// Returns the given <see cref="GameColor"/> type to a <see cref="ColorItem"/>.
        /// </summary>
        /// <param name="clr">The color to convert.</param>
        /// <returns></returns>
        public static ColorItem ToColorItem(this GameColor clr)
        {
            return new ColorItem()
            {
                ColorBrush = new SolidColorBrush(Color.FromArgb(clr.Alpha, clr.Red, clr.Green, clr.Blue))
            };
        }
        #endregion
    }
}

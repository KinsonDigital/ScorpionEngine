using KDScorpionCore.Graphics;
using System.Windows.Media;

namespace ParticleMaker
{
    public static class ExtensionMethods
    {
        private const float PI = 3.1415926535897931f;


        /// <summary>
        /// Converts the given degrees to radians.
        /// </summary>
        /// <param name="degrees">The degrees to convert.</param>
        /// <returns></returns>
        public static float ToRadians(this float degrees)
        {
            return degrees * PI / 180f;
        }


        public static ColorItem ToColorItem(this GameColor clr)
        {
            return new ColorItem()
            {
                ColorBrush = new SolidColorBrush(Color.FromArgb(clr.Alpha, clr.Red, clr.Green, clr.Blue))
            };
        }
    }
}

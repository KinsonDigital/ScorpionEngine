using ScorpionEngine.Input;
using ScorpionEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Utils
{
    /// <summary>
    /// Provides basic tools to make things easier internally in the engine.
    /// </summary>
    public static class Tools
    {
        private const float PI = 3.1415926535897931f;

        #region Public Methods
        /// <summary>
        /// Gets the minimum value of the given enumeration.
        /// </summary>
        /// <param name="enumType">The enum to process.</param>
        /// <returns></returns>
        public static int GetEnumMin(Type enumType)
        {
            var values = Enum.GetValues(enumType);

            //Get the smallest item out of all of the values
            return values.Cast<int>().Concat(new[] { int.MaxValue }).Min();
        }


        /// <summary>
        /// Rotates the <paramref name="vector"/> around the <paramref name="rotateOrigin"/> at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="origin">The vector to rotate the <paramref name="vector"/> around.</param>
        /// <param name="angle">The angle in radians to rotate <paramref name="vector"/>.  Value must be positive.</param>
        /// <returns></returns>
        public static Vector RotateAround(Vector vector, Vector origin, float angle, bool clockWise = true)
        {
            angle = clockWise ? angle : angle * -1;

            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);

            var dx = vector.X - origin.X;//The delta x
            var dy = vector.Y - origin.Y;//The delta y

            var tempX = dx * cos - dy * sin;
            var tempY = dx * sin + dy * cos;

            var x = tempX + origin.X;
            var y = tempY + origin.Y;


            return new Vector(x, y);
        }


        public static float ToDegrees(this float radians)
        {
            return radians * 180.0f / PI;
        }


        public static float ToRadians(this float degrees)
        {
            return degrees * PI / 180f;
        }
        #endregion
    }
}

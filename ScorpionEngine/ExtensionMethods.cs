using ScorpionEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine
{
    public static class ExtensionMethods
    {
        private const float PI = 3.1415926535897931f;


        public static float ToDegrees(this float radians)
        {
            return radians * 180.0f / PI;
        }


        public static float ToRadians(this float degrees)
        {
            return degrees * PI / 180f;
        }


        /// <summary>
        /// Sets the value to positive if its negative.
        /// </summary>
        /// <param name="value">The value to force.</param>
        /// <returns></returns>
        public static float ForcePositive(this float value)
        {
            return value < 0 ? value * -1 : value;
        }


        /// <summary>
        /// Sets the value to negative if its positive.
        /// </summary>
        /// <param name="value">The value to force.</param>
        /// <returns></returns>
        public static float ForceNegative(this float value)
        {
            return value > 0 ? value * -1 : value;
        }


        /// <summary>
        /// Rotates the <paramref name="vector"/> around the <paramref name="rotateOrigin"/> at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="origin">The vector to rotate the <paramref name="vector"/> around.</param>
        /// <param name="angle">The angle in radians to rotate <paramref name="vector"/>.  Value must be positive.</param>
        /// <returns></returns>
        public static Vector RotateAround(this Vector vector, Vector origin, float angle, bool clockWise = true)
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
    }
}

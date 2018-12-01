using ScorpionCore;
using ScorpionEngine.Physics;
using System;

namespace ScorpionEngine
{
    public static class ExtensionMethods
    {
        private const float PI = 3.1415926535897931f;


        /// <summary>
        /// Converts the given radians value into degrees.
        /// </summary>
        /// <param name="radians">The value to convert.</param>
        /// <returns></returns>
        public static float ToDegrees(this float radians)
        {
            return radians * 180.0f / PI;
        }


        /// <summary>
        /// Converts the given degrees value into radians.
        /// </summary>
        /// <param name="degrees">The value to convert.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Returns a random value between the given <paramref name="minValue"/> and <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="minValue">The minimum value that the result will be.</param>
        /// <param name="maxValue">The maximum value that the result will be.</param>
        /// <returns></returns>
        public static float Next(this Random random, float minValue, float maxValue)
        {
            var minValueAsInt = (int)(minValue * 1000);
            var maxValueAsInt = (int)(maxValue * 1000);

            var randomResult = random.Next(minValueAsInt, maxValueAsInt);


            return randomResult / 1000f;
        }


        /// <summary>
        /// Returns a true/false value that represents the flip of a coin.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <returns></returns>
        public static bool FlipCoin(this Random random)
        {
            return random.NextDouble() <= 0.5f;
        }
    }
}

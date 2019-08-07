using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace KDParticleEngine
{
    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Public Methods
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

            if (minValueAsInt > maxValueAsInt)
                return maxValue;

            var randomResult = random.Next(minValueAsInt, maxValueAsInt);

            return randomResult / 1000f;
        }


        /// <summary>
        /// Returns a true/false value that represents the flip of a coin.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static bool FlipCoin(this Random random) => random.NextDouble() <= 0.5f;


        /// <summary>
        /// Adds the given <paramref name="point"/>'s X and Y components to this point and returns the result.
        /// </summary>
        /// <param name="otherPoint">The current point to add the given point to.</param>
        /// <param name="point">The point to add to this point.</param>
        /// <returns></returns>
        public static PointF Add(this PointF otherPoint, PointF point) => new PointF(otherPoint.X + point.X, otherPoint.Y + point.Y);
        #endregion
    }
}

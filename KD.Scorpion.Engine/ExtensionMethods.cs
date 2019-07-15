using System;
using System.Diagnostics.CodeAnalysis;

namespace KDScorpionEngine
{
    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns a random value between the given <paramref name="minValue"/> and <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="minValue">The minimum value that the result will be.</param>
        /// <param name="maxValue">The maximum value that the result will be.</param>
        /// <returns></returns>
        public static float Next(this Random random, float minValue, float maxValue)
        {
            return random.Next((int)(minValue * 1000), (int)(maxValue * 1000)) / 1000f;
        }


        /// <summary>
        /// Returns a true/false value that represents the flip of a coin.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static bool FlipCoin(this Random random)
        {
            return random.NextDouble() <= 0.5f;
        }
    }
}

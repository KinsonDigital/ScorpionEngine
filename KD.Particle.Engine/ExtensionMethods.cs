using KDScorpionCore.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KDParticleEngine
{
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
        public static bool FlipCoin(this Random random)
        {
            return random.NextDouble() <= 0.5f;
        }


        /// <summary>
        /// Converts this <see cref="ParticleColor"/> to a <see cref="GameColor"/>.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns></returns>
        public static GameColor ToGameColor(this ParticleColor color)
        {
            return new GameColor(color.Red, color.Green, color.Blue, color.Alpha);
        }


        /// <summary>
        /// Converts all of the given <see cref="ParticleColor"/>s to <see cref="GameColor"/>s.
        /// </summary>
        /// <param name="colors">The list of colors to convert.</param>
        /// <returns></returns>
        public static GameColor[] ToGameColors(this ParticleColor[] colors)
        {
            var result = new List<GameColor>();

            foreach (var clr in colors)
            {
                result.Add(clr.ToGameColor());
            }


            return result.ToArray();
        }


        /// <summary>
        /// Converts this <see cref="ParticleColor"/> to a <see cref="GameColor"/>.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns></returns>
        public static ParticleColor ToParticleColor(this GameColor color)
        {
            return new ParticleColor(color.Alpha, color.Red, color.Green, color.Blue);
        }


        /// <summary>
        /// Converts all of the given <see cref="ParticleColor"/>s to <see cref="GameColor"/>s.
        /// </summary>
        /// <param name="colors">The list of colors to convert.</param>
        /// <returns></returns>
        public static ParticleColor[] ToParticleColors(this GameColor[] colors)
        {
            var result = new List<ParticleColor>();

            foreach (var clr in colors)
            {
                result.Add(clr.ToParticleColor());
            }


            return result.ToArray();
        }
    }
}

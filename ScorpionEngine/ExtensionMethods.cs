// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Numerics;

    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns a random float that is whithin a specified range.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="minValue">The minimum value that the result will be.</param>
        /// <param name="maxValue">The maximum value that the result will be.</param>
        /// <returns>
        ///     A 32-bit signed float greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>;that is,
        ///     the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        ///     If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
        /// </returns>
        public static float Next(this Random random, float minValue, float maxValue) => random.Next((int)(minValue * 1000), (int)(maxValue * 1000)) / 1000f;

        /// <summary>
        /// Returns a true/false value that represents the flip of a coin.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <returns>True or false results.</returns>
        [ExcludeFromCodeCoverage]
        public static bool FlipCoin(this Random random) => random.NextDouble() <= 0.5f;

        /// <summary>
        /// Returns a value indicating if the given <paramref name="vector"/>
        /// is contained by the <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rectangle that might contain the <paramref name="vector"/>.</param>
        /// <param name="vector">The vector that might be contained by the <paramref name="rect"/>.</param>
        /// <returns>True if the <paramref name="vector"/> is contained by the rectangle.</returns>
        public static bool Contains(this Rectangle rect, Vector2 vector)
        {
            var point = new Point((int)vector.X, (int)vector.Y);

            return rect.Contains(point);
        }

        /// <summary>
        /// Converts the negative of positive number to be positived.
        /// </summary>
        /// <param name="value">The value to change.</param>
        /// <returns>The negative result of value.</returns>
        public static float ToNegative(this float value) => value < 0 ? value : value * -1f;

        /// <summary>
        /// Converts the positive number to be negative.
        /// </summary>
        /// <param name="value">The value to change.</param>
        /// <returns>The absolute result of the value.</returns>
        public static float ToPositive(this float value) => value >= 0 ? value : value * -1f;

        /// <summary>
        /// Rotates the <paramref name="vector"/> around the <paramref name="rotateOrigin"/> at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="origin">The vector to rotate the <paramref name="vector"/> around.</param>
        /// <param name="angle">The angle in radians to rotate <paramref name="vector"/>.  Value must be positive.</param>
        /// <param name="clockWise">True to rotate the point clockwise and false for counter clockwise.</param>
        /// <returns>A vector rotated around a point.</returns>
        public static Vector2 RotateAround(this Vector2 vector, Vector2 origin, float angle, bool clockWise = true)
        {
            angle = clockWise ? angle : angle * -1;

            var cos = (float)Math.Cos(angle);
            var sin = (float)Math.Sin(angle);

            var dx = vector.X - origin.X; // The delta x
            var dy = vector.Y - origin.Y; // The delta y

            var tempX = (dx * cos) - (dy * sin);
            var tempY = (dx * sin) + (dy * cos);

            var x = tempX + origin.X;
            var y = tempY + origin.Y;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Converts the given radians value into degrees.
        /// </summary>
        /// <param name="radians">The value to convert.</param>
        /// <returns>The degrees result.</returns>
        public static float ToDegrees(this float radians) => radians * 180.0f / (float)Math.PI;

        /// <summary>
        /// Converts the given degrees value into radians.
        /// </summary>
        /// <param name="degrees">The value to convert.</param>
        /// <returns>The radians result.</returns>
        public static float ToRadians(this float degrees) => degrees * (float)Math.PI / 180f;
    }
}

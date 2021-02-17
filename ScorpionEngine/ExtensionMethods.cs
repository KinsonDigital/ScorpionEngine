﻿// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
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
        /// Returns a random value between the given <paramref name="minValue"/> and <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <param name="minValue">The minimum value that the result will be.</param>
        /// <param name="maxValue">The maximum value that the result will be.</param>
        /// <returns></returns>
        public static float Next(this Random random, float minValue, float maxValue) => random.Next((int)(minValue * 1000), (int)(maxValue * 1000)) / 1000f;

        /// <summary>
        /// Returns a true/false value that represents the flip of a coin.
        /// </summary>
        /// <param name="random">The random instance to use.</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static bool FlipCoin(this Random random) => random.NextDouble() <= 0.5f;

        public static bool Contains(this Rectangle rect, Vector2 vector)
        {
            var point = new Point((int)vector.X, (int)vector.Y);

            return rect.Contains(point);
        }

        public static float ToNegative(this float value) => value < 0 ? value : value * -1f;
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
        /// <returns></returns>
        public static float ToDegrees(this float radians)
        {
            return radians * 180.0f / (float)Math.PI;
        }

        /// <summary>
        /// Converts the given degrees value into radians.
        /// </summary>
        /// <param name="degrees">The value to convert.</param>
        /// <returns></returns>
        public static float ToRadians(this float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }
    }
}

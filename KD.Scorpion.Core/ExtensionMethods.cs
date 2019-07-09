﻿using System;

namespace KDScorpionCore
{
    /// <summary>
    /// Provides various methods for the <see cref="KDScorpionCore"/> library to utilize.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Private Fields
        private const float PI = 3.1415926535897931f;
        #endregion


        #region Public Methods
        /// <summary>
        /// Converts the given <paramref name="radians"/> value into degrees.
        /// </summary>
        /// <param name="radians">The value to convert.</param>
        /// <returns></returns>
        public static float ToDegrees(this float radians) => radians * 180.0f / PI;


        /// <summary>
        /// Converts the given <paramref name="degrees"/> value into radians.
        /// </summary>
        /// <param name="degrees">The value to convert.</param>
        /// <returns></returns>
        public static float ToRadians(this float degrees) => degrees * PI / 180f;


        /// <summary>
        /// Sets the value to positive if its negative.
        /// </summary>
        /// <param name="value">The value to force.</param>
        /// <returns></returns>
        public static float ForcePositive(this float value) => value < 0 ? value * -1 : value;


        /// <summary>
        /// Sets the value to negative if its positive.
        /// </summary>
        /// <param name="value">The value to force.</param>
        /// <returns></returns>
        public static float ForceNegative(this float value) => value > 0 ? value * -1 : value;


        /// <summary>
        /// Rotates the <paramref name="vector"/> around the <paramref name="origin"/> at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="origin">The origin to rotate the <paramref name="vector"/> around.</param>
        /// <param name="angle">The angle in degress to rotate <paramref name="vector"/>.  Value must be positive.</param>
        /// <returns></returns>
        public static Vector RotateAround(this Vector vector, Vector origin, float angle, bool clockWise = true)
        {
            var angleRadians = clockWise ? angle.ToRadians() : angle.ToRadians() * -1;

            var cos = (float)Math.Cos(angleRadians);
            var sin = (float)Math.Sin(angleRadians);

            var dx = vector.X - origin.X;//The delta x
            var dy = vector.Y - origin.Y;//The delta y

            var tempX = dx * cos - dy * sin;
            var tempY = dx * sin + dy * cos;

            var x = tempX + origin.X;
            var y = tempY + origin.Y;


            return new Vector(x, y);
        }
        #endregion
    }
}

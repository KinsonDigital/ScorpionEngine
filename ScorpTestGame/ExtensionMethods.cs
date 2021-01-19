// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace ScorpTestGame
{
    using System;
    using System.Drawing;
    using System.Numerics;
    using Raptor;

    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts the given <see cref="TimeSpan"/> to the type <see cref="GameTime"/>.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <returns></returns>
        public static FrameTime ToGameTime(this TimeSpan time) => new FrameTime() { ElapsedTime = time, TotalTime = new TimeSpan() };

        /// <summary>
        /// Converts the given <see cref="TimeSpan"/> to the type <see cref="GameTime"/>.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this FrameTime time) => time.ElapsedTime;

        /// <summary>
        /// Converts the given <paramref name="vector"/> of type <see cref="Vector2"/> to the type <see cref="PointF"/>.
        /// </summary>
        /// <param name="vector">The vector to convert.</param>
        /// <returns></returns>
        public static PointF ToPointF(this Vector2 vector) => new PointF(vector.X, vector.Y);

        /// <summary>
        /// Converts the given <paramref name="point"/> of type <see cref="PointF"/> to type <see cref="Vector2"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        /// <returns></returns>
        public static Vector2 ToVector(this PointF point) => new Vector2(point.X, point.Y);
    }
}

// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Numerics;
    using SimpleInjector;
    using SimpleInjector.Diagnostics;

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
        /// <returns>
        ///     <see langword="true"/> or false results.
        /// </returns>
        [ExcludeFromCodeCoverage]
        public static bool FlipCoin(this Random random) => random.NextDouble() <= 0.5f;

        /// <summary>
        /// Returns a value indicating if the given <paramref name="vector"/>
        /// is contained by the <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="rect">The rectangle that might contain the <paramref name="vector"/>.</param>
        /// <param name="vector">The vector that might be contained by the <paramref name="rect"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the <paramref name="vector"/> is contained by the rectangle.
        /// </returns>
        public static bool Contains(this Rectangle rect, Vector2 vector)
        {
            var point = new Point((int)vector.X, (int)vector.Y);

            return rect.Contains(point);
        }

        /// <summary>
        /// Converts the given <param name="value"/> to a negative number.
        /// </summary>
        /// <param name="value">The value to change.</param>
        /// <returns>The negative result of value.</returns>
        public static float ToNegative(this float value) => value < 0 ? value : value * -1f;

        /// <summary>
        /// Converts the given <param name="value"/> to a positive number.
        /// </summary>
        /// <param name="value">The value to change.</param>
        /// <returns>The absolute result of the value.</returns>
        public static float ToPositive(this float value) => value >= 0 ? value : value * -1f;

        /// <summary>
        /// Rotates the <paramref name="vector"/> around the <paramref name="origin"/> at the given <paramref name="angle"/>.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="origin">The point to rotate the <paramref name="vector"/> around.</param>
        /// <param name="angle">The angle in radians to rotate <paramref name="vector"/>.</param>
        /// <param name="clockWise">True to rotate the point clockwise and false for counter clockwise.</param>
        /// <returns>A vector rotated around a point.</returns>
        /// <remarks>
        ///     The <param name="angle"/> must be a positive number.
        /// </remarks>
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
        /// Converts the given <param name="radians"/> value into degrees.
        /// </summary>
        /// <param name="radians">The value to convert.</param>
        /// <returns>The degrees result.</returns>
        public static float ToDegrees(this float radians) => radians * 180.0f / (float)Math.PI;

        /// <summary>
        /// Converts the given <param name="degrees"/> value into radians.
        /// </summary>
        /// <param name="degrees">The value to convert.</param>
        /// <returns>The radians result.</returns>
        public static float ToRadians(this float degrees) => degrees * (float)Math.PI / 180f;

        /// <summary>
        /// Converts the given list of items of type <typeparamref name="T"/>
        /// to a <see cref="ReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of item in the current array of items and resulting collection.</typeparam>
        /// <param name="items">The list of items to convert.</param>
        /// <returns>The original list of items converted to a read only collection.</returns>
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this T[] items)
            => new ReadOnlyCollection<T>(items);

        /// <summary>
        /// Registers that a new instance of <typeparamref name="TConcrete"/> will be returned every time it is requested (transient).
        /// <para>
        ///     This method uses the container's <see cref="ContainerOptions.LifestyleSelectionBehavior"/> to select the exact
        ///     lifestyle for the specified type.  By defautl this will be <see cref="Lifestyle.Transient"/>.
        /// </para>
        /// <para>
        ///     Ignores any warnings when a component is registered as transient, while implementing <see cref="IDisposable"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="TConcrete">The concrete type that will be registered.</typeparam>
        /// <param name="container">
        ///     The container to use to create an instance of type <typeparamref name="TConcrete"/>
        ///     for registration of dependencies.
        /// </param>
        [ExcludeFromCodeCoverage]
        public static void RegisterAndIgnoreDisposableWarnings<TConcrete>(this Container container)
            where TConcrete : class
        {
            container.Register<TConcrete>();
            var registration = container.GetRegistration<TConcrete>()?.Registration;

            if (registration is null)
            {
                throw new Exception($"No registraction found for type '{typeof(TConcrete)}'.");
            }

            registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposed by application code.");
        }

        /// <summary>
        /// Returns the <paramref name="items"/> as a <see cref="ReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="items">The items to convert.</param>
        /// <returns>The original items as a read only colleciton.</returns>
        internal static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this List<T> items)
            => new ReadOnlyCollection<T>(items.ToArray());
    }
}

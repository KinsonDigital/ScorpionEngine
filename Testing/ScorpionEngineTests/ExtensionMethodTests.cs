// <copyright file="ExtensionMethodTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using System.Numerics;
    using KDScorpionEngine;
    using Raptor;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KDScorpionEngine.ExtensionMethods"/> class.
    /// </summary>
    public class ExtensionMethodTests
    {
        #region Method Tests
        [Fact]
        public void ToDegress_WhenInvoking_ReturnsCorrectValue()
        {
            // Arrange
            var expected = 45.0f;

            // Act
            var actual = 0.785398185f.ToDegrees();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToRadians_WhenInvoking_ReturnsCorrectValue()
        {
            // Arrange
            var expected = 0.785398185f;

            // Act
            var actual = 45f.ToRadians();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForcePositive_WhenNegative_ReturnsPositiveResult()
        {
            // Arrange
            var expected = 10f;

            // Act
            var actual = (-10f).ForcePositive();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForcePositive_WhenPositive_ReturnsPositiveResult()
        {
            // Arrange
            var expected = 10f;

            // Act
            var actual = 10f.ForcePositive();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForceNegative_WhenNegative_ReturnsNegativeResult()
        {
            // Arrange
            var expected = -10f;

            // Act
            var actual = (-10f).ForceNegative();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForceNegative_WhenPositive_ReturnsNegativeResult()
        {
            // Arrange
            var expected = -10f;

            // Act
            var actual = 10f.ForceNegative();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RotateAround_WhenRotatingClockWise_ReturnsCorrectResult()
        {
            // Arrange
            var vector = new Vector2(0, 0);

            var expected = new Vector2(5f, -2.07106781f);

            // Act
            var actual = vector.RotateAround(new Vector2(5, 5), 45f, true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RotateAround_WhenRotatingCounterClockWise_ReturnsCorrectResult()
        {
            // Arrange
            var vector = new Vector2(0, 0);

            var expected = new Vector2(-2.07106781f, 5f);

            // Act
            var actual = vector.RotateAround(new Vector2(5, 5), 45f, false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Next_WhenInvoking_ReturnsValidValueWithinRange()
        {
            // Arrange
            var random = new Random();
            var expected = true;

            // Act
            var actual = false;

            for (var i = 0; i < 1000; i++)
            {
                var randomResult = random.Next(1f, 10f);

                if (randomResult >= 1 || randomResult <= 10)
                    actual = true;
            }

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

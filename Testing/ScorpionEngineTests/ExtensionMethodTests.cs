// <copyright file="ExtensionMethodTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using System.Drawing;
    using System.Numerics;
    using KDScorpionEngine;
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
            var actual = (-10f).ToPositive();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForcePositive_WhenPositive_ReturnsPositiveResult()
        {
            // Arrange
            var expected = 10f;

            // Act
            var actual = 10f.ToPositive();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForceNegative_WhenNegative_ReturnsNegativeResult()
        {
            // Arrange
            var expected = -10f;

            // Act
            var actual = (-10f).ToNegative();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ForceNegative_WhenPositive_ReturnsNegativeResult()
        {
            // Arrange
            var expected = -10f;

            // Act
            var actual = 10f.ToNegative();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RotateAround_WhenRotatingClockWise_ReturnsCorrectResult()
        {
            // Arrange
            var vector = new Vector2(0, 0);

            var expected = new Vector2(6.6279078f, -1.8811274f);

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

            var expected = new Vector2(-1.8811274f, 6.6279078f);

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

            // Act
            var randomResult = random.Next(1f, 10f);
            var actual = randomResult >= 1f && randomResult < 10f;

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData(0, 0, 100, 100, 50, 50, true)]
        [InlineData(0, 0, 100, 100, 500, 500, false)]
        public void Contains_WhenInvoked_ReturnsCorrectResult(
            int rectX,
            int rectY,
            int rectWidth,
            int rectHeight,
            int vectorX,
            int vectorY,
            bool expected)
        {
            // Arrange
            var rectangle = new Rectangle(rectX, rectY, rectWidth, rectHeight);
            var vector = new Vector2(vectorX, vectorY);

            // Act
            var actual = rectangle.Contains(vector);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(4f, -4f)]
        [InlineData(-6f, -6f)]
        public void ToNegative_WhenInvoked_Expectation(float value, float expected)
        {
            // Act & Assert
            Assert.Equal(expected, value.ToNegative());
        }

        [Theory]
        [InlineData(-8f, 8f)]
        [InlineData(3f, 3f)]
        public void ToPositive_WhenInvoked_Expectation(float value, float expected)
        {
            // Act & Assert
            Assert.Equal(expected, value.ToPositive());
        }
        #endregion
    }
}

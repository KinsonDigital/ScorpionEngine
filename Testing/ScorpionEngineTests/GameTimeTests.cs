// <copyright file="GameTimeTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using KDScorpionEngine;
    using Xunit;

    public class GameTimeTests
    {
        #region Prop Tests
        [Fact]
        public void TotalGameMilliseconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var time = new GameTime();

            // Act
            time.AddTime(16);
            time.AddTime(16);

            // Assert
            Assert.Equal(32, time.TotalGameMilliseconds);
        }

        [Fact]
        public void TotalGameMinutes_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var time = new GameTime();

            // Act
            time.AddTime(360_000);

            // Assert
            Assert.Equal(6, time.TotalGameMinutes);
        }

        [Fact]
        public void TotalGameSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var time = new GameTime();

            // Act
            time.AddTime(12_349);

            // Assert
            Assert.Equal(12.349, time.TotalGameSeconds);
        }

        [Fact]
        public void TotalGameHours_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var time = new GameTime();

            // Act
            time.AddTime(5_400_000);

            // Assert
            Assert.Equal(1.5, time.TotalGameHours);
        }
        #endregion
    }
}

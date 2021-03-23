// <copyright file="CounterTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Utils
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Utils;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="Counter"/> class.
    /// </summary>
    public class CounterTests
    {
        #region Method Tests
        [Fact]
        public void Count_WhenInvoked_IncrementsValue()
        {
            // Arrange
            var counter = new Counter()
            {
                Min = 0,
                Max = 4,
            };

            var expected = 1;

            // Act
            counter.Count();
            var actual = counter.Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenMaxReached_InvokesMaxReachedEvent()
        {
            // Arrange
            var counter = new Counter()
            {
                Min = 0,
                Max = 2,
            };

            var expected = true;
            var actual = false;
            counter.MaxReachedWhenIncrementing += (obj, e) =>
            {
                actual = true;
            };

            // Act
            counter.Count();
            counter.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenResetTypeSetToAutoAndMaxReached_InvokeReset()
        {
            // Arrange
            var expected = 0;
            var counter = new Counter()
            {
                Min = 0,
                Max = 2,
            };

            // Act
            counter.Count();
            counter.Count();
            counter.Count();
            var actual = counter.Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenResetTypeSetToManualAndMaxReached_DoNotInvokeReset()
        {
            // Arrange
            var expected = 3;
            var counter = new Counter()
            {
                Min = 0,
                Max = 2,
                ResetMode = ResetType.Manual,
            };

            // Act
            counter.Count();
            counter.Count();
            counter.Count();
            var actual = counter.Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenDecrementingAndGreaterThanMin_DoNotInvokedMinReachedEvent()
        {
            // Arrange
            var expectedValue = 1;
            var expectedMinReached = false;
            var actualMinReached = false;
            var counter = new Counter()
            {
                Min = 0,
                Max = 2,
                Value = 2,
                CountDirection = CountType.Decrement,
            };
            counter.MinReachedWhenDecrementing += (sender, e) => actualMinReached = true;

            // Act
            counter.Count();
            var actualValue = counter.Value;

            // Assert
            Assert.Equal(expectedValue, actualValue);
            Assert.Equal(expectedMinReached, actualMinReached);
        }

        [Fact]
        public void Count_WhenInvoked_DecrementsValue()
        {
            // Arrange
            var counter = new Counter()
            {
                Min = 1,
                Max = 4,
                CountDirection = CountType.Decrement,
                ResetMode = ResetType.Manual,
            };
            var expected = -1;

            // Act
            counter.Count();
            var actual = counter.Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenMaxReached_InvokesMinReachedEvent()
        {
            // Arrange
            var counter = new Counter()
            {
                Min = 1,
                Max = 2,
                CountDirection = CountType.Decrement,
                ResetMode = ResetType.Manual,
            };
            var expected = true;
            var actual = false;
            counter.MinReachedWhenDecrementing += (obj, e) =>
            {
                actual = true;
            };

            // Act
            counter.Count();
            counter.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenResetTypeSetToAutoAndMinReached_InvokeReset()
        {
            // Arrange
            var expected = 2;
            var counter = new Counter()
            {
                Min = 0,
                Max = 2,
                CountDirection = CountType.Decrement,
            };

            // Act
            counter.Count();
            var actual = counter.Value;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Count_WhenSettingInvalidCountType_ThrowsException()
        {
            // Arrange
            var counter = new Counter()
            {
                CountDirection = (CountType)44,
            };

            // Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Count());
            Assert.Equal($"The {nameof(counter.CountDirection)} is set to an invalid enumeration value.", actual.Message);
        }

        [Fact]
        public void Reset_WhenSettingInvalidCountType_ThrowsException()
        {
            // Arrange
            var counter = new Counter()
            {
                CountDirection = (CountType)44,
            };

            // Act/Assert
            var actual = Assert.Throws<Exception>(() => counter.Reset());
            Assert.Equal($"The {nameof(counter.CountDirection)} is set to an invalid enumeration value.", actual.Message);
        }
        #endregion

        #region Prop Tests
        [Theory]
        [InlineData(1, 4, 2, 2)]
        [InlineData(1, 4, 1, 1)]
        [InlineData(1, 4, 4, 4)]
        [InlineData(1, 4, 100, 4)]
        [InlineData(1, 4, -100, -100)]
        public void Min_WhenSettingValue_ReturnsCorrectValue(int min, int max, int value, int expected)
        {
            // Arrange
            var counter = new Counter()
            {
                Min = min,
                Max = max,
            };

            // Act
            counter.Min = value;

            // Assert
            Assert.Equal(expected, counter.Min);
        }

        [Theory]
        [InlineData(1, 4, 2, 2)]
        [InlineData(1, 4, 1, 1)]
        [InlineData(1, 4, 4, 4)]
        [InlineData(1, 4, 100, 100)]
        [InlineData(1, 4, -100, 1)]
        public void Max_WhenSettingMaxMoreThanMin_ProperlySetsValue(int min, int max, int value, int expected)
        {
            // Arrange
            var counter = new Counter()
            {
                Min = min,
                Max = max,
            };

            // Act
            counter.Max = value;

            // Assert
            Assert.Equal(expected, counter.Max);
        }
        #endregion
    }
}

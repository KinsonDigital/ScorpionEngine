// <copyright file="StopWatchTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Utils
{
    using KDScorpionEngine;
    using KDScorpionEngine.Utils;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="StopWatch"/> class.
    /// </summary>
    public class StopWatchTests
    {
        #region Prop Tests
        [Fact]
        public void ResetMode_WhenSettingValue_ValueIsSetCorrectly()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var expected = ResetType.Manual;

            // Act
            stopWatch.ResetMode = ResetType.Manual;
            var actual = stopWatch.ResetMode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeOut_WhenUsingNegativeValue_SetsValueToZero()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var expected = 1;

            // Act
            stopWatch.TimeOut = -100;
            var actual = stopWatch.TimeOut;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TimeOut_WhenUsingPositiveValue_SetsValueToIncomingValue()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var expected = 100;

            // Act
            stopWatch.TimeOut = 100;
            var actual = stopWatch.TimeOut;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ElapsedSeconds_WhenSettingValue_ConvertsSecondsToMilliseconds()
        {
            // Arrange
            var stopWatch = new StopWatch();
            stopWatch.TimeOut = 2500;

            var gameTime = new GameTime();
            gameTime.AddTime(750);
            var expected = 0.75f; // Seconds

            // Act
            stopWatch.Start();
            stopWatch.Update(gameTime);
            var actual = stopWatch.ElapsedSeconds;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StopOnReset_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var stopwatch = new StopWatch();

            // Act
            stopwatch.StopOnReset = true;

            // Assert
            Assert.True(stopwatch.StopOnReset);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Start_WhenStarting_SetsRunningToTrue()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var expected = true;

            // Act
            stopWatch.Start();
            var actual = stopWatch.Running;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Stop_WhenStoping_SetsRunningToFalse()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var expected = false;

            // Act
            stopWatch.Stop();
            var actual = stopWatch.Running;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Reset_WithStopOnResetTrue_StopsAndResetsStopWatch()
        {
            // Arrange
            var stopWatch = new StopWatch();
            stopWatch.StopOnReset = true;
            stopWatch.Start();

            var gameTime = new GameTime();
            gameTime.AddTime(500);
            stopWatch.Update(gameTime);

            // Act
            stopWatch.Reset();

            // Assert
            Assert.Equal(0, stopWatch.ElapsedMS);
            Assert.False(stopWatch.Running);
        }

        [Fact]
        public void Reset_WithStopOnResetFalse_ResetsStopWatch()
        {
            // Arrange
            var stopWatch = new StopWatch();
            stopWatch.StopOnReset = false;
            stopWatch.Start();

            var gameTime = new GameTime();
            gameTime.AddTime(500);
            stopWatch.Update(gameTime);

            // Act
            stopWatch.Reset();

            // Assert
            Assert.Equal(0, stopWatch.ElapsedMS);
            Assert.True(stopWatch.Running);
        }

        [Fact]
        public void Restart_WhenInvoked_ResetsStopWatch()
        {
            // Arrange
            var stopWatch = new StopWatch();

            var gameTime = new GameTime();
            gameTime.AddTime(500);
            stopWatch.Update(gameTime);

            // Act
            stopWatch.Restart();

            // Assert
            Assert.True(stopWatch.Running);
            Assert.Equal(0, stopWatch.ElapsedMS);
            Assert.Equal(0, stopWatch.ElapsedSeconds);
        }

        [Fact]
        public void Update_WhenInvokingWithResetTypeAsAuto_ResetStopWatch()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var gameTime = new GameTime();
            gameTime.AddTime(1001);

            // Act
            stopWatch.Start();
            stopWatch.Update(gameTime);

            // Assert
            Assert.Equal(0, stopWatch.ElapsedMS);
            Assert.True(stopWatch.Running);
        }

        [Fact]
        public void Update_WhenInvoking_OnTimeElapsedEventInvoked()
        {
            // Arrange
            var expected = true;
            var actual = false;
            var stopWatch = new StopWatch();
            stopWatch.TimeOut = 2500;

            stopWatch.TimeElapsed += (sender, e) => actual = true;
            var gameTime = new GameTime();
            gameTime.AddTime(2500);

            // Act
            stopWatch.Start();
            stopWatch.Update(gameTime);

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

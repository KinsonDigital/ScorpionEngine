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
        public void Reset_WhenReseting_SetElapsedToZeroAndRunningToFalse()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var gameTime = new GameTime();
            gameTime.AddTime(500);
            var expectedRunning = false;
            var expectedElapsedMS = 0;

            // Act
            stopWatch.Start();
            stopWatch.Update(gameTime);
            stopWatch.Reset();
            var actualRunning = stopWatch.Running;
            var actualElapsedMS = stopWatch.ElapsedMS;

            // Assert
            Assert.Equal(expectedElapsedMS, actualElapsedMS);
            Assert.Equal(expectedRunning, actualRunning);
        }

        [Fact]
        public void Update_WhenInvokingWithResetTypeAsAuto_ResetStopWatch()
        {
            // Arrange
            var stopWatch = new StopWatch();
            var gameTime = new GameTime();
            gameTime.AddTime(1001);
            var expectedElapsedMS = 0;
            var expectedRunning = false;

            // Act
            stopWatch.Start();
            stopWatch.Update(gameTime);
            var actualElapsedMS = stopWatch.ElapsedMS;
            var actualRunning = stopWatch.Running;

            // Assert
            Assert.Equal(expectedElapsedMS, actualElapsedMS);
            Assert.Equal(expectedRunning, actualRunning);
        }

        [Fact]
        public void Update_WhenInvoking_OnTimeElapsedEventInvoked()
        {
            // Arrange
            var expected = true;
            var actual = false;
            var stopWatch = new StopWatch();
            stopWatch.TimeOut = 2500;

            stopWatch.OnTimeElapsed += (sender, e) => actual = true;
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

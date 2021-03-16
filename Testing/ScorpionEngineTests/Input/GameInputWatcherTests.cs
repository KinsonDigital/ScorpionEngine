// <copyright file="GameInputWatcherTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Input
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using KDScorpionEngine;
    using KDScorpionEngine.Utils;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor.Input;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="GameINputWatcher"/> class.
    /// </summary>
    public class GameInputWatcherTests
    {
        private readonly Mock<IStopWatch> mockButtonDownTimer;
        private readonly Mock<IStopWatch> mockButtonReleaseTimer;
        private readonly Mock<ICounter> mockCounter;

        public GameInputWatcherTests()
        {
            this.mockButtonDownTimer = new Mock<IStopWatch>();
            this.mockButtonReleaseTimer = new Mock<IStopWatch>();
            this.mockCounter = new Mock<ICounter>();
        }

        #region Prop Tests
        [Fact]
        public void Button_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var expected = KeyCode.Space;

            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Space;

            // Act
            var actual = watcher.Input;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentHitCountPercentage_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockCounter.SetupGet(p => p.Value).Returns(6);
            this.mockCounter.SetupGet(p => p.Max).Returns(10);
            var watcher = CreateWatcher();

            // Act
            watcher.Update(It.IsAny<GameTime>());

            // Assert
            Assert.Equal(60, watcher.CurrentHitCountPercentage);
        }

        [Fact]
        public void DownElapsedResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.DownElapsedResetMode = ResetType.Manual;

            var expected = ResetType.Manual;

            // Act
            var actual = watcher.DownElapsedResetMode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HitCountResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.HitCountResetMode = ResetType.Manual;

            var expected = ResetType.Manual;

            // Act
            var actual = watcher.HitCountResetMode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComboButtons_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.ComboInputs = new List<KeyCode>()
            {
                KeyCode.LeftControl,
                KeyCode.Space,
            };

            var expected = new List<KeyCode>()
            {
                KeyCode.LeftControl,
                KeyCode.Space,
            };

            // Act
            var actual = watcher.ComboInputs;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockButtonDownTimer.SetupGet(p => p.ElapsedMS).Returns(1234);

            var expected = 1234;
            var watcher = CreateWatcher();

            // Act
            var actual = watcher.InputDownElapsedMS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var expected = 1234f;
            this.mockButtonDownTimer.SetupGet(p => p.ElapsedSeconds).Returns(1234);

            var watcher = CreateWatcher();

            // Act
            var actual = watcher.InputDownElapsedSeconds;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockButtonReleaseTimer.SetupGet(p => p.ElapsedMS).Returns(1234);

            var expected = 1234;

            var watcher = CreateWatcher();

            // Act
            var actual = watcher.InputReleasedElapsedMS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockButtonReleaseTimer.SetupGet(p => p.ElapsedSeconds).Returns(1234);

            var expected = 1234;
            var watcher = CreateWatcher();

            // Act & Assert
            watcher.Update(new GameTime());
            var actual = watcher.InputReleasedElapsedSeconds;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockButtonDownTimer.SetupProperty(p => p.TimeOut);

            var expected = 1234;
            var watcher = CreateWatcher();

            // Act
            watcher.DownTimeOut = 1234;
            var actual = watcher.DownTimeOut;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockButtonReleaseTimer.SetupProperty(p => p.TimeOut);

            var expected = 1234;
            var watcher = CreateWatcher();

            // Act
            watcher.ReleaseTimeOut = 1234;
            var actual = watcher.ReleaseTimeOut;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReleasedElapsedResetMode_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.ReleasedElapsedResetMode = ResetType.Manual;

            var expected = ResetType.Manual;

            // Act
            var actual = watcher.ReleasedElapsedResetMode;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        /// <summary>
        /// Creats an instance of <see cref="MouseWatcher"/> for the purpose of testing.
        /// </summary>
        /// <returns>An instance to test.</returns>
        private FakeGameInputWatcher CreateWatcher()
            => new FakeGameInputWatcher(
                this.mockButtonDownTimer.Object,
                this.mockButtonReleaseTimer.Object,
                this.mockCounter.Object);
    }
}

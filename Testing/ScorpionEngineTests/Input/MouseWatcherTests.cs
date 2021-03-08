// <copyright file="MouseWatcherTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Input
{
    using System;
    using System.Collections.Generic;
    using KDScorpionEngine;
    using KDScorpionEngine.Input;
    using KDScorpionEngine.Utils;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor.Input;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="MouseWatcher"/> class.
    /// </summary>
    public class MouseWatcherTests
    {
        private readonly Mock<IGameInput<MouseButton, MouseState>> mockMouse;
        private readonly Mock<IStopWatch> mockButtonDownTimer;
        private readonly Mock<IStopWatch> mockButtonReleaseTimer;
        private readonly Mock<ICounter> mockCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseWatcherTests"/> class.
        /// </summary>
        public MouseWatcherTests()
        {
            this.mockMouse = new Mock<IGameInput<MouseButton, MouseState>>();
            this.mockButtonDownTimer = new Mock<IStopWatch>();
            this.mockButtonReleaseTimer = new Mock<IStopWatch>();
            this.mockCounter = new Mock<ICounter>();
        }

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            // Arrange
            var watcher = CreateWatcher();

            var expectedComboButtons = new List<MouseButton>();
            var expectedEnabled = true;

            // Act
            var actualEnabled = watcher.Enabled;
            var actualComboButtons = watcher.ComboButtons;

            // Assert
            Assert.Equal(expectedEnabled, actualEnabled);
            Assert.Equal(expectedComboButtons, actualComboButtons);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void Button_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Button = MouseButton.LeftButton;

            var expected = MouseButton.LeftButton;

            // Act
            var actual = watcher.Button;

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
            watcher.ComboButtons = new List<MouseButton>()
            {
                MouseButton.LeftButton,
                MouseButton.RightButton,
            };

            var expected = new List<MouseButton>()
            {
                MouseButton.LeftButton,
                MouseButton.RightButton,
            };

            // Act
            var actual = watcher.ComboButtons;

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

        #region Method Tests
        [Fact]
        public void Update_WithNullInputHitCountReachedEvent_DoesNotThrowNullReferenceExceptions()
        {
            // Arrange
            this.mockCounter.Setup(m => m.Count())
                .Callback(() => this.mockCounter.Raise(m => m.MaxReachedWhenIncrementing += null, EventArgs.Empty));

            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Button = MouseButton.LeftButton;

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() =>
            {
                watcher.Update(new GameTime());
            });
        }

        [Fact]
        public void Update_WithProperButtonState_ResetsDownTimerAndStartsButtonUpTimer()
        {
            // Arrange
            MockButton(MouseButton.LeftButton, true);
            var watcher = CreateWatcher();
            watcher.Button = MouseButton.LeftButton;

            // Act & Assert
            watcher.Update(new GameTime()); // Run once to get the internal states.

            MockButton(MouseButton.LeftButton, false);

            watcher.Update(new GameTime()); // Run update again to set the current and previous states different

            // Assert
            this.mockButtonDownTimer.Verify(m => m.Reset(), Times.Once());

            // The first time start is invoked is during creation of the watcher
            this.mockButtonReleaseTimer.Verify(m => m.Start(), Times.Exactly(2));
        }

        [Fact]
        public void Update_WhenDisabled_DoesNotGetMouseState()
        {
            // Arrange
            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Enabled = false;
            watcher.HitCountMax = 1;

            var gameTime = new GameTime();
            gameTime.AddTime(2000);

            // Act
            var actualCurrentHitCount = watcher.CurrentHitCount;

            // Assert
            this.mockMouse.Verify(m => m.GetState(), Times.Never());
        }

        [Fact]
        public void Update_WhenDisabled_DoesNotUpdateButtonDownTimer()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Enabled = false;

            var gameTime = new GameTime();
            gameTime.AddTime(2000);

            // Act
            watcher.Update(gameTime);

            // Assert
            this.mockButtonDownTimer.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Never());
        }

        [Fact]
        public void Update_WhenDisabled_DoesNotUpdateButtonReleaseTimer()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Enabled = false;

            var gameTime = new GameTime();
            gameTime.AddTime(2000);

            // Act
            watcher.Update(gameTime);

            // Assert
            this.mockButtonReleaseTimer.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Never());
        }

        [Fact]
        public void Update_WhenDisabled_DoesNotInvokeInputHitCountReachedEvent()
        {
            // Arrange
            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Enabled = false;
            watcher.HitCountMax = 1;

            // Act & Assert
            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.InputHitCountReached += e,
                e => watcher.InputHitCountReached -= e,
                () =>
                {
                    watcher.Update(new GameTime());

                    MockButton(MouseButton.LeftButton, false);
                    watcher.Update(new GameTime());

                    MockButton(MouseButton.LeftButton, true);
                    watcher.Update(new GameTime());

                    MockButton(MouseButton.LeftButton, false);
                    watcher.Update(new GameTime());
                });
        }

        // TODO: Need to make counter interface and inject
        [Fact]
        public void Update_WhenDisabled_DoesNotResetCounter()
        {
            // Arrange
            this.mockCounter.SetupGet(p => p.Max).Returns(1);
            this.mockCounter.SetupGet(p => p.Value).Returns(1);

            var watcher = CreateWatcher();
            watcher.Enabled = false;

            // Act
            MockButton(MouseButton.LeftButton, true);
            watcher.Update(new GameTime());

            MockButton(MouseButton.LeftButton, false);
            watcher.Update(new GameTime());

            // Assert
            this.mockCounter.Verify(m => m.Reset(), Times.Never());
        }

        [Fact]
        public void Update_WhenDisabled_DoesNotResetReleaseTimer()
        {
            // Arrange
            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Enabled = false;

            // Act
            MockButton(MouseButton.LeftButton, false);
            watcher.Update(new GameTime());

            // Assert
            this.mockButtonReleaseTimer.Verify(m => m.Reset(), Times.Never());
        }

        [Fact]
        public void Update_WhenDisabled_DoesNotResetButtonDownTimer()
        {
            // Arrange
            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Enabled = false;

            // Act
            MockButton(MouseButton.LeftButton, false);
            watcher.Update(new GameTime());

            MockButton(MouseButton.LeftButton, true);
            watcher.Update(new GameTime());

            // Assert
            this.mockButtonDownTimer.Verify(m => m.Reset(), Times.Never());
            this.mockButtonReleaseTimer.Verify(m => m.Start(), Times.Once());
        }

        [Fact]
        public void Update_WhenUpdating_InvokesHitCountReachedEvent()
        {
            // Arrange
            this.mockCounter.Setup(m => m.Count())
                .Callback(() => this.mockCounter.Raise(m => m.MaxReachedWhenIncrementing += null, EventArgs.Empty));

            var watcher = CreateWatcher();
            watcher.Button = MouseButton.LeftButton;

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputHitCountReached += e,
                e => watcher.InputHitCountReached -= e,
                () =>
                {
                    MockButton(MouseButton.LeftButton, true);
                    watcher.Update(It.IsAny<GameTime>());

                    watcher.Update(It.IsAny<GameTime>());
                });
        }

        [Fact]
        public void Update_WhenInvoked_UpdatesCounter()
        {
            // Arrange
            this.mockCounter.SetupGet(p => p.Value).Returns(0);
            this.mockCounter.SetupGet(p => p.Max).Returns(10);

            var watcher = CreateWatcher();

            // Act
            MockButton(MouseButton.LeftButton, true);
            watcher.Update(It.IsAny<GameTime>());

            MockButton(MouseButton.LeftButton, false);
            watcher.Update(It.IsAny<GameTime>());

            // Assert
            this.mockCounter.Verify(m => m.Count(), Times.Once());
        }

        [Fact]
        public void Update_WhenUpdating_InvokesInputDownTimeoutEvent()
        {
            // Arrange
            this.mockButtonDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) => this.mockButtonDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty));

            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Button = MouseButton.LeftButton;

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputDownTimedOut += e,
                e => watcher.InputDownTimedOut -= e,
                () => watcher.Update(It.IsAny<GameTime>()));
        }

        [Fact]
        public void Update_WithNullInputDownTimeoutEvent_ShouldNotThrowNullRefException()
        {
            // Arrange
            MockButton(MouseButton.LeftButton, true);
            var watcher = new MouseWatcher(
                true,
                this.mockMouse.Object,
                new FakeStopWatch(),
                this.mockButtonReleaseTimer.Object,
                this.mockCounter.Object);

            watcher.Button = MouseButton.LeftButton;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() =>
            {
                watcher.Update(gameTime);
                watcher.Update(gameTime);
            });
        }

        [Fact]
        public void Update_WhenUpdating_InvokesInputReleaseTimeoutEvent()
        {
            // Arrange
            var watcher = new MouseWatcher(
                true,
                this.mockMouse.Object,
                this.mockButtonReleaseTimer.Object,
                new FakeStopWatch(),
                this.mockCounter.Object);

            watcher.Button = MouseButton.RightButton;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputReleaseTimedOut += e,
                e => watcher.InputReleaseTimedOut -= e,
                () =>
                {
                    MockButton(MouseButton.RightButton, true);
                    watcher.Update(gameTime);

                    MockButton(MouseButton.RightButton, false);
                    watcher.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WithNullInputReleasedTimeoutEvent_ShouldNotThrowNullRefException()
        {
            // Arrange
            var watcher = new MouseWatcher(true,
                this.mockMouse.Object,
                this.mockButtonReleaseTimer.Object,
                new FakeStopWatch(),
                this.mockCounter.Object);

            watcher.Button = MouseButton.RightButton;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() =>
            {
                MockButton(MouseButton.RightButton, true);
                watcher.Update(gameTime);

                MockButton(MouseButton.RightButton, false);
                watcher.Update(gameTime);
            });
        }

        [Fact]
        public void Update_WithSetComboButtons_InvokeInputComboPressedEvent()
        {
            // Arrange
            MockButtons(new[] { MouseButton.LeftButton, MouseButton.RightButton }, true);

            var watcher = CreateWatcher();
            watcher.ComboButtons = new List<MouseButton>()
            {
                MouseButton.LeftButton,
                MouseButton.RightButton,
            };

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputComboPressed += e,
                e => watcher.InputComboPressed -= e,
                () =>
                {
                    watcher.Update(new GameTime());
                });
        }

        [Fact]
        public void Update_WithNullInputComboPressedEvent_DoesNotThrowException()
        {
            // Arrange
            MockButtons(new[] { MouseButton.LeftButton, MouseButton.RightButton }, true);

            var watcher = CreateWatcher();
            watcher.ComboButtons = new List<MouseButton>()
            {
                MouseButton.LeftButton,
                MouseButton.RightButton,
            };

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() =>
            {
                watcher.Update(new GameTime());
            });
        }
        #endregion

        /// <summary>
        /// Sets up the mock for the given <paramref name="button"/> to the given <paramref name="state"/>.
        /// </summary>
        /// <param name="button">The button to mock.</param>
        /// <param name="state">The state of the button to mock.</param>
        private void MockButton(MouseButton button, bool state)
        {
            this.mockMouse.Setup(m => m.GetState())
                .Returns(() =>
                {
                    var keyState = new MouseState();
                    keyState.SetButtonState(button, state);
                    return keyState;
                });
        }

        /// <summary>
        /// Sets up the mock for the given <paramref name="buttons"/> to the given <paramref name="state"/>.
        /// </summary>
        /// <param name="buttons">The buttons to mock.</param>
        /// <param name="state">The state of the button to mock.</param>
        private void MockButtons(MouseButton[] buttons, bool state)
        {
            this.mockMouse.Setup(m => m.GetState())
                .Returns(() =>
                {
                    var keyState = new MouseState();

                    foreach (var key in buttons)
                    {
                        keyState.SetButtonState(key, state);
                    }

                    return keyState;
                });
        }

        /// <summary>
        /// Creats an instance of <see cref="MouseWatcher"/> for the purpose of testing.
        /// </summary>
        /// <returns>An instance to test.</returns>
        private MouseWatcher CreateWatcher()
            => new MouseWatcher(
                true,
                this.mockMouse.Object,
                this.mockButtonDownTimer.Object,
                this.mockButtonReleaseTimer.Object,
                this.mockCounter.Object);
    }
}

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
            var actualComboButtons = watcher.ComboInputs;

            // Assert
            Assert.Equal(expectedEnabled, actualEnabled);
            Assert.Equal(expectedComboButtons, actualComboButtons);
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
            watcher.Input = MouseButton.LeftButton;

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
            watcher.Input = MouseButton.LeftButton;

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
            watcher.Input = MouseButton.LeftButton;

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
                .Callback<GameTime>((_) => this.mockButtonDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty));

            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Input = MouseButton.LeftButton;

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
            this.mockButtonDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((_) => this.mockButtonDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty));

            MockButton(MouseButton.LeftButton, true);
            var watcher = new MouseWatcher(
                this.mockMouse.Object,
                this.mockButtonDownTimer.Object,
                this.mockButtonReleaseTimer.Object,
                this.mockCounter.Object);

            watcher.Input = MouseButton.LeftButton;

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
            this.mockButtonReleaseTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((_) => this.mockButtonReleaseTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty));

            var watcher = new MouseWatcher(
                this.mockMouse.Object,
                this.mockButtonDownTimer.Object,
                this.mockButtonReleaseTimer.Object,
                this.mockCounter.Object);

            watcher.Input = MouseButton.RightButton;

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
            var watcher = new MouseWatcher(this.mockMouse.Object,
                                           this.mockButtonReleaseTimer.Object,
                                           new Mock<IStopWatch>().Object,
                                           this.mockCounter.Object);

            watcher.Input = MouseButton.RightButton;

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
            watcher.ComboInputs = new List<MouseButton>()
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
            watcher.ComboInputs = new List<MouseButton>()
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

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesInputHitCountReachedEvent()
        {
            // Arrange
            this.mockCounter.Setup(m => m.Count())
                .Callback(() => this.mockCounter.Raise(m => m.MaxReachedWhenIncrementing += null, EventArgs.Empty));

            MockButton(MouseButton.LeftButton, true);

            var watcher = CreateWatcher();
            watcher.Input = MouseButton.LeftButton;
            watcher.HitCountMax = 1;

            // Act & Assert
            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.InputHitCountReached += e,
                e => watcher.InputHitCountReached -= e,
                () =>
                {
                    watcher.Dispose();
                    watcher.Update(It.IsAny<GameTime>());
                    MockButton(MouseButton.LeftButton, false);
                    watcher.Update(It.IsAny<GameTime>());
                });
        }

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesInputDownTimeOutEvent()
        {
            // Arrange
            MockButton(MouseButton.LeftButton, true);
            this.mockButtonDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockButtonDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();
            watcher.Input = MouseButton.LeftButton;

            watcher.InputDownTimedOut += (sender, e)
                => Assert.True(false, $"The '{nameof(KeyboardWatcher.InputDownTimedOut)}' was not unsubscribed and was invoked.");

            // Act
            watcher.Dispose();
            watcher.Update(It.IsAny<GameTime>());
        }

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesInputReleaseTimeOutEvent()
        {
            // Arrange
            this.mockButtonReleaseTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockButtonReleaseTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();
            watcher.Input = MouseButton.LeftButton;

            watcher.InputReleaseTimedOut += (sender, e)
                => Assert.True(false, $"The '{nameof(KeyboardWatcher.InputReleaseTimedOut)}' was not unsubscribed and was invoked.");

            // Act
            watcher.Dispose();

            MockButton(MouseButton.LeftButton, true);
            watcher.Update(It.IsAny<GameTime>());

            MockButton(MouseButton.LeftButton, false);
            watcher.Update(It.IsAny<GameTime>());
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
            => new MouseWatcher(this.mockMouse.Object,
                                this.mockButtonDownTimer.Object,
                                this.mockButtonReleaseTimer.Object,
                                this.mockCounter.Object);
    }
}

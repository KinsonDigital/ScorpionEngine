// <copyright file="KeyboardWatcherTests.cs" company="KinsonDigital">
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
    /// Unit tests to test the <see cref="KeyboardWatcher"/> class.
    /// </summary>
    public class KeyboardWatcherTests
    {
        private readonly Mock<IGameInput<KeyCode, KeyboardState>> mockKeyboard;
        private readonly Mock<IStopWatch> mockKeyDownTimer;
        private readonly Mock<IStopWatch> mockKeyReleaseTimer;
        private readonly Mock<ICounter> mockCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardWatcherTests"/> class.
        /// </summary>
        public KeyboardWatcherTests()
        {
            this.mockKeyboard = new Mock<IGameInput<KeyCode, KeyboardState>>();
            this.mockKeyboard.Name = nameof(this.mockKeyboard);

            this.mockKeyDownTimer = new Mock<IStopWatch>();
            this.mockKeyDownTimer.Name = nameof(this.mockKeyDownTimer);

            this.mockKeyReleaseTimer = new Mock<IStopWatch>();
            this.mockKeyReleaseTimer.Name = nameof(this.mockKeyReleaseTimer);

            this.mockCounter = new Mock<ICounter>();
            this.mockCounter.Name = nameof(this.mockCounter);
        }

        #region Method Tests
        [Fact]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            // Arrange
            var watcher = CreateWatcher();
            var expectedComboButtons = new List<KeyCode>();
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
        public void Update_WhenInvokingWithProperButtonState_ResetsKeyDownTimerAndStartsKeyReleaseTimer()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;

            // Act & Assert
            MockKey(KeyCode.Left, true);
            watcher.Update(It.IsAny<GameTime>()); // Run once to get the internal states.

            MockKey(KeyCode.Left, false);
            watcher.Update(It.IsAny<GameTime>()); // Run update again to set the current and previous states different

            var actualDownTimerElapsed = watcher.InputDownElapsedMS;
            var actualUpTimerElapsed = watcher.InputReleasedElapsedMS;

            // Assert
            this.mockKeyDownTimer.Verify(m => m.Reset(), Times.Once());

            // NOTE: This is expected to be invoked 2 times because upon
            // creation of the watcher, the timers are started
            this.mockKeyReleaseTimer.Verify(m => m.Start(), Times.Exactly(2));
        }

        [Fact]
        public void Update_WhenDisabled_DoNotUpdateAnything()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Enabled = false;
            watcher.Input = KeyCode.Left;
            watcher.HitCountMax = 1;

            var beforeStates = new List<KeyValuePair<KeyCode, bool>>
            {
                KeyValuePair.Create(KeyCode.Left, true),
                KeyValuePair.Create(KeyCode.LeftControl, true),
                KeyValuePair.Create(KeyCode.Space, true)
            };

            MockKeys(beforeStates.ToArray());

            //MockKeys(new[] { KeyCode.Left, KeyCode.LeftControl, KeyCode.Space }, true);
            watcher.ComboInputs = new List<KeyCode>(new[] { KeyCode.LeftControl, KeyCode.Space });

            // Act
            watcher.Update(It.IsAny<GameTime>());

            var afterStates = new List<KeyValuePair<KeyCode, bool>>
            {
                KeyValuePair.Create(KeyCode.Left, false),
                KeyValuePair.Create(KeyCode.LeftControl, true),
                KeyValuePair.Create(KeyCode.Space, true)
            };

            MockKeys(afterStates.ToArray());
            watcher.Update(It.IsAny<GameTime>());

            // Assert
            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.InputComboPressed += e,
                e => watcher.InputComboPressed -= e,
                () =>
                {
                    watcher.Update(It.IsAny<GameTime>());
                });

            this.mockKeyboard.Verify(m => m.GetState(), Times.Never());
            this.mockKeyDownTimer.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Never());
            this.mockKeyReleaseTimer.Verify(m => m.Update(It.IsAny<GameTime>()), Times.Never());
            this.mockCounter.Verify(m => m.Count(), Times.Never());
            this.mockKeyDownTimer.Verify(m => m.Reset(), Times.Never());
            this.mockKeyReleaseTimer.Verify(m => m.Start(), Times.Exactly(1));
        }

        [Fact]
        public void Update_WhenUpdating_InvokesInputHitCountReachedEvent()
        {
            // Arrange
            this.mockCounter.SetupGet(p => p.Max).Returns(1);
            this.mockCounter.Setup(m => m.Count())
                .Callback(() => this.mockCounter.Raise(m => m.MaxReachedWhenIncrementing += null, EventArgs.Empty));
            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputHitCountReached += e,
                e => watcher.InputHitCountReached -= e,
                () =>
                {
                    MockKey(KeyCode.Left, true);
                    watcher.Update(It.IsAny<GameTime>());

                    MockKey(KeyCode.Left, false);
                    watcher.Update(It.IsAny<GameTime>());
                });
        }

        [Fact]
        public void Update_WithNullInputHitCountReachedEvent_DoesNotThrowException()
        {
            // Arrange
            this.mockCounter.SetupGet(p => p.Max).Returns(1);
            this.mockCounter.Setup(m => m.Count())
                .Callback(() => this.mockCounter.Raise(m => m.MaxReachedWhenIncrementing += null, EventArgs.Empty));
            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() =>
            {
                MockKey(KeyCode.Left, true);
                watcher.Update(It.IsAny<GameTime>());

                MockKey(KeyCode.Left, false);
                watcher.Update(It.IsAny<GameTime>());
            });
        }

        [Fact]
        public void Update_WhenKeyDownTimeOutIsReached_KeyDownTimerIsReset()
        {
            // Arrange
            MockKey(KeyCode.Space, true);
            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Space;

            this.mockKeyDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var gameTime = new GameTime();
            gameTime.AddTime(500);

            // Act & Assert
            watcher.Update(gameTime);

            this.mockKeyDownTimer.Verify(m => m.Reset(), Times.Once());
        }

        [Fact]
        public void Update_WhenKeyDownTimeOutIsReachedWithResetSetToManual_KeyDownTimerIsNotReset()
        {
            // Arrange
            MockKey(KeyCode.Space, true);
            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Space;

            this.mockKeyDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var gameTime = new GameTime();
            gameTime.AddTime(500);

            // Act & Assert
            watcher.Update(gameTime);

            this.mockKeyDownTimer.Verify(m => m.Reset(), Times.Once());
        }

        [Fact]
        public void Update_WhenKeyIsDownPosition_InputDownEventInvoked()
        {
            // Arrange
            MockKey(KeyCode.Space, true);
            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Space;

            var gameTime = new GameTime();
            gameTime.AddTime(500);

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputDown += e,
                e => watcher.InputDown -= e,
                () =>
                {
                    watcher.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WhenKeyIsReleased_InputPressedEventInvoked()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Space;

            var gameTime = new GameTime();
            gameTime.AddTime(500);

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputReleased += e,
                e => watcher.InputReleased -= e,
                () =>
                {
                    MockKey(KeyCode.Space, true);
                    watcher.Update(gameTime);

                    MockKey(KeyCode.Space, false);
                    watcher.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WhenKeyDownTimoutIsExpired_InvokesInputDownTimedOutEvent()
        {
            // Arrange
            MockKey(KeyCode.Left, true);
            this.mockKeyDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputDownTimedOut += e,
                e => watcher.InputDownTimedOut -= e,
                () =>
                {
                    watcher.Update(It.IsAny<GameTime>());
                });
        }

        [Fact]
        public void Update_WithNullInputDownTimedOutEvent_DoesNotThrowNullRefException()
        {
            // Arrange
            MockKey(KeyCode.Left, true);
            this.mockKeyDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() => watcher.Update(gameTime));
        }

        [Fact]
        public void Update_WhenDownKeyTimesOut_InvokesInputReleaseTimedOutEvent()
        {
            // Arrange
            this.mockKeyReleaseTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyReleaseTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputReleaseTimedOut += e,
                e => watcher.InputReleaseTimedOut -= e,
                () =>
                {
                    watcher.Update(It.IsAny<GameTime>());
                });
        }

        [Fact]
        public void Update_WhenKeyDownTimeOutIsReached_KeyReleaseTimerIsReset()
        {
            // Arrange
            this.mockKeyReleaseTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyReleaseTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();

            // Act
            watcher.Update(It.IsAny<GameTime>());

            // Assert
            this.mockKeyReleaseTimer.Verify(m => m.Reset(), Times.Exactly(1));
        }

        [Fact]
        public void Update_WhenKeyReleaseTimeOutIsReachedWithResetSetToManual_KeyReleaseTimerIsNotReset()
        {
            // Arrange
            this.mockKeyReleaseTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyReleaseTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();
            watcher.DownElapsedResetMode = ResetType.Manual;

            // Act
            watcher.Update(It.IsAny<GameTime>());

            // Assert
            this.mockKeyReleaseTimer.Verify(m => m.Reset(), Times.Never());
        }

        [Fact]
        public void Update_WithNullInputReleaseTimeOutEvent_ShouldNotThrowNullRefException()
        {
            // Arrange
            this.mockKeyReleaseTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyReleaseTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() => watcher.Update(It.IsAny<GameTime>()));
        }

        [Fact]
        public void Update_WhenAllComboKeysArePressed_InvokesInputComboPressedEvent()
        {
            // Arrange
            MockKeys(new[] { KeyCode.LeftControl, KeyCode.Right }, true);

            var watcher = CreateWatcher();
            watcher.ComboInputs = new List<KeyCode>()
            {
                KeyCode.LeftControl,
                KeyCode.Right,
            };

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.InputComboPressed += e,
                e => watcher.InputComboPressed -= e,
                () =>
                {
                    watcher.Update(It.IsAny<GameTime>());
                });
        }

        [Fact]
        public void Update_WhenInvokingWithNullInputComboPressedEvent_DoesNotThrowNullException()
        {
            // Arrange
            MockKeys(new[] { KeyCode.Left, KeyCode.Right }, true);

            var watcher = CreateWatcher();
            watcher.ComboInputs = new List<KeyCode>()
            {
                KeyCode.Left,
                KeyCode.Right,
            };

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() => watcher.Update(It.IsAny<GameTime>()));
        }

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesInputHitCountReachedEvent()
        {
            // Arrange
            this.mockCounter.Setup(m => m.Count())
                .Callback(() => this.mockCounter.Raise(m => m.MaxReachedWhenIncrementing += null, EventArgs.Empty));

            MockKey(KeyCode.Left, true);

            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;
            watcher.HitCountMax = 1;

            // Act & Assert
            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.InputHitCountReached += e,
                e => watcher.InputHitCountReached -= e,
                () =>
                {
                    watcher.Dispose();
                    watcher.Update(It.IsAny<GameTime>());
                    MockKey(KeyCode.Left, false);
                    watcher.Update(It.IsAny<GameTime>());
                });
        }

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesInputDownTimeOutEvent()
        {
            // Arrange
            MockKey(KeyCode.Left, true);
            this.mockKeyDownTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyDownTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;

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
            this.mockKeyReleaseTimer.Setup(m => m.Update(It.IsAny<GameTime>()))
                .Callback<GameTime>((gameTime) =>
                {
                    this.mockKeyReleaseTimer.Raise(m => m.TimeElapsed += null, EventArgs.Empty);
                });

            var watcher = CreateWatcher();
            watcher.Input = KeyCode.Left;

            watcher.InputReleaseTimedOut += (sender, e)
                => Assert.True(false, $"The '{nameof(KeyboardWatcher.InputReleaseTimedOut)}' was not unsubscribed and was invoked.");

            // Act
            watcher.Dispose();

            MockKey(KeyCode.Left, true);
            watcher.Update(It.IsAny<GameTime>());

            MockKey(KeyCode.Left, false);
            watcher.Update(It.IsAny<GameTime>());
        }
        #endregion

        /// <summary>
        /// Sets up the mock for the given <paramref name="key"/> to the given <paramref name="state"/>.
        /// </summary>
        /// <param name="key">The key to mock.</param>
        /// <param name="state">The state of the key to mock.</param>
        private void MockKey(KeyCode key, bool state)
        {
            this.mockKeyboard.Setup(m => m.GetState())
                .Returns(() =>
                {
                    var keyState = new KeyboardState();
                    keyState.SetKeyState(key, state);

                    return keyState;
                });
        }

        /// <summary>
        /// Sets up the mock for the given <paramref name="keys"/> to the given <paramref name="state"/>.
        /// </summary>
        /// <param name="keys">The keys to mock.</param>
        /// <param name="state">The state of the key to mock.</param>
        private void MockKeys(KeyCode[] keys, bool state)
        {
            this.mockKeyboard.Setup(m => m.GetState())
                .Returns(() =>
                {
                    var keyState = new KeyboardState();

                    foreach (var key in keys)
                    {
                        keyState.SetKeyState(key, state);
                    }

                    return keyState;
                });
        }

        /// <summary>
        /// Sets up the mock for the given <paramref name="keyStates"/>.
        /// </summary>
        /// <param name="keyStates">The keyboard keys and associated states.</param>
        private void MockKeys(KeyValuePair<KeyCode, bool>[] keyStates)
        {
            this.mockKeyboard.Setup(m => m.GetState())
                .Returns(() =>
                {
                    var keyState = new KeyboardState();

                    foreach (var state in keyStates)
                    {
                        keyState.SetKeyState(state.Key, state.Value);
                    }

                    return keyState;
                });
        }

        /// <summary>
        /// Creates a new instance of <see cref="KeyboardWatcher"/> for the purpose of testing.
        /// </summary>
        /// <returns>The instance to test.</returns>
        private KeyboardWatcher CreateWatcher()
            => new KeyboardWatcher(this.mockKeyboard.Object,
                                   this.mockKeyDownTimer.Object,
                                   this.mockKeyReleaseTimer.Object,
                                   this.mockCounter.Object);
    }
}

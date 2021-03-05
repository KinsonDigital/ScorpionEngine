// <copyright file="KeyboardWatcherTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Input
{
    using System;
    using System.Collections.Generic;
    using KDScorpionEngine;
    using KDScorpionEngine.Input;
    using Moq;
    using Raptor.Input;
    using Xunit;

    //TODO: DO NOT DELETE THIS!! This will be reworked once the new keyboard implementation for the Raptor keyboard are finished
    // Refer to cards below for more info
    // 1. https://dev.azure.com/KinsonDigital/GameDevTools/_workitems/edit/2424
    // 2. https://dev.azure.com/KinsonDigital/GameDevTools/_workitems/edit/2425

    /// <summary>
    /// Unit tests to test the <see cref="KeyboardWatcher"/> class.
    /// </summary>
    public class KeyboardWatcherTests // : IDisposable
    {
        private readonly Mock<IGameInput<KeyCode, KeyboardState>> mockKeyboard;

        public KeyboardWatcherTests() => this.mockKeyboard = new Mock<IKeyboard>();
            this.mockKeyboard = new Mock<IGameInput<KeyCode, KeyboardState>>();

        #region Method Tests
        [Fact]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            // Arrange
            var watcher = new KeyboardWatcher(true, this.mockKeyboard.Object);
            var expectedComboButtons = new List<KeyCode>();
            var expectedEnabled = true;

            // Act
            var actualEnabled = watcher.Enabled;
            var actualComboButtons = watcher.ComboKeys;

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
            watcher.Key = KeyCode.Left;
            var expected = KeyCode.Left;

            // Act
            var actual = watcher.Key;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentHitCountPercentage_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            MockKey(KeyCode.Left, true);

            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.HitCountMax = 10;

            var gameTime = new GameTime();

            var expected = 60;

            // Act
            for (var i = 0; i < 12; i++)
            {
                MockKey(KeyCode.Left, i % 2 == 0);

                watcher.Update(gameTime);
            }

            var actual = watcher.CurrentHitCountPercentage;

            // Assert
            Assert.Equal(expected, actual);
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
        public void ComboKeys_WhenSettingWithNonNullValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.ComboKeys = new List<KeyCode>()
            {
                KeyCode.Left,
                KeyCode.Right,
            };

            var expected = new List<KeyCode>()
            {
                KeyCode.Left,
                KeyCode.Right,
            };

            // Act
            var actual = watcher.ComboKeys;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            var expected = 16;
            var gameTime = new GameTime();
            gameTime.AddTime(16);

            // Act
            watcher.Update(gameTime);
            var actual = watcher.InputDownElapsedMS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.InputReleasedTimeout = 2000;
            watcher.InputDownTimeOut = 3000;

            var expected = 1.234f;

            var gameTime = new GameTime();
            gameTime.AddTime(1234);

            // Act
            watcher.Update(gameTime);
            var actual = watcher.InputDownElapsedSeconds;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            var expected = 31;
            var gameTime = new GameTime();
            gameTime.AddTime(31);

            // Act
            watcher.Update(gameTime);
            var actual = watcher.InputReleasedElapsedMS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            MockKey(KeyCode.W, false);

            var watcher = CreateWatcher();
            watcher.Key = KeyCode.W;
            watcher.HitCountMax = 0;
            watcher.InputReleasedTimeout = 5000;

            var gameTime = new GameTime();
            gameTime.AddTime(4321);

            var expected = 4.321;

            // Act/Assert
            watcher.Update(gameTime); // Run once to get the internal states.

            // watcher.Update(gameTime);
            var actual = Math.Round(watcher.InputReleasedElapsedSeconds, 3);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.InputDownTimeOut = 123;

            var expected = 123;

            // Act
            var actual = watcher.InputDownTimeOut;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.InputReleasedTimeout = 456;

            var expected = 456;

            // Act
            var actual = watcher.InputReleasedTimeout;

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
        public void Update_WhenInvokingWithNullOnInputHitCountReachedEvent_RunsWithNoNullReferenceExceptions()
        {
            // Arrange
            MockKey(KeyCode.W, false);

            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.HitCountMax = 0;

            // Act/Assert
            AssertHelpers.DoesNotThrowNullReference(() =>
            {
                watcher.Update(new GameTime());
            });
        }

        [Fact]
        public void Update_WhenInvokingWhileCountingWithNullCounter_RunsWithNoNullReferenceExceptions()
        {
            // Arrange
            MockKey(KeyCode.Left, true);

            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.HitCountMax = 0;

            watcher.SetFieldNull("counter");

            // Act/Assert
            AssertHelpers.DoesNotThrowNullReference(() =>
            {
                watcher.Update(new GameTime());
            });
        }

        [Fact]
        public void Update_WhenInvokingWithProperButtonState_ResetsDownTimerAndStartsButtonUpTimer()
        {
            // Arrange
            MockKey(KeyCode.Left, true);

            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.HitCountMax = 0;

            var gameTime = new GameTime();
            gameTime.AddTime(10);

            var expectedDownTimerElapsed = 0;
            var expectedUpTimerElapsed = 10;

            // Act/Assert
            watcher.Update(gameTime); // Run once to get the internal states.
            MockKey(KeyCode.Left, false);
            watcher.Update(gameTime); // Run update again to set the current and previous states different
            watcher.Update(gameTime);
            var actualDownTimerElapsed = watcher.InputDownElapsedMS;
            var actualUpTimerElapsed = watcher.InputReleasedElapsedMS;

            // Assert
            Assert.Equal(expectedDownTimerElapsed, actualDownTimerElapsed);
            Assert.Equal(expectedUpTimerElapsed, actualUpTimerElapsed);
        }

        [Fact]
        public void Update_WhenDisabled_DoNotUpdateAnything()
        {
            // Arrange
            var expectedCurrentHitCount = 0;
            var watcher = CreateWatcher();
            watcher.Enabled = false;
            watcher.Key = KeyCode.Left;
            watcher.HitCountMax = 1;

            var gameTime = new GameTime();
            gameTime.AddTime(2000);

            // Act
            watcher.Update(gameTime);
            watcher.Update(gameTime);
            var actualCurrentHitCount = watcher.CurrentHitCount;

            // Assert
            Assert.Equal(expectedCurrentHitCount, actualCurrentHitCount);

            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.OnInputHitCountReached += e,
                e => watcher.OnInputHitCountReached -= e,
                () =>
                {
                    watcher.Update(gameTime);
                });

            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.OnInputDownTimeOut += e,
                e => watcher.OnInputDownTimeOut -= e,
                () =>
                {
                    watcher.Update(gameTime);
                });

            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.OnInputReleasedTimeOut += e,
                e => watcher.OnInputReleasedTimeOut -= e,
                () =>
                {
                    watcher.Update(gameTime);
                });

            AssertHelpers.DoesNotRaise<EventArgs>(
                e => watcher.OnInputComboPressed += e,
                e => watcher.OnInputComboPressed -= e,
                () =>
                {
                    watcher.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WhenUpdating_InvokesOnInputHitCountReachedEvent()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.HitCountMax = 2;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.OnInputHitCountReached += e,
                e => watcher.OnInputHitCountReached -= e,
                () =>
                {
                    for (var i = 0; i < 4; i++)
                    {
                        MockKey(KeyCode.Left, i % 2 == 0);
                        watcher.Update(gameTime);
                    }
                });
        }

        [Fact]
        public void Update_WhenUpdating_InvokesOnInputDownTimeOutEvent()
        {
            // Arrange
            MockKey(KeyCode.Left, true);
            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.InputDownTimeOut = 1000;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Ace & Assert
            Assert.Raises<EventArgs>(
                e => watcher.OnInputDownTimeOut += e,
                e => watcher.OnInputDownTimeOut -= e,
                () =>
                {
                    watcher.Update(gameTime);
                    watcher.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WhenUpdatingWithNullOnInputDownTimeOutEvent_ShouldNotThrowNullRefException()
        {
            // Arrange
            MockKey(KeyCode.Left, true);

            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act/Assert
            AssertHelpers.DoesNotThrow<NullReferenceException>(() =>
            {
                watcher.Update(gameTime);
                watcher.Update(gameTime);
            });
        }

        [Fact]
        public void Update_WhenUpdating_InvokesOnInputReleasedTimeoutEvent()
        {
            // Arrange
            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Right;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.OnInputReleasedTimeOut += e,
                e => watcher.OnInputReleasedTimeOut -= e,
                () =>
                {
                    watcher.Update(gameTime);
                    watcher.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WhenUpdatingWithNullInputReleasedTimeOutEvent_ShouldNotThrowNullRefException()
        {
            // Arrange
            MockKey(KeyCode.Right, false);

            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Right;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act/Assert
            AssertHelpers.DoesNotThrow<NullReferenceException>(() =>
            {
                watcher.Update(gameTime);
                watcher.Update(gameTime);
            });
        }

        [Fact]
        public void Update_WhenAllComboKeysArePressed_InvokesOnInputComboPressedEvent()
        {
            // Arrange
            MockKeys(new[] { KeyCode.LeftControl, KeyCode.Right }, true);

            var watcher = CreateWatcher();
            watcher.ComboKeys = new List<KeyCode>()
            {
                KeyCode.LeftControl,
                KeyCode.Right,
            };

            // Act & Assert
            Assert.Raises<EventArgs>(
                e => watcher.OnInputComboPressed += e,
                e => watcher.OnInputComboPressed -= e,
                () =>
                {
                    watcher.Update(new GameTime());
                });
        }

        [Fact]
        public void Update_WhenInvokingWithNullOnInputComboPressedEvent_DoesNotThrowNullException()
        {
            // Arrange
            MockKeys(new[] { KeyCode.Left, KeyCode.Right }, true);

            var watcher = CreateWatcher();
            watcher.ComboKeys = new List<KeyCode>()
            {
                KeyCode.Left,
                KeyCode.Right,
            };
            var gameTime = new GameTime();
            gameTime.AddTime(16);

            // Act & Assert
            AssertHelpers.DoesNotThrowNullReference(() => watcher.Update(gameTime));
        }

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesOnInputHitCountReachedEvent()
        {
            // Arrange
            MockKey(KeyCode.Left, true);
            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.HitCountMax = 1;
            var expectedOnInputHitCountReachedInvoked = false;
            watcher.OnInputHitCountReached += (sender, e) => expectedOnInputHitCountReachedInvoked = true;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act
            watcher.Dispose();
            watcher.Update(gameTime);
            MockKey(KeyCode.Left, false);
            watcher.Update(gameTime);

            // Assert
            Assert.False(expectedOnInputHitCountReachedInvoked);
        }

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesOnInputDownTimeOutEvent()
        {
            // Arrange
            MockKey(KeyCode.Left, true);
            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.InputDownTimeOut = 500;
            var expectedOnInputDownTimeOutInvoked = false;
            watcher.OnInputDownTimeOut += (sender, e) => expectedOnInputDownTimeOutInvoked = true;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act
            watcher.Dispose();
            watcher.Update(gameTime);
            watcher.Update(gameTime);

            // Assert
            Assert.False(expectedOnInputDownTimeOutInvoked);
        }

        [Fact]
        public void Dispose_WhenInvoked_UnsubscribesOnInputReleasedTimeOutEvent()
        {
            // Arrange
            MockKey(KeyCode.Left, false);
            var watcher = CreateWatcher();
            watcher.Key = KeyCode.Left;
            watcher.InputReleasedTimeout = 400;
            var expectedOnInputReleasedTimeOutInvoked = false;
            watcher.OnInputReleasedTimeOut += (sender, e) => expectedOnInputReleasedTimeOutInvoked = true;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act
            watcher.Dispose();
            watcher.Update(gameTime);

            // Assert
            Assert.False(expectedOnInputReleasedTimeOutInvoked);
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
        /// Creates a new instance of <see cref="KeyboardWatcher"/> for the purpose of testing.
        /// </summary>
        /// <returns>The instance to test.</returns>
        private KeyboardWatcher CreateWatcher() => new KeyboardWatcher(true, this.mockKeyboard.Object);
    }
}

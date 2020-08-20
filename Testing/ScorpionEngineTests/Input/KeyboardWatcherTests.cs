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
    using Raptor;
    using Raptor.Input;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KeyboardWatcher"/> class.
    /// </summary>
    public class KeyboardWatcherTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> mockKeyboard;
        #endregion

        #region Constructors
        public KeyboardWatcherTests() => this.mockKeyboard = new Mock<IKeyboard>();
        #endregion

        #region Method Tests
        [Fact]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object);
            var expectedComboButtons = new List<KeyCode>();
            var expectedEnabled = true;

            // Act
            var actualEnabled = keyboardWatcher.Enabled;
            var actualComboButtons = keyboardWatcher.ComboKeys;

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
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
            };
            var expected = KeyCode.Left;

            // Act
            var actual = keyboardWatcher.Key;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CurrentHitCountPercentage_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Left)).Returns(true);
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
                HitCountMax = 10,
            };

            // This value is meaningless and is only for satisfy the update param
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 0) };

            var expected = 60;

            // Act
            for (var i = 0; i < 6; i++)
            {
                keyboardWatcher.Update(engineTime);
            }

            var actual = keyboardWatcher.CurrentHitCountPercentage;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DownElapsedResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                DownElapsedResetMode = ResetType.Manual,
            };

            var expected = ResetType.Manual;

            // Act
            var actual = keyboardWatcher.DownElapsedResetMode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HitCountResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                HitCountResetMode = ResetType.Manual,
            };

            var expected = ResetType.Manual;

            // Act
            var actual = keyboardWatcher.HitCountResetMode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComboKeys_WhenSettingWithNonNullValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                ComboKeys = new List<KeyCode>()
                {
                    KeyCode.Left,
                    KeyCode.Right,
                },
            };

            var expected = new List<KeyCode>()
            {
                KeyCode.Left,
                KeyCode.Right,
            };

            // Act
            var actual = keyboardWatcher.ComboKeys;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object);
            var expected = 16;

            // Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = keyboardWatcher.InputDownElapsedMS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                InputReleasedTimeout = 2000,
                InputDownTimeOut = 3000,
            };

            var expected = 1.234f;

            // Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 1234) });
            var actual = keyboardWatcher.InputDownElapsedSeconds;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object);

            var expected = 31;

            // Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 31) });
            var actual = keyboardWatcher.InputReleasedElapsedMS;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.W)).Returns(false);

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.W,
                HitCountMax = 0,
                InputReleasedTimeout = 5000,
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 4321) };
            var expected = 4.321;

            // Act/Assert
            keyboardWatcher.Update(engineTime); // Run once to get the internal states.

            // keyboardWatcher.Update(engineTime);
            var actual = Math.Round(keyboardWatcher.InputReleasedElapsedSeconds, 3);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                InputDownTimeOut = 123,
            };

            var expected = 123;

            // Act
            var actual = keyboardWatcher.InputDownTimeOut;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputReleasedTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                InputReleasedTimeout = 456,
            };

            var expected = 456;

            // Act
            var actual = keyboardWatcher.InputReleasedTimeout;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReleasedElapsedResetMode_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                ReleasedElapsedResetMode = ResetType.Manual,
            };

            var expected = ResetType.Manual;

            // Act
            var actual = keyboardWatcher.ReleasedElapsedResetMode;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWithNullOnInputHitCountReachedEvent_RunsWithNoNullReferenceExceptions()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
                HitCountMax = 0,
            };

            // Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                keyboardWatcher.Update(new EngineTime());
            });
        }

        [Fact]
        public void Update_WhenInvokingWhileCountingWithNullCounter_RunsWithNoNullReferenceExceptions()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
                HitCountMax = 0,
            };

            keyboardWatcher.SetFieldNull("counter");

            // Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                keyboardWatcher.Update(new EngineTime());
            });
        }

        [Fact]
        public void Update_WhenInvokingWithProperButtonState_ResetsDownTimerAndStartsButtonUpTimer()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
                HitCountMax = 0,
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 10) };
            var expectedDownTimerElapsed = 0;
            var expectedUpTimerElapsed = 10;

            // Act/Assert
            keyboardWatcher.Update(engineTime); // Run once to get the internal states.
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Left)).Returns(false);
            keyboardWatcher.Update(engineTime); // Run update again to set the current and previous states different
            keyboardWatcher.Update(engineTime);
            var actualDownTimerElapsed = keyboardWatcher.InputDownElapsedMS;
            var actualUpTimerElapsed = keyboardWatcher.InputReleasedElapsedMS;

            // Assert
            Assert.Equal(expectedDownTimerElapsed, actualDownTimerElapsed);
            Assert.Equal(expectedUpTimerElapsed, actualUpTimerElapsed);
        }

        [Fact]
        public void Update_WhenDisabled_DoNotUpdateAnything()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Left)).Returns(true);
            this.mockKeyboard.Setup(m => m.IsKeyUp(KeyCode.Left)).Returns(true);
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Left)).Returns(true);

            var expectedCurrentHitCount = 0;
            var expectedInputHitCountReachedEventInvoked = false;
            var expectedInputDownTimeOutEventInvoked = false;
            var expectedInputReleasedTimeOutEventInvoked = false;
            var expectedInputComboPressedEventInvoked = false;

            var actualInputHitCountReachedEventInvoked = false;
            var actualInputDownTimeOutEventInvoked = false;
            var actualInputReleasedTimeOutEventInvoked = false;
            var actualInputComboPressedEventInvoked = false;

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Enabled = false,
                Key = KeyCode.Left,
                HitCountMax = 1,
            };

            // These events should never be fired.
            keyboardWatcher.OnInputHitCountReached += (sender, e) => actualInputHitCountReachedEventInvoked = true;
            keyboardWatcher.OnInputDownTimeOut += (sender, e) => actualInputDownTimeOutEventInvoked = true;
            keyboardWatcher.OnInputReleasedTimeOut += (sender, e) => actualInputReleasedTimeOutEventInvoked = true;
            keyboardWatcher.OnInputComboPressed += (sender, e) => actualInputComboPressedEventInvoked = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 2000) };

            // Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);
            var actualCurrentHitCount = keyboardWatcher.CurrentHitCount;

            // Assert
            Assert.Equal(expectedCurrentHitCount, actualCurrentHitCount);
            Assert.Equal(expectedInputHitCountReachedEventInvoked, actualInputHitCountReachedEventInvoked);
            Assert.Equal(expectedInputDownTimeOutEventInvoked, actualInputDownTimeOutEventInvoked);
            Assert.Equal(expectedInputReleasedTimeOutEventInvoked, actualInputReleasedTimeOutEventInvoked);
            Assert.Equal(expectedInputComboPressedEventInvoked, actualInputComboPressedEventInvoked);
        }

        [Fact]
        public void Update_WhenUpdating_InvokesHitCountReachedEvent()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyPressed(KeyCode.Left)).Returns(true);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
                HitCountMax = 2,
            };
            keyboardWatcher.OnInputHitCountReached += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            // Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenUpdating_InvokesInputDownTimeoutEvent()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Left)).Returns(true);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
                InputDownTimeOut = 1000,
            };
            keyboardWatcher.OnInputDownTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            // Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenUpdatingWithNullInputDownTimeoutEvent_ShouldNotThrowNullRefException()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Left,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                keyboardWatcher.Update(engineTime);
                keyboardWatcher.Update(engineTime);
            });
        }

        [Fact]
        public void Update_WhenUpdating_InvokesInputReleaseTimeoutEvent()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyUp(KeyCode.Right)).Returns(true);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Right,
            };
            keyboardWatcher.OnInputReleasedTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            // Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenUpdatingWithNullInputReleasedTimeoutEvent_ShouldNotThrowNullRefException()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyUp(KeyCode.Right)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                Key = KeyCode.Right,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            // Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                keyboardWatcher.Update(engineTime);
                keyboardWatcher.Update(engineTime);
            });
        }

        [Fact]
        public void Update_WhenInvokingWithOnInputComboPressedNotNullAndSetComboKeys_InvokesOnInputComboPressedEvent()
        {
            // Arrange
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Left)).Returns(true);
            this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Right)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                ComboKeys = new List<KeyCode>()
                {
                    KeyCode.Left,
                    KeyCode.Right,
                },
            };
            var expected = true;
            var actual = false;

            keyboardWatcher.OnInputComboPressed += new EventHandler((sender, e) => actual = true);

            // Act
            keyboardWatcher.Update(new EngineTime());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithNullOnInputComboPressedEvent_DoesNotThrowNullException()
        {
            // Arrange
            var keyboardWatcher = new KeyboardWatcher(this.mockKeyboard.Object)
            {
                ComboKeys = new List<KeyCode>()
                {
                    KeyCode.Left,
                    KeyCode.Right,
                },
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            // Act & Assert
            AssertExt.DoesNotThrowNullReference(() => keyboardWatcher.Update(engineTime));
        }
        #endregion

        public void Dispose() => this.mockKeyboard = null;
    }
}

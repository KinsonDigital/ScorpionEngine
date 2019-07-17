using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Input;
using KDScorpionEngine;
using PluginSystem;

namespace KDScorpionEngineTests.Input
{
    /// <summary>
    /// Unit tests to test the <see cref="KeyboardWatcher"/> class.
    /// </summary>
    public class KeyboardWatcherTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> _mockKeyboard;
        #endregion


        #region Constructors
        public KeyboardWatcherTests()
        {
            _mockKeyboard = new Mock<IKeyboard>();
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object);
            var expectedComboButtons = new List<KeyCodes>();
            var expectedEnabled = true;

            //Act
            var actualEnabled = keyboardWatcher.Enabled;
            var actualComboButtons = keyboardWatcher.ComboKeys;

            //Assert
            Assert.Equal(expectedEnabled, actualEnabled);
            Assert.Equal(expectedComboButtons, actualComboButtons);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Button_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left
            };
            var expected = KeyCodes.Left;

            //Act
            var actual = keyboardWatcher.Key;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void CurrentHitCountPercentage_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyPressed(KeyCodes.Left)).Returns(true);
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left,
                HitCountMax = 10
            };
            //This value is meaningless and is only for satisfy the update param
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 0) };

            var expected = 60;

            //Act
            for (int i = 0; i < 6; i++)
            {
                keyboardWatcher.Update(engineTime);
            }

            var actual = keyboardWatcher.CurrentHitCountPercentage;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void DownElapsedResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                DownElapsedResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = keyboardWatcher.DownElapsedResetMode;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void HitCountResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                HitCountResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = keyboardWatcher.HitCountResetMode;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ComboKeys_WhenSettingWithNonNullValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                ComboKeys = new List<KeyCodes>()
                {
                    KeyCodes.Left,
                    KeyCodes.Right
                }
            };

            var expected = new List<KeyCodes>()
            {
                KeyCodes.Left,
                KeyCodes.Right
            };

            //Act
            var actual = keyboardWatcher.ComboKeys;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputDownElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object);
            var expected = 16;

            //Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = keyboardWatcher.InputDownElapsedMS;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputDownElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                InputReleasedTimeout = 2000,
                InputDownTimeOut = 3000
            };

            var expected = 1.234f;

            //Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 1234) });
            var actual = keyboardWatcher.InputDownElapsedSeconds;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputReleasedElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object);

            var expected = 31;

            //Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 31) });
            var actual = keyboardWatcher.InputReleasedElapsedMS;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputReleasedElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.W)).Returns(false);

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.W,
                HitCountMax = 0,
                InputReleasedTimeout = 5000
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 4321) };
            var expected = 4.321;

            //Act/Assert
            keyboardWatcher.Update(engineTime);//Run once to get the internal states.

            //keyboardWatcher.Update(engineTime);
            var actual = Math.Round(keyboardWatcher.InputReleasedElapsedSeconds, 3);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                InputDownTimeOut = 123
            };

            var expected = 123;

            //Act
            var actual = keyboardWatcher.InputDownTimeOut;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputReleasedTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                InputReleasedTimeout = 456
            };

            var expected = 456;

            //Act
            var actual = keyboardWatcher.InputReleasedTimeout;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ReleasedElapsedResetMode_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                ReleasedElapsedResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = keyboardWatcher.ReleasedElapsedResetMode;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWithNullOnInputHitCountReachedEvent_RunsWithNoNullReferenceExceptions()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyPressed(KeyCodes.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left,
                HitCountMax = 0
            };

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                keyboardWatcher.Update(new EngineTime());
            });
        }


        [Fact]
        public void Update_WhenInvokingWhileCountingWithNullCounter_RunsWithNoNullReferenceExceptions()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyPressed(KeyCodes.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left,
                HitCountMax = 0
            };

            keyboardWatcher.SetFieldNull("_counter");

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                keyboardWatcher.Update(new EngineTime());
            });
        }


        [Fact]
        public void Update_WhenInvokingWithProperButtonState_ResetsDownTimerAndStartsButtonUpTimer()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left,
                HitCountMax = 0
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 10) };
            var expectedDownTimerElapsed = 0;
            var expectedUpTimerElapsed = 10;

            //Act/Assert
            keyboardWatcher.Update(engineTime);//Run once to get the internal states.
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(false);
            keyboardWatcher.Update(engineTime);//Run update again to set the current and previous states different
            keyboardWatcher.Update(engineTime);
            var actualDownTimerElapsed = keyboardWatcher.InputDownElapsedMS;
            var actualUpTimerElapsed = keyboardWatcher.InputReleasedElapsedMS;

            //Assert
            Assert.Equal(expectedDownTimerElapsed, actualDownTimerElapsed);
            Assert.Equal(expectedUpTimerElapsed, actualUpTimerElapsed);
        }


        [Fact]
        public void Update_WhenDisabled_DoNotUpdateAnything()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(true);
            _mockKeyboard.Setup(m => m.IsKeyUp(KeyCodes.Left)).Returns(true);
            _mockKeyboard.Setup(m => m.IsKeyPressed(KeyCodes.Left)).Returns(true);

            var expectedCurrentHitCount = 0;
            var expectedInputHitCountReachedEventInvoked = false;
            var expectedInputDownTimeOutEventInvoked = false;
            var expectedInputReleasedTimeOutEventInvoked = false;
            var expectedInputComboPressedEventInvoked = false;

            var actualInputHitCountReachedEventInvoked = false;
            var actualInputDownTimeOutEventInvoked = false;
            var actualInputReleasedTimeOutEventInvoked = false;
            var actualInputComboPressedEventInvoked = false;

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Enabled = false,
                Key = KeyCodes.Left,
                HitCountMax = 1
            };

            //These events should never be fired.
            keyboardWatcher.OnInputHitCountReached += (sender, e) => actualInputHitCountReachedEventInvoked = true;
            keyboardWatcher.OnInputDownTimeOut += (sender, e) => actualInputDownTimeOutEventInvoked = true;
            keyboardWatcher.OnInputReleasedTimeOut += (sender, e) => actualInputReleasedTimeOutEventInvoked = true;
            keyboardWatcher.OnInputComboPressed += (sender, e) => actualInputComboPressedEventInvoked = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 2000) };

            //Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);
            var actualCurrentHitCount = keyboardWatcher.CurrentHitCount;

            //Assert
            Assert.Equal(expectedCurrentHitCount, actualCurrentHitCount);
            Assert.Equal(expectedInputHitCountReachedEventInvoked, actualInputHitCountReachedEventInvoked);
            Assert.Equal(expectedInputDownTimeOutEventInvoked, actualInputDownTimeOutEventInvoked);
            Assert.Equal(expectedInputReleasedTimeOutEventInvoked, actualInputReleasedTimeOutEventInvoked);
            Assert.Equal(expectedInputComboPressedEventInvoked, actualInputComboPressedEventInvoked);
        }


        [Fact]
        public void Update_WhenUpdating_InvokesHitCountReachedEvent()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyPressed(KeyCodes.Left)).Returns(true);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left,
                HitCountMax = 2
            };
            keyboardWatcher.OnInputHitCountReached += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenUpdating_InvokesInputDownTimeoutEvent()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(true);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left,
                InputDownTimeOut = 1000
            };
            keyboardWatcher.OnInputDownTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenUpdatingWithNullInputDownTimeoutEvent_ShouldNotThrowNullRefException()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Left,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                keyboardWatcher.Update(engineTime);
                keyboardWatcher.Update(engineTime);
            });
        }


        [Fact]
        public void Update_WhenUpdating_InvokesInputReleaseTimeoutEvent()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyUp(KeyCodes.Right)).Returns(true);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Right,
            };
            keyboardWatcher.OnInputReleasedTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenUpdatingWithNullInputReleasedTimeoutEvent_ShouldNotThrowNullRefException()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyUp(KeyCodes.Right)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                Key = KeyCodes.Right,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                keyboardWatcher.Update(engineTime);
                keyboardWatcher.Update(engineTime);
            });
        }


        [Fact]
        public void Update_WhenInvokingWithOnInputComboPressedNotNullAndSetComboKeys_InvokesOnInputComboPressedEvent()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(true);
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Right)).Returns(true);

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                ComboKeys = new List<KeyCodes>()
                {
                    KeyCodes.Left,
                    KeyCodes.Right
                }
            };
            var expected = true;
            var actual = false;
            
            keyboardWatcher.OnInputComboPressed += new EventHandler((sender, e) => actual = true);

            //Act
            keyboardWatcher.Update(new EngineTime());

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvokingWithNullOnInputComboPressedEvent_DoesNotThrowNullException()
        {
            //Arrange
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(() =>
            {
                return _mockKeyboard.Object;
            });

            var keyboardWatcher = new KeyboardWatcher(_mockKeyboard.Object)
            {
                ComboKeys = new List<KeyCodes>()
                {
                    KeyCodes.Left,
                    KeyCodes.Right
                }
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act & Assert
            AssertExt.DoesNotThrowNullReference(() => keyboardWatcher.Update(engineTime));
        }
        #endregion


        #region Public Methods
        public void Dispose() => _mockKeyboard = null;
        #endregion
    }
}

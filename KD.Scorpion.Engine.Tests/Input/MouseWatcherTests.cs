using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Input;
using KDScorpionEngine;

namespace KDScorpionEngineTests.Input
{
    /// <summary>
    /// Unit tests to test the <see cref="MouseWatcher"/> class.
    /// </summary>
    public class MouseWatcherTests : IDisposable
    {
        #region Private Fields
        private Mock<IMouse> _mockMouse;
        #endregion


        #region Constructors
        public MouseWatcherTests()
        {
            _mockMouse = new Mock<IMouse>();
            _mockMouse.Setup(m => m.IsButtonPressed(InputButton.LeftButton)).Returns(true);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object);

            var expectedComboButtons = new List<InputButton>();
            var expectedEnabled = true;

            //Act
            var actualEnabled = mouseWatcher.Enabled;
            var actualComboButtons = mouseWatcher.ComboButtons;

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
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton
            };
            var expected = InputButton.LeftButton;

            //Act
            var actual = mouseWatcher.Button;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void CurrentHitCountPercentage_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
                HitCountMax = 10
            };
            //This value is meaningless and is only for satisfy the update param
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 0) };

            var expected = 60;

            //Act
            for (int i = 0; i < 6; i++)
            {
                mouseWatcher.Update(engineTime);
            }

            var actual = mouseWatcher.CurrentHitCountPercentage;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void DownElapsedResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                DownElapsedResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = mouseWatcher.DownElapsedResetMode;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void HitCountResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                HitCountResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = mouseWatcher.HitCountResetMode;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ComboButtons_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                ComboButtons = new List<InputButton>()
                {
                    InputButton.LeftButton,
                    InputButton.RightButton
                }
            };

            var expected = new List<InputButton>()
            {
                InputButton.LeftButton,
                InputButton.RightButton
            };

            //Act
            var actual = mouseWatcher.ComboButtons;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputDownElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object);

            var expected = 16;

            //Act
            mouseWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = mouseWatcher.InputDownElapsedMS;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputDownElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                InputReleasedTimeout = 2000
            };

            var expected = 1.234f;

            //Act
            mouseWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 1234) });
            var actual = mouseWatcher.InputDownElapsedSeconds;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputReleasedElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object);

            var expected = 31;

            //Act
            mouseWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 31) });
            var actual = mouseWatcher.InputReleasedElapsedMS;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputReleasedElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.MiddleButton)).Returns(false);

            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.MiddleButton,
                HitCountMax = 0,
                InputReleasedTimeout = 5000
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 4321) };
            var expected = 4.321;

            //Act/Assert
            mouseWatcher.Update(engineTime);//Run once to get the internal states.
            var actual = Math.Round(mouseWatcher.InputReleasedElapsedSeconds, 3);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                InputDownTimeOut = 123
            };

            var expected = 123;

            //Act
            var actual = mouseWatcher.InputDownTimeOut;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputReleasedTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                InputReleasedTimeout = 456
            };

            var expected = 456;

            //Act
            var actual = mouseWatcher.InputReleasedTimeout;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ReleasedElapsedResetMode_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                ReleasedElapsedResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = mouseWatcher.ReleasedElapsedResetMode;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWithNullOnInputHitCountReachedEvent_RunsWithNoNullReferenceExceptions()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
                HitCountMax = 0
            };

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                mouseWatcher.Update(new EngineTime());
            });
        }


        [Fact]
        public void Update_WhenInvokingWhileCountingWithNullCounter_RunsWithNoNullReferenceExceptions()
        {
            //Arrange
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
                HitCountMax = 0
            };

            mouseWatcher.SetFieldNull("_counter");

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                mouseWatcher.Update(new EngineTime());
            });
        }


        [Fact]
        public void Update_WhenInvokingWithProperButtonState_ResetsDownTimerAndStartsButtonUpTimer()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);

            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
                HitCountMax = 0
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 10) };
            var expectedDownTimerElapsed = 0;
            var expectedUpTimerElapsed = 10;

            //Act/Assert
            mouseWatcher.Update(engineTime);//Run once to get the internal states.
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(false);//Return false to simulate that the button is not down
            mouseWatcher.Update(engineTime);//Run update again to set the current and previous states different
            mouseWatcher.Update(engineTime);
            var actualDownTimerElapsed = mouseWatcher.InputDownElapsedMS;
            var actualUpTimerElapsed = mouseWatcher.InputReleasedElapsedMS;

            //Assert
            Assert.Equal(expectedDownTimerElapsed, actualDownTimerElapsed);
            Assert.Equal(expectedUpTimerElapsed, actualUpTimerElapsed);
        }


        [Fact]
        public void Update_WhenDisabled_DoNotUpdateAnything()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);
            _mockMouse.Setup(m => m.IsButtonUp(InputButton.LeftButton)).Returns(true);
            _mockMouse.Setup(m => m.IsButtonPressed(InputButton.LeftButton)).Returns(true);

            var expectedCurrentHitCount = 0;
            var expectedInputHitCountReachedEventInvoked = false;
            var expectedInputDownTimeOutEventInvoked = false;
            var expectedInputReleasedTimeOutEventInvoked = false;
            var expectedInputComboPressedEventInvoked = false;

            var actualInputHitCountReachedEventInvoked = false;
            var actualInputDownTimeOutEventInvoked = false;
            var actualInputReleasedTimeOutEventInvoked = false;
            var actualInputComboPressedEventInvoked = false;

            var mouseWatcher = new MouseWatcher(false, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
                HitCountMax = 1
            };

            //These events should never be fired.
            mouseWatcher.OnInputHitCountReached += (sender, e) => actualInputHitCountReachedEventInvoked = true;
            mouseWatcher.OnInputDownTimeOut += (sender, e) => actualInputDownTimeOutEventInvoked = true;
            mouseWatcher.OnInputReleasedTimeOut += (sender, e) => actualInputReleasedTimeOutEventInvoked = true;
            mouseWatcher.OnInputComboPressed += (sender, e) => actualInputComboPressedEventInvoked = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 2000) };

            //Act
            mouseWatcher.Update(engineTime);
            mouseWatcher.Update(engineTime);
            var actualCurrentHitCount = mouseWatcher.CurrentHitCount;

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
            _mockMouse.Setup(m => m.IsButtonPressed(InputButton.LeftButton)).Returns(true);

            var expected = true;
            var actual = false;
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
                HitCountMax = 2
            };
            mouseWatcher.OnInputHitCountReached += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            mouseWatcher.Update(engineTime);
            mouseWatcher.Update(engineTime);
            mouseWatcher.Update(engineTime);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenUpdating_InvokesInputDownTimeoutEvent()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);

            var expected = true;
            var actual = false;
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
            };
            mouseWatcher.OnInputDownTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            mouseWatcher.Update(engineTime);
            mouseWatcher.Update(engineTime);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenUpdatingWithNullInputDownTimeoutEvent_ShouldNotThrowNullRefException()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);

            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.LeftButton,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                mouseWatcher.Update(engineTime);
                mouseWatcher.Update(engineTime);
            });
        }


        [Fact]
        public void Update_WhenUpdating_InvokesInputReleaseTimeoutEvent()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonUp(InputButton.RightButton)).Returns(true);

            var expected = true;
            var actual = false;
            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.RightButton,
            };
            mouseWatcher.OnInputReleasedTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            mouseWatcher.Update(engineTime);
            mouseWatcher.Update(engineTime);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenUpdatingWithNullInputReleasedTimeoutEvent_ShouldNotThrowNullRefException()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonUp(InputButton.RightButton)).Returns(true);

            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                Button = InputButton.RightButton,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                mouseWatcher.Update(engineTime);
                mouseWatcher.Update(engineTime);
            });
        }


        [Fact]
        public void Update_WhenInvokingWithSetComboButtons_InvokesOnInputComboPressedEvent()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.RightButton)).Returns(true);

            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                ComboButtons = new List<InputButton>()
                {
                    InputButton.LeftButton,
                    InputButton.RightButton
                }
            };
            var expected = true;
            var actual = false;
            
            mouseWatcher.OnInputComboPressed += new EventHandler((sender, e) => actual = true);

            //Act
            mouseWatcher.Update(new EngineTime());

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvokingWithNullOnInputComboPressedEvent_DoesNotThrowNullException()
        {
            //Arrange
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.LeftButton)).Returns(true);
            _mockMouse.Setup(m => m.IsButtonDown(InputButton.RightButton)).Returns(true);

            var mouseWatcher = new MouseWatcher(true, _mockMouse.Object)
            {
                ComboButtons = new List<InputButton>()
                {
                    InputButton.LeftButton,
                    InputButton.RightButton
                }
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act & Assert
            AssertExt.DoesNotThrowNullReference(() => mouseWatcher.Update(engineTime));
        }
        #endregion


        #region Public Methods
        public void Dispose() => _mockMouse = null;
        #endregion
    }
}

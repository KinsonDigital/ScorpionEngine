using Moq;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Input;
using ScorpionEngine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class MouseWatcherTests
    {
        #region Method Tests
        [Fact]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            //Arrange
            SetupPluginAs<IMouse>();

            var mouseWatcher = new MouseWatcher(true);

            var expectedComboButtons = new List<InputButton>();
            var expectedEnabled = true;

            //Act
            var actualEnabled = mouseWatcher.Enabled;
            var actualComboButtons = mouseWatcher.ComboButtons;

            //Assert
            Assert.Equal(expectedEnabled, actualEnabled);
            Assert.Equal(expectedComboButtons, actualComboButtons);
            AssertExt.IsNullOrZeroField(mouseWatcher, "_mouse");
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Button_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
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
            var mouse = new Mock<IMouse>();
            mouse.Setup(m => m.IsButtonPressed((int)InputButton.LeftButton)).Returns(true);
            SetupPluginAs(mouse);
            var mouseWatcher = new MouseWatcher(true)
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
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
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
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
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
            var mouseWatcher = new MouseWatcher(true)
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
            var mouseWatcher = new MouseWatcher(true);

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
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
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
            var mouseWatcher = new MouseWatcher(true);

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
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.MiddleButton)).Returns(false);

            SetupPluginAs(mockMouse);

            var mouseWatcher = new MouseWatcher(true)
            {
                Button = InputButton.MiddleButton,
                HitCountMax = 0,
                InputReleasedTimeout = 5000
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 4321) };
            var expected = 4.321;

            //Act/Assert
            mouseWatcher.Update(engineTime);//Run once to get the internal states.
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.MiddleButton)).Returns(false);//Return false to simulate that the button is not down
            //mouseWatcher.Update(engineTime);
            var actual = Math.Round(mouseWatcher.InputReleasedElapsedSeconds, 3);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
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
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
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
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
            {
                ReleasedElapsedResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = mouseWatcher.ReleasedElapsedResetMode;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ResetHitCountOnEnable_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
            {
                ResetHitCountOnEnable = true
            };

            var expected = true;

            //Act
            var actual = mouseWatcher.ResetHitCountOnEnable;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ResetHitCountOnInputRelease_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
            {
                ResetHitCountOnInputRelease = true
            };

            var expected = true;

            //Act
            var actual = mouseWatcher.ResetHitCountOnInputRelease;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ResetTimeOnEnable_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            SetupPluginAs<IMouse>();
            var mouseWatcher = new MouseWatcher(true)
            {
                ResetTimeOnEnable = true
            };

            var expected = true;

            //Act
            var actual = mouseWatcher.ResetTimeOnEnable;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWithNullOnInputHitCountReachedEvent_RunsWithNoNullReferenceExceptions()
        {
            //Arrange
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.LeftButton)).Returns(true);

            SetupPluginAs(mockMouse);

            var mouseWatcher = new MouseWatcher(true)
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
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonPressed((int)InputButton.LeftButton)).Returns(true);

            SetupPluginAs(mockMouse);

            var mouseWatcher = new MouseWatcher(true)
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
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);

            SetupPluginAs(mockMouse);

            var mouseWatcher = new MouseWatcher(true)
            {
                Button = InputButton.LeftButton,
                HitCountMax = 0
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 10) };
            var expectedDownTimerElapsed = 0;
            var expectedUpTimerElapsed = 10;

            //Act/Assert
            mouseWatcher.Update(engineTime);//Run once to get the internal states.
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(false);//Return false to simulate that the button is not down
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
            var mouse = new Mock<IMouse>();
            mouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);
            mouse.Setup(m => m.IsButtonUp((int)InputButton.LeftButton)).Returns(true);
            mouse.Setup(m => m.IsButtonPressed((int)InputButton.LeftButton)).Returns(true);

            SetupPluginAs(mouse);

            var expectedCurrentHitCount = 0;
            var expectedInputHitCountReachedEventInvoked = false;
            var expectedInputDownTimeOutEventInvoked = false;
            var expectedInputReleasedTimeOutEventInvoked = false;
            var expectedInputComboPressedEventInvoked = false;

            var actualInputHitCountReachedEventInvoked = false;
            var actualInputDownTimeOutEventInvoked = false;
            var actualInputReleasedTimeOutEventInvoked = false;
            var actualInputComboPressedEventInvoked = false;

            var mouseWatcher = new MouseWatcher(false)
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
            var mouse = new Mock<IMouse>();
            mouse.Setup(m => m.IsButtonPressed((int)InputButton.LeftButton)).Returns(true);
            SetupPluginAs(mouse);

            var expected = true;
            var actual = false;
            var mouseWatcher = new MouseWatcher(true)
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
            var mouse = new Mock<IMouse>();
            mouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);
            SetupPluginAs(mouse);

            var expected = true;
            var actual = false;
            var mouseWatcher = new MouseWatcher(true)
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
            var mouse = new Mock<IMouse>();
            mouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);
            SetupPluginAs(mouse);

            var mouseWatcher = new MouseWatcher(true)
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
            var mouse = new Mock<IMouse>();
            mouse.Setup(m => m.IsButtonUp((int)InputButton.RightButton)).Returns(true);
            SetupPluginAs(mouse);

            var expected = true;
            var actual = false;
            var mouseWatcher = new MouseWatcher(true)
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
            var mouse = new Mock<IMouse>();
            mouse.Setup(m => m.IsButtonUp((int)InputButton.RightButton)).Returns(true);
            SetupPluginAs(mouse);

            var mouseWatcher = new MouseWatcher(true)
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
            var mockMouse = new Mock<IMouse>();
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.RightButton)).Returns(true);

            SetupPluginAs(mockMouse);
            var mouseWatcher = new MouseWatcher(true)
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
            var mockMouse = new Mock<IMouse>();
            SetupPluginAs(mockMouse);
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.LeftButton)).Returns(true);
            mockMouse.Setup(m => m.IsButtonDown((int)InputButton.RightButton)).Returns(true);

            var mouseWatcher = new MouseWatcher(true)
            {
                ComboButtons = new List<InputButton>()
                {
                    InputButton.LeftButton,
                    InputButton.RightButton
                }
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act

            //Assert
            AssertExt.DoesNotThrowNullReference(() => mouseWatcher.Update(engineTime));
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Sets up the plugin system to pull in the required object by the given generic type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of mocked plugin to load into the plugin system.  <typeparamref name="T"/> must implement an interface of type <see cref="IPlugin"/>.</typeparam>
        private void SetupPluginAs<T>() where T : class, IPlugin
        {
            var mockMouse = new Mock<T>();
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<T>()).Returns(mockMouse.Object);

            PluginSystem.LoadEnginePluginLibrary(mockPluginLib.Object);
        }


        private void SetupPluginAs<T>(Mock<T> mockObject) where T : class, IPlugin
        {
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<T>()).Returns(mockObject.Object);

            PluginSystem.LoadEnginePluginLibrary(mockPluginLib.Object);
        }
        #endregion
    }
}

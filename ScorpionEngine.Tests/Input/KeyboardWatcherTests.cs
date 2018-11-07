using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Input;
using System;
using System.Collections.Generic;


namespace ScorpionEngine.Tests.Input
{
    public class KeyboardWatcherTests
    {
        #region Method Tests
        [Test]
        public void Ctor_WhenUsingEnabled_CorrectlyPerformSetup()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true);

            var expectedComboButtons = new List<InputKeys>();
            var expectedEnabled = true;

            //Act
            var actualEnabled = keyboardWatcher.Enabled;
            var actualComboButtons = keyboardWatcher.ComboKeys;

            //Assert
            Assert.AreEqual(expectedEnabled, actualEnabled);
            Assert.AreEqual(expectedComboButtons, actualComboButtons);
            AssertExt.IsNullOrZeroField(keyboardWatcher, "_keyboard");
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Button_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockKeyboard.Object);

            PluginSystem.LoadEnginePluginLibrary(mockPluginLib.Object);
            
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left
            };
            var expected = InputKeys.Left;

            //Act
            var actual = keyboardWatcher.Key;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void CurrentHitCountPercentage_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyPressed((int)InputKeys.Left)).Returns(true);
            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left,
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DownElapsedResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                DownElapsedResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = keyboardWatcher.DownElapsedResetMode;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void HitCountResetMode_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                HitCountResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = keyboardWatcher.HitCountResetMode;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ComboButtons_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                ComboKeys = new List<InputKeys>()
                {
                    InputKeys.Left,
                    InputKeys.Right
                }
            };

            var expected = new List<InputKeys>()
            {
                InputKeys.Left,
                InputKeys.Right
            };

            //Act
            var actual = keyboardWatcher.ComboKeys;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void InputDownElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true);
            var expected = 16;

            //Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = keyboardWatcher.InputDownElapsedMS;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void InputDownElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                InputReleasedTimeout = 2000,
                InputDownTimeOut = 3000
            };

            var expected = 1.234f;

            //Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 1234) });
            var actual = keyboardWatcher.InputDownElapsedSeconds;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void InputReleasedElapsedMS_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true);

            var expected = 31;

            //Act
            keyboardWatcher.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 31) });
            var actual = keyboardWatcher.InputReleasedElapsedMS;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void InputReleasedElapsedSeconds_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.W)).Returns(false);

            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.W,
                HitCountMax = 0,
                InputReleasedTimeout = 5000
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 4321) };
            var expected = 4.321;

            //Act/Assert
            keyboardWatcher.Update(engineTime);//Run once to get the internal states.
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.W)).Returns(false);//Return false to simulate that the button is not down
            //keyboardWatcher.Update(engineTime);
            var actual = Math.Round(keyboardWatcher.InputReleasedElapsedSeconds, 3);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void InputDownTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                InputDownTimeOut = 123
            };

            var expected = 123;

            //Act
            var actual = keyboardWatcher.InputDownTimeOut;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void InputReleasedTimeOut_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                InputReleasedTimeout = 456
            };

            var expected = 456;

            //Act
            var actual = keyboardWatcher.InputReleasedTimeout;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ReleasedElapsedResetMode_WhenSettingAndGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            Helpers.SetupPluginLib<IKeyboard>(PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                ReleasedElapsedResetMode = ResetType.Manual
            };

            var expected = ResetType.Manual;

            //Act
            var actual = keyboardWatcher.ReleasedElapsedResetMode;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Update_WhenInvokingWithNullOnInputHitCountReachedEvent_RunsWithNoNullReferenceExceptions()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyPressed((int)InputKeys.Left)).Returns(true);

            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left,
                HitCountMax = 0
            };


            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                keyboardWatcher.Update(new EngineTime());
            });
        }


        [Test]
        public void Update_WhenInvokingWhileCountingWithNullCounter_RunsWithNoNullReferenceExceptions()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyPressed((int)InputKeys.Left)).Returns(true);

            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left,
                HitCountMax = 0
            };

            keyboardWatcher.SetFieldNull("_counter");

            //Act/Assert
            AssertExt.DoesNotThrowNullReference(() =>
            {
                keyboardWatcher.Update(new EngineTime());
            });
        }


        [Test]
        public void Update_WhenInvokingWithProperButtonState_ResetsDownTimerAndStartsButtonUpTimer()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);

            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left,
                HitCountMax = 0
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 10) };
            var expectedDownTimerElapsed = 0;
            var expectedUpTimerElapsed = 10;

            //Act/Assert
            keyboardWatcher.Update(engineTime);//Run once to get the internal states.
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(false);//Return false to simulate that the button is not down
            keyboardWatcher.Update(engineTime);//Run update again to set the current and previous states different
            keyboardWatcher.Update(engineTime);
            var actualDownTimerElapsed = keyboardWatcher.InputDownElapsedMS;
            var actualUpTimerElapsed = keyboardWatcher.InputReleasedElapsedMS;

            //Assert
            Assert.AreEqual(expectedDownTimerElapsed, actualDownTimerElapsed);
            Assert.AreEqual(expectedUpTimerElapsed, actualUpTimerElapsed);
        }


        [Test]
        public void Update_WhenDisabled_DoNotUpdateAnything()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);
            mockMouse.Setup(m => m.IsKeyUp((int)InputKeys.Left)).Returns(true);
            mockMouse.Setup(m => m.IsKeyPressed((int)InputKeys.Left)).Returns(true);

            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var expectedCurrentHitCount = 0;
            var expectedInputHitCountReachedEventInvoked = false;
            var expectedInputDownTimeOutEventInvoked = false;
            var expectedInputReleasedTimeOutEventInvoked = false;
            var expectedInputComboPressedEventInvoked = false;

            var actualInputHitCountReachedEventInvoked = false;
            var actualInputDownTimeOutEventInvoked = false;
            var actualInputReleasedTimeOutEventInvoked = false;
            var actualInputComboPressedEventInvoked = false;

            var keyboardWatcher = new KeyboardWatcher(false)
            {
                Key = InputKeys.Left,
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
            Assert.AreEqual(expectedCurrentHitCount, actualCurrentHitCount);
            Assert.AreEqual(expectedInputHitCountReachedEventInvoked, actualInputHitCountReachedEventInvoked);
            Assert.AreEqual(expectedInputDownTimeOutEventInvoked, actualInputDownTimeOutEventInvoked);
            Assert.AreEqual(expectedInputReleasedTimeOutEventInvoked, actualInputReleasedTimeOutEventInvoked);
            Assert.AreEqual(expectedInputComboPressedEventInvoked, actualInputComboPressedEventInvoked);
        }


        [Test]
        public void Update_WhenUpdating_InvokesHitCountReachedEvent()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyPressed((int)InputKeys.Left)).Returns(true);
            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left,
                HitCountMax = 2
            };
            keyboardWatcher.OnInputHitCountReached += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenUpdating_InvokesInputDownTimeoutEvent()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);
            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left,
                InputDownTimeOut = 1000
            };
            keyboardWatcher.OnInputDownTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenUpdatingWithNullInputDownTimeoutEvent_ShouldNotThrowNullRefException()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);
            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Left,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                keyboardWatcher.Update(engineTime);
                keyboardWatcher.Update(engineTime);
            });
        }


        [Test]
        public void Update_WhenUpdating_InvokesInputReleaseTimeoutEvent()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyUp((int)InputKeys.Right)).Returns(true);
            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var expected = true;
            var actual = false;
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Right,
            };
            keyboardWatcher.OnInputReleasedTimeOut += (sender, e) => actual = true;

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            keyboardWatcher.Update(engineTime);
            keyboardWatcher.Update(engineTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenUpdatingWithNullInputReleasedTimeoutEvent_ShouldNotThrowNullRefException()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyUp((int)InputKeys.Right)).Returns(true);
            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);

            var keyboardWatcher = new KeyboardWatcher(true)
            {
                Key = InputKeys.Right,
            };

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act/Assert
            AssertExt.DoesNotThrow<NullReferenceException>(() =>
            {
                keyboardWatcher.Update(engineTime);
                keyboardWatcher.Update(engineTime);
            });
        }


        [Test]
        public void Update_WhenInvokingWithSetComboButtons_InvokesOnInputComboPressedEvent()
        {
            //Arrange
            var mockMouse = new Mock<IKeyboard>();
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);
            mockMouse.Setup(m => m.IsKeyDown((int)InputKeys.Right)).Returns(true);

            Helpers.SetupPluginLib(mockMouse, PluginLibType.Engine);
            var keyboardWatcher = new KeyboardWatcher(true)
            {
                ComboKeys = new List<InputKeys>()
                {
                    InputKeys.Left,
                    InputKeys.Right
                }
            };
            var expected = true;
            var actual = false;
            
            keyboardWatcher.OnInputComboPressed += new EventHandler((sender, e) => actual = true);

            //Act
            keyboardWatcher.Update(new EngineTime());

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWithNullOnInputComboPressedEvent_DoesNotThrowNullException()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);
            mockKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Right)).Returns(true);

            var mockPluginLib = new Mock<IPluginLibrary>();
            mockPluginLib.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(() =>
            {
                return mockKeyboard.Object;
            });

            PluginSystem.LoadEnginePluginLibrary(mockPluginLib.Object);

            var keyboardWatcher = new KeyboardWatcher(true)
            {
                ComboKeys = new List<InputKeys>()
                {
                    InputKeys.Left,
                    InputKeys.Right
                }
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act

            //Assert
            AssertExt.DoesNotThrowNullReference(() => keyboardWatcher.Update(engineTime));
        }
        #endregion


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
        }
    }
}

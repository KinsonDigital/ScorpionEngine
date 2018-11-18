using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Input;
using System;

namespace ScorpionEngine.Tests.Input
{
    public class KeyBehaviorTests
    {
        #region Constructors
        [Test]
        public void Ctor_SingleParamValue_SetsPropsCorrectly()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var expected = true;
            var keyboard = new Keyboard(mockCoreKeyboard.Object);

            //Act
            var behavior = new KeyBehavior(true, keyboard);

            //Assert
            Assert.AreEqual(expected, behavior.Enabled);
        }


        [Test]
        public void Ctor_SingleParamValueWithNullKeyboard_InternallyLoadsPlugin()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var mockPluginLibrary = new Mock<IPluginLibrary>();
            mockPluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadEnginePluginLibrary(mockPluginLibrary.Object);

            //Act
            var behavior = new KeyBehavior(true, null);

            //Assert
            mockPluginLibrary.Verify(m => m.LoadPlugin<IKeyboard>(), Times.Once());
        }


        [Test]
        public void Ctor_TwoParamValues_SetsPropsCorrectly()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var expectedEnabled = true;
            var expectedKey = InputKeys.Right;
            var keyboard = new Keyboard(mockCoreKeyboard.Object);

            //Act
            var behavior = new KeyBehavior(InputKeys.Right, true, keyboard);

            //Assert
            Assert.AreEqual(expectedEnabled, behavior.Enabled);
            Assert.AreEqual(expectedKey, behavior.Key);
        }


        [Test]
        public void Ctor_TwoParamValuesWithNullKeyboard_InternallyLoadsPlugin()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var mockPluginLibrary = new Mock<IPluginLibrary>();
            mockPluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadEnginePluginLibrary(mockPluginLibrary.Object);

            //Act
            var behavior = new KeyBehavior(InputKeys.Space, true, null);

            //Assert
            mockPluginLibrary.Verify(m => m.LoadPlugin<IKeyboard>(), Times.Once());
        }
        #endregion


        #region Prop Tests
        [Test]
        public void IsDown_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var behavior = new KeyBehavior(true, keyboard)
            {
                Key = InputKeys.Space
            };
            var expected = true;

            //Act
            var actual = behavior.IsDown;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var behavior = new KeyBehavior(true, keyboard);
            var expected = "John Doe";

            //Act
            behavior.Name = "John Doe";
            var actual = behavior.Name;

            //Assert
            Assert.AreEqual(expected, actual);
        }



        [Test]
        public void TimeDelay_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var behavior = new KeyBehavior(true, keyboard);
            var expected = 1234;

            //Act
            behavior.TimeDelay = 1234;
            var actual = behavior.TimeDelay;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinous_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.KeyDownContinuous
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinousWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.KeyDownContinuous
            };

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
               keyBehavior.Update(new EngineTime());
            });

            mockCoreKeyboard.Verify(m => m.IsKeyDown((int)InputKeys.Space), Times.Once());
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDown_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyPressed((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnDown
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDownWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyPressed((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnDown
            };

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
               keyBehavior.Update(new EngineTime());
            });

            mockCoreKeyboard.Verify(m => m.IsKeyPressed((int)InputKeys.Space), Times.Once());
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnceOnRelease_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnRelease
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyUpEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnceOnReleaseWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnRelease
            };

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(new EngineTime());
            });

            //The IsKeyDown method has to be invoked as well
            mockCoreKeyboard.Verify(m => m.IsKeyUp((int)InputKeys.Space), Times.Once());
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelay_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnKeyDownTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(engineTime);
            keyBehavior.Update(engineTime);

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnKeyDownTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            keyBehavior.Update(engineTime);

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(engineTime);
            });

            //The IsKeyDown method has to be invoked as well
            mockCoreKeyboard.Verify(m => m.IsKeyDown((int)InputKeys.Space), Times.Once());
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelay_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnKeyReleaseTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyUpEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(engineTime);
            keyBehavior.Update(engineTime);

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp((int)InputKeys.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnKeyReleaseTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            //Act
            keyBehavior.Update(engineTime);

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(engineTime);
            });

            mockCoreKeyboard.Verify(m => m.IsKeyUp((int)InputKeys.Space), Times.Once());
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelay_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new int[] { (int)InputKeys.Space } );

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnAnyKeyPress
            };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyPressEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.AreEqual(expectedEventInvoked, actualEventInvoked);
        }


        [Test]
        public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new int[] { (int)InputKeys.Space });

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnAnyKeyPress
            };


            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(new EngineTime());
            });

            mockCoreKeyboard.Verify(m => m.GetCurrentPressedKeys(), Times.Once());
        }


        [Test]
        public void Update_WhenDisabled_KeyboardNotUpdated()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard)
            {
                Enabled = false
            };

            //Act
            keyBehavior.Update(new EngineTime());

            //Assert
            mockCoreKeyboard.Verify(m => m.UpdateCurrentState(), Times.Never());
            mockCoreKeyboard.Verify(m => m.UpdatePreviousState(), Times.Never());
        }


        [Test]
        public void Update_WhenInvokedWithIncorrectBehaviorType_ThrowsException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(InputKeys.Space, true, keyboard);

            //Act
            keyBehavior.BehaviorType = (KeyBehaviorType)123;

            //Assert
            Assert.Throws<Exception>(() =>
            {
                keyBehavior.Update(new EngineTime());
            });
        }
        #endregion
    }
}

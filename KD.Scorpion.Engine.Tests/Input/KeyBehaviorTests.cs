using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine;
using KDScorpionEngine.Input;

namespace KDScorpionEngineTests.Input
{
    public class KeyBehaviorTests
    {
        #region Constructors
        [Fact]
        public void Ctor_SingleParamValue_SetsPropsCorrectly()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var expected = true;
            var keyboard = new Keyboard(mockCoreKeyboard.Object);

            //Act
            var behavior = new KeyBehavior(true, keyboard);

            //Assert
            Assert.Equal(expected, behavior.Enabled);
        }


        [Fact]
        public void Ctor_TwoParamValues_SetsPropsCorrectly()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            var expectedEnabled = true;
            var expectedKey = KeyCodes.Right;
            var keyboard = new Keyboard(mockCoreKeyboard.Object);

            //Act
            var behavior = new KeyBehavior(KeyCodes.Right, true, keyboard);

            //Assert
            Assert.Equal(expectedEnabled, behavior.Enabled);
            Assert.Equal(expectedKey, behavior.Key);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void IsDown_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var behavior = new KeyBehavior(true, keyboard)
            {
                Key = KeyCodes.Space
            };
            var expected = true;

            //Act
            var actual = behavior.IsDown;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var behavior = new KeyBehavior(true, keyboard);
            var expected = "John Doe";

            //Act
            behavior.Name = "John Doe";
            var actual = behavior.Name;

            //Assert
            Assert.Equal(expected, actual);
        }



        [Fact]
        public void TimeDelay_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var behavior = new KeyBehavior(true, keyboard);
            var expected = 1234;

            //Act
            behavior.TimeDelay = 1234;
            var actual = behavior.TimeDelay;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinous_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.KeyDownContinuous
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinousWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.KeyDownContinuous
            };

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
               keyBehavior.Update(new EngineTime());
            });

            mockCoreKeyboard.Verify(m => m.IsKeyDown(KeyCodes.Space), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDown_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyPressed(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnDown
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDownWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyPressed(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnDown
            };

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
               keyBehavior.Update(new EngineTime());
            });

            mockCoreKeyboard.Verify(m => m.IsKeyPressed(KeyCodes.Space), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnRelease_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnRelease
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyUpEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnReleaseWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnceOnRelease
            };

            //Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(new EngineTime());
            });

            //The IsKeyDown method has to be invoked as well
            mockCoreKeyboard.Verify(m => m.IsKeyUp(KeyCodes.Space), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelay_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
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
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
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
            mockCoreKeyboard.Verify(m => m.IsKeyDown(KeyCodes.Space), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelay_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
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
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyUp(KeyCodes.Space)).Returns(true);

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
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

            mockCoreKeyboard.Verify(m => m.IsKeyUp(KeyCodes.Space), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelay_InvokeKeyDownEvent()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new [] { KeyCodes.Space } );

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                BehaviorType = KeyBehaviorType.OnAnyKeyPress
            };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            //Act
            keyBehavior.KeyPressEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            //Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new [] { KeyCodes.Space });

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
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


        [Fact]
        public void Update_WhenDisabled_KeyboardNotUpdated()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard)
            {
                Enabled = false
            };

            //Act
            keyBehavior.Update(new EngineTime());

            //Assert
            mockCoreKeyboard.Verify(m => m.UpdateCurrentState(), Times.Never());
            mockCoreKeyboard.Verify(m => m.UpdatePreviousState(), Times.Never());
        }


        [Fact]
        public void Update_WhenInvokedWithIncorrectBehaviorType_ThrowsException()
        {
            //Arrange
            var mockCoreKeyboard = new Mock<IKeyboard>();

            var keyboard = new Keyboard(mockCoreKeyboard.Object);
            var keyBehavior = new KeyBehavior(KeyCodes.Space, true, keyboard);

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

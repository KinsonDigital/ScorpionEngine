using System;
using Moq;
using Xunit;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine;
using Raptor.Input;
using Raptor.Plugins;
using Raptor;

namespace KDScorpionEngineTests.Input
{
    /// <summary>
    /// Unit tests to test the <see cref="KeyBehavior"/> class.
    /// </summary>
    public class KeyBehaviorTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> _mockCoreKeyboard;
        #endregion


        #region Constructors
        public KeyBehaviorTests()
        {
            _mockCoreKeyboard = new Mock<IKeyboard>();
            _mockCoreKeyboard.Setup(m => m.IsKeyDown(It.IsAny<KeyCode>())).Returns(true);
            _mockCoreKeyboard.Setup(m => m.IsKeyPressed(It.IsAny<KeyCode>())).Returns(true);
            _mockCoreKeyboard.Setup(m => m.IsKeyUp(It.IsAny<KeyCode>())).Returns(true);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_SingleParamValue_SetsPropsCorrectly()
        {
            // Act
            _mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCode.X)).Returns(true);
            var behavior = new KeyBehavior(_mockCoreKeyboard.Object);

            // Assert
            Assert.Equal(KeyCode.X, behavior.Key);
        }


        [Fact]
        public void Ctor_TwoParamValues_SetsPropsCorrectly()
        {
            // Act
            var behavior = new KeyBehavior(_mockCoreKeyboard.Object);

            // Assert
            Assert.True(behavior.Enabled);
            Assert.Equal(KeyCode.X, behavior.Key);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void IsDown_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var behavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                Key = KeyCode.Space
            };
            var expected = true;

            // Act
            var actual = behavior.IsDown;

            // Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Name_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var behavior = new KeyBehavior(_mockCoreKeyboard.Object);
            var expected = "John Doe";

            // Act
            behavior.Name = "John Doe";
            var actual = behavior.Name;

            // Assert
            Assert.Equal(expected, actual);
        }



        [Fact]
        public void TimeDelay_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var behavior = new KeyBehavior(_mockCoreKeyboard.Object);
            var expected = 1234;

            // Act
            behavior.TimeDelay = 1234;
            var actual = behavior.TimeDelay;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinous_InvokeKeyDownEvent()
        {
            // Arrange
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.KeyDownContinuous
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            // Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinousWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.KeyDownContinuous
            };

            // Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
               keyBehavior.Update(new EngineTime());
            });

            _mockCoreKeyboard.Verify(m => m.IsKeyDown(It.IsAny<KeyCode>()), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDown_InvokeKeyDownEvent()
        {
            // Arrange
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnceOnDown
            };

            var actualEventInvoked = false;

            // Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            // Assert
            Assert.True(actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDownWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnceOnDown
            };

            // Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
               keyBehavior.Update(new EngineTime());
            });

            _mockCoreKeyboard.Verify(m => m.IsKeyPressed(It.IsAny<KeyCode>()), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnRelease_InvokeKeyDownEvent()
        {
            // Arrange
            _mockCoreKeyboard.Setup(m => m.IsKeyUp(KeyCode.Space)).Returns(true);
            
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnceOnRelease
            };

            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            // Act
            keyBehavior.KeyUpEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnReleaseWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            _mockCoreKeyboard.Setup(m => m.IsKeyUp(KeyCode.Space)).Returns(true);

            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnceOnRelease
            };

            // Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(new EngineTime());
            });

            // The IsKeyDown method has to be invoked as well
            _mockCoreKeyboard.Verify(m => m.IsKeyUp(It.IsAny<KeyCode>()), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelay_InvokeKeyDownEvent()
        {
            // Arrange
            _mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCode.Space)).Returns(true);

            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnKeyDownTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            // Act
            keyBehavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(engineTime);
            keyBehavior.Update(engineTime);

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            _mockCoreKeyboard.Setup(m => m.IsKeyDown(KeyCode.Space)).Returns(true);

            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnKeyDownTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            keyBehavior.Update(engineTime);

            // Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(engineTime);
            });

            // The IsKeyDown method has to be invoked as well
            _mockCoreKeyboard.Verify(m => m.IsKeyDown(It.IsAny<KeyCode>()), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelay_InvokeKeyDownEvent()
        {
            // Arrange
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnKeyReleaseTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            // Act
            keyBehavior.KeyUpEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(engineTime);
            keyBehavior.Update(engineTime);

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            _mockCoreKeyboard.Setup(m => m.IsKeyUp(KeyCode.Space)).Returns(true);

            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnKeyReleaseTimeDelay
            };
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 501) };

            // Act
            keyBehavior.Update(engineTime);

            // Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(engineTime);
            });

            _mockCoreKeyboard.Verify(m => m.IsKeyUp(It.IsAny<KeyCode>()), Times.Once());
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelay_InvokeKeyDownEvent()
        {
            // Arrange
            _mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new [] { KeyCode.Space } );

            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnAnyKeyPress
            };
            var expectedEventInvoked = true;
            var actualEventInvoked = false;

            // Act
            keyBehavior.KeyPressEvent += (sender, e) => actualEventInvoked = true;
            keyBehavior.Update(new EngineTime());

            // Assert
            Assert.Equal(expectedEventInvoked, actualEventInvoked);
        }


        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            _mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new [] { KeyCode.Space });

            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = KeyBehaviorType.OnAnyKeyPress
            };


            // Act/Assert
            AssertExt.DoesNotThrow<Exception>(() =>
            {
                keyBehavior.Update(new EngineTime());
            });

            _mockCoreKeyboard.Verify(m => m.GetCurrentPressedKeys(), Times.Once());
        }


        [Fact]
        public void Update_WhenDisabled_KeyboardNotUpdated()
        {
            // Arrange
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                Enabled = false
            };

            // Act
            keyBehavior.Update(new EngineTime());

            // Assert
            _mockCoreKeyboard.Verify(m => m.UpdateCurrentState(), Times.Never());
            _mockCoreKeyboard.Verify(m => m.UpdatePreviousState(), Times.Never());
        }


        [Fact]
        public void Update_WhenInvokedWithIncorrectBehaviorType_ThrowsException()
        {
            // Arrange & Act
            var keyBehavior = new KeyBehavior(_mockCoreKeyboard.Object)
            {
                BehaviorType = (KeyBehaviorType)123
            };

            // Assert
            Assert.Throws<Exception>(() =>
            {
                keyBehavior.Update(new EngineTime());
            });
        }
        #endregion


        #region Public Methods
        public void Dispose() => _mockCoreKeyboard = null;
        #endregion
    }
}

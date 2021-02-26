// <copyright file="KeyBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Behaviors
{
    using System;
    using KDScorpionEngine;
    using KDScorpionEngine.Behaviors;
    using Moq;
    using Raptor.Input;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="KeyBehavior"/> class.
    /// </summary>
    public class KeyBehaviorTests
    {
        private readonly Mock<IKeyboard> mockKeyboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBehaviorTests"/> class.
        /// </summary>
        public KeyBehaviorTests() => this.mockKeyboard = new Mock<IKeyboard>();

        #region Constructor Tests
        [Fact]
        public void Ctor_SingleParamValue_SetsPropsCorrectly()
        {
            // Act
            var behavior = CreateBehavior(KeyCode.X);

            // Assert
            Assert.Equal(KeyCode.X, behavior.Key);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void IsDown_WhenGettingValue_ReturnsCorrectValue()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);
            var behavior = CreateBehavior(KeyCode.Space);
            behavior.Update(new GameTime());
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
            var behavior = CreateBehavior();
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
            var behavior = CreateBehavior();
            var expected = 1234;

            // Act
            behavior.TimeDelay = 1234;
            var actual = behavior.TimeDelay;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Enabled_WhenSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var behavior = CreateBehavior();

            // Act
            behavior.Enabled = false;

            // Assert
            Assert.False(behavior.Enabled);
        }

        [Fact]
        public void ID_WhenCreatingNewInstance_IDIsSet()
        {
            // Act
            var behavior = CreateBehavior();

            // Assert
            Assert.True(behavior.ID != Guid.Empty);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenDisabled_DoesNotUpdate()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;
            behavior.Enabled = false;

            // Act
            behavior.Update(new GameTime());

            // Assert
            Assert.False(behavior.IsDown);
            this.mockKeyboard.Verify(m => m.GetState(), Times.Never());
        }

        [Fact]
        public void Update_WithUnknownKeyBehavior_ThrowsException()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = (KeyBehaviorType)1234;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<Exception>(() =>
            {
                behavior.Update(new GameTime());
            }, "Invalid 'KeyBehaviorType' of value '1234'.");
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinous_InvokeKeyDownEvent()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            // Act & Assert
            Assert.Raises<KeyEventArgs>(
                e => behavior.KeyDownEvent += e,
                e => behavior.KeyDownEvent -= e,
                () => behavior.Update(new GameTime()));
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToKeyDownContinousWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.KeyDownContinuous;

            // Act/Assert
            AssertHelpers.DoesNotThrow<Exception>(() =>
            {
                behavior.Update(new GameTime());
            });
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDown_InvokeKeyDownEvent()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnceOnDown;

            var actualEventInvoked = false;

            // Act
            behavior.KeyDownEvent += (sender, e) => actualEventInvoked = true;
            behavior.Update(new GameTime());

            // Assert
            Assert.True(actualEventInvoked);
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnDownWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnceOnDown;

            // Act/Assert
            AssertHelpers.DoesNotThrow<Exception>(() =>
            {
                behavior.Update(new GameTime());
            });
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnRelease_InvokeKeyDownEvent()
        {
            // Arrange
            MockKeyAsUp(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnceOnRelease;

            // Act & Assert
            Assert.Raises<KeyEventArgs>(
                e => behavior.KeyUpEvent += e,
                e => behavior.KeyUpEvent -= e,
                () => behavior.Update(new GameTime()));
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnceOnReleaseWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            MockKeyAsUp(KeyCode.Space);
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnceOnRelease;

            // Act/Assert
            AssertHelpers.DoesNotThrow<Exception>(() =>
            {
                behavior.Update(new GameTime());
            });
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelay_InvokeKeyDownEvent()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);

            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnKeyDownTimeDelay;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            Assert.Raises<KeyEventArgs>(
                e => behavior.KeyDownEvent += e,
                e => behavior.KeyDownEvent -= e,
                () =>
                {
                    behavior.Update(gameTime);
                    behavior.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyDownTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            MockKeyAsDown(KeyCode.Space);

            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnKeyDownTimeDelay;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            behavior.Update(gameTime);

            // Act & Assert
            AssertHelpers.DoesNotThrow<Exception>(() =>
            {
                behavior.Update(gameTime);
            });
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelay_InvokeKeyDownEvent()
        {
            // Arrange
            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnKeyReleaseTimeDelay;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act & Assert
            Assert.Raises<KeyEventArgs>(
                e => behavior.KeyUpEvent += e,
                e => behavior.KeyUpEvent  -= e,
                () =>
                {
                    behavior.Update(gameTime);
                    behavior.Update(gameTime);
                });
        }

        [Fact]
        public void Update_WhenKeyBehaviorIsSetToOnKeyReleaseTimeDelayWithNoEventSetup_DoesNotThrowException()
        {
            // Arrange
            MockKeyAsUp(KeyCode.Space);

            var behavior = CreateBehavior();
            behavior.BehaviorType = KeyBehaviorType.OnKeyReleaseTimeDelay;

            var gameTime = new GameTime();
            gameTime.AddTime(501);

            // Act
            behavior.Update(gameTime);

            // Act/Assert
            AssertHelpers.DoesNotThrow<Exception>(() =>
            {
                behavior.Update(gameTime);
            });
        }

        //[Fact]
        //public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelay_InvokeKeyDownEvent()
        //{
        //    // Arrange
        //    this.mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new[] { KeyCode.Space });

        //    var behavior = CreateBehavior(this.mockCoreKeyboard.Object)
        //    {
        //        BehaviorType = KeyBehaviorType.OnAnyKeyPress,
        //    };
        //    var expectedEventInvoked = true;
        //    var actualEventInvoked = false;

        //    // Act
        //    behavior.KeyPressEvent += (sender, e) => actualEventInvoked = true;
        //    behavior.Update(new GameTime());

        //    // Assert
        //    Assert.Equal(expectedEventInvoked, actualEventInvoked);
        //}

        //[Fact]
        //public void Update_WhenKeyBehaviorIsSetToOnAnyKeyPressTimeDelayWithNoEventSetup_DoesNotThrowException()
        //{
        //    // Arrange
        //    this.mockCoreKeyboard.Setup(m => m.GetCurrentPressedKeys()).Returns(new[] { KeyCode.Space });

        //    var behavior = CreateBehavior(this.mockCoreKeyboard.Object)
        //    {
        //        BehaviorType = KeyBehaviorType.OnAnyKeyPress,
        //    };

        //    // Act/Assert
        //    AssertHelpers.DoesNotThrow<Exception>(() =>
        //    {
        //        behavior.Update(new GameTime());
        //    });

        //    this.mockCoreKeyboard.Verify(m => m.GetCurrentPressedKeys(), Times.Once());
        //}

        //[Fact]
        //public void Update_WhenDisabled_KeyboardNotUpdated()
        //{
        //    // Arrange
        //    var behavior = CreateBehavior(this.mockCoreKeyboard.Object)
        //    {
        //        Enabled = false,
        //    };

        //    // Act
        //    behavior.Update(new GameTime());

        //    // Assert
        //    this.mockCoreKeyboard.Verify(m => m.UpdateCurrentState(), Times.Never());
        //    this.mockCoreKeyboard.Verify(m => m.UpdatePreviousState(), Times.Never());
        //}

        //[Fact]
        //public void Update_WhenInvokedWithIncorrectBehaviorType_ThrowsException()
        //{
        //    // Arrange & Act
        //    var behavior = CreateBehavior(this.mockCoreKeyboard.Object)
        //    {
        //        BehaviorType = (KeyBehaviorType)123,
        //    };

        //    // Assert
        //    Assert.Throws<Exception>(() =>
        //    {
        //        behavior.Update(new GameTime());
        //    });
        //}
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="KeyBehavior"/> for the purpose of testing.
        /// </summary>
        /// <param name="key">The key to assign the behavior to.</param>
        /// <returns>The instance to test.</returns>
        private KeyBehavior CreateBehavior(KeyCode key = KeyCode.Space) => new KeyBehavior(key, this.mockKeyboard.Object, true);

        /// <summary>
        /// Mocks the given <paramref name="key"/> as in the down position.
        /// </summary>
        /// <param name="key">The key to mock.</param>
        private void MockKeyAsDown(KeyCode key)
        {
            this.mockKeyboard.Setup(m => m.GetState())
                .Returns(() =>
                {
                    var keyState = new KeyboardState();
                    keyState.SetKeyState(key, true);

                    return keyState;
                });
        }

        /// <summary>
        /// Mocks the given <paramref name="key"/> as in the up position.
        /// </summary>
        /// <param name="key">The key to mock.</param>
        private void MockKeyAsUp(KeyCode key)
        {
            this.mockKeyboard.Setup(m => m.GetState())
                .Returns(() =>
                {
                    var keyState = new KeyboardState();
                    keyState.SetKeyState(key, false);

                    return keyState;
                });
        }
    }
}

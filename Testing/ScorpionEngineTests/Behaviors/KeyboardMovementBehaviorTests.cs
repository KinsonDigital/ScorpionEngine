// <copyright file="KeyboardMovementBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

#pragma warning disable IDE0049 // Simplify Names
namespace KDScorpionEngineTests.Behaviors
{
    using System;
    using System.Numerics;
    using KDScorpionEngine;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using Moq;
    using Raptor.Input;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="MoveLeftKeyboardBehavior{T}"/> class.
    /// </summary>
    public class KeyboardMovementBehaviorTests
    {
        private readonly Mock<IGameInput<KeyCode, KeyboardState>> mockGameInput;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMovementBehaviorTests"/> class.
        /// </summary>
        public KeyboardMovementBehaviorTests() => this.mockGameInput = new Mock<IGameInput<KeyCode, KeyboardState>>();

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoked_ProperlySetsUpDefaults()
        {
            // Arrange
            var entity = new Entity();

            // Act
            var behavior = CreateBehavior(entity, (gameTime, currentPos) => Vector2.Zero);

            // Assert
            Assert.True(behavior.Enabled);
            Assert.NotEqual(Guid.Empty, behavior.ID);
        }

        [Fact]
        public void Ctor_WithNullEntity_ThrowsException()
        {
            // Arrange
            Entity entity = null;

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
            {
                CreateBehavior(entity, (gameTime, currentPos) => Vector2.Zero);
            }, "The parameter must not be null. (Parameter 'entity')");
        }

        [Fact]
        public void Ctor_WithNullCalcPositionDelegate_ThrowsException()
        {
            // Arrange
            var entity = new Entity();

            // Act & Assert
            AssertHelpers.ThrowsWithMessage<ArgumentNullException>(() =>
            {
                CreateBehavior(entity, null);
            }, "The parameter must not be null. (Parameter 'calcPosition')");
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void MoveKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new Entity();

            var behavior = CreateBehavior(entity, (gameTime, currentPos) => Vector2.Zero);
            var expected = KeyCode.Space;

            // Act
            behavior.MoveKey = KeyCode.Space;
            var actual = behavior.MoveKey;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvoked_CorrectlyUpdatesPosition()
        {
            // Arrange
            MockKey(KeyCode.Space, true);
            var expected = new Vector2(30, 40f);
            var entity = new Entity();
            entity.Position = new Vector2(20, 40);

            var behavior = CreateBehavior(
                entity,
                (gameTime, currentPos) => new Vector2(currentPos.X + 10, currentPos.Y));

            behavior.MoveKey = KeyCode.Space;

            var gameTime = new GameTime();
            gameTime.AddTime(16);

            // Act
            behavior.Update(gameTime);

            // Assert
            Assert.Equal(expected, entity.Position);
        }

        [Fact]
        public void Update_WhenKeyIsDown_SetsEntityAsMoving()
        {
            // Arrange
            var entity = new Entity();

            var behavior = CreateBehavior(entity, (gameTime, currentPos) => Vector2.Zero);
            behavior.MoveKey = KeyCode.Space;

            MockKey(KeyCode.Space, true);

            var gameTime = new GameTime();
            gameTime.AddTime(16);

            // Act
            behavior.Update(gameTime);

            // Assert
            Assert.True(behavior.IsMoving);
        }

        [Fact]
        public void Dispose_WhenInvoked_DisposesOfBehavior()
        {
            // Arrange
            var entity = new Entity();
            var behavior = CreateBehavior(entity, (gameTime, currentPos) => Vector2.Zero);

            // Act
            behavior.Dispose();
            behavior.Dispose();

            // Assert
            AssertHelpers.PrivateFieldTrue(behavior, "isDisposed");
        }
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="KeyboardMovementBehavior"/> for the purpose of testing.
        /// </summary>
        /// <param name="entity">The entity to add to the behavior.</param>
        /// <returns>The instance to test.</returns>
        private KeyboardMovementBehavior CreateBehavior(Entity entity, Func<GameTime, Vector2, Vector2> calcPosition)
            => new KeyboardMovementBehavior(this.mockGameInput.Object, entity, calcPosition);

        /// <summary>
        /// Mocks the given key to be set to the given state.
        /// </summary>
        /// <param name="key">The key to mock.</param>
        /// <param name="state">The state to mock the key to.</param>
        private void MockKey(KeyCode key, bool state)
        {
            this.mockGameInput.Setup(m => m.GetState()).Returns(() =>
            {
                var keyboardState = new KeyboardState();
                keyboardState.SetKeyState(key, state);

                return keyboardState;
            });
        }
    }
}

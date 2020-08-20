// <copyright file="MovementByKeyboardBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Behaviors
{
    using System;
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using Moq;
    using Raptor;
    using Raptor.Input;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="MovementByKeyboardBehavior{T}"/> class.
    /// </summary>
    public class MovementByKeyboardBehaviorTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> mockKeyboard;
        private Mock<IPhysicsBody> mockPhysicsBody;
        #endregion

        #region Constructors
        public MovementByKeyboardBehaviorTests()
        {
            this.mockKeyboard = new Mock<IKeyboard>();
            this.mockPhysicsBody = new Mock<IPhysicsBody>();
            this.mockPhysicsBody.SetupProperty(m => m.X);
            this.mockPhysicsBody.SetupProperty(m => m.Y);
            this.mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>())).
                Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    this.mockPhysicsBody.Object.X += forceX;
                    this.mockPhysicsBody.Object.Y += forceY;
                });
        }
        #endregion

        #region Constructor Tests

        // [Fact]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            // Arrange
            SetKeyboardKey(KeyCode.Right);
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
            {
                LinearSpeed = 1234,
            };

            // Act
            behavior.Update(new EngineTime());

            // Assert
            Assert.Equal(1234, entity.Position.X);
        }

        // [Fact]
        public void Ctor_WhenInvoking_CreatesMoveLeftBehavior()
        {
            // Arrange
            SetKeyboardKey(KeyCode.Left);
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
            {
                LinearSpeed = -5678,
            };

            // Act
            behavior.Update(new EngineTime());

            // Assert
            Assert.Equal(-5678, entity.Position.X);
        }

        // [Fact]
        public void Ctor_WhenInvoking_CreatesMoveUpBehavior()
        {
            // Arrange
            SetKeyboardKey(KeyCode.Up);
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
            {
                LinearSpeed = -1478,
            };

            // Act
            behavior.Update(new EngineTime());

            // Assert
            Assert.Equal(-1478, entity.Position.Y);
        }

        // [Fact]
        public void Ctor_WhenInvoking_CreatesMoveDownBehavior()
        {
            // Arrange
            SetKeyboardKey(KeyCode.Down);
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
            {
                LinearSpeed = 9876,
            };

            // Act
            behavior.Update(new EngineTime());

            // Assert
            Assert.Equal(9876, entity.Position.Y);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void MoveUpKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            // Arrange
            SetKeyboardKey(It.IsAny<KeyCode>());
            var entity = new DynamicEntity();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
            var expected = KeyCode.W;

            // Act
            behavior.MoveUpKey = KeyCode.W;
            var actual = behavior.MoveUpKey;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MoveDownKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            // Arrange
            SetKeyboardKey(It.IsAny<KeyCode>());
            var entity = new DynamicEntity(It.IsAny<Vector2[]>(), It.IsAny<Vector2>());

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
            var expected = KeyCode.S;

            // Act
            behavior.MoveDownKey = KeyCode.S;
            var actual = behavior.MoveDownKey;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MoveLeftKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            // Arrange
            SetKeyboardKey(It.IsAny<KeyCode>());
            var entity = new DynamicEntity(It.IsAny<Vector2[]>(), It.IsAny<Vector2>());

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
            var expected = KeyCode.S;

            // Act
            behavior.MoveLeftKey = KeyCode.S;
            var actual = behavior.MoveLeftKey;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MoveRightKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            // Arrange
            SetKeyboardKey(It.IsAny<KeyCode>());
            var entity = new DynamicEntity(It.IsAny<Vector2[]>(), It.IsAny<Vector2>());

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
            var expected = KeyCode.S;

            // Act
            behavior.MoveRightKey = KeyCode.S;
            var actual = behavior.MoveRightKey;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        public void Dispose()
        {
            this.mockKeyboard = null;
            this.mockPhysicsBody = null;
        }

        #region Private Methods
        private void SetKeyboardKey(KeyCode key) => this.mockKeyboard.Setup(m => m.IsKeyDown(key)).Returns(true);
        #endregion
    }
}

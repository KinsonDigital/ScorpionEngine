// <copyright file="BehaviorFactoryTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Factories
{
    using System.Numerics;
    using KDScorpionEngine.Behaviors;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Factories;
    using Raptor.Input;
    using Xunit;

    /// <summary>
    /// Tests the <see cref="BehaviorFactory"/> class.
    /// </summary>
    public class BehaviorFactoryTests
    {
        #region Method Tests
        [Fact]
        public void CreatekeyboardMovement_WhenInvoked_CreatesBehavior()
        {
            // Arrange
            var entity = new Entity();

            // Act
            var actual = BehaviorFactory.CreateKeyboardMovement(
                entity,
                KeyCode.Right,
                (gameTime, currentPosition) => Vector2.Zero) as KeyboardMovementBehavior;

            // Assert
            Assert.Equal(KeyCode.Right, actual.MoveKey);
        }
        #endregion
    }
}

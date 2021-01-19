// <copyright file="MovementByKeyboardBehaviorTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Behaviors
{
    using KDScorpionEngine.Behaviors;

    //TODO: DO NOT DELETE THIS!! This will be reworked once the new keyboard implementation for the Raptor keyboard are finished
    // Refer to cards below for more info
    // 1. https://dev.azure.com/KinsonDigital/GameDevTools/_workitems/edit/2424
    // 2. https://dev.azure.com/KinsonDigital/GameDevTools/_workitems/edit/2425


    /// <summary>
    /// Unit tests to test the <see cref="MovementByKeyboardBehavior{T}"/> class.
    /// </summary>
    public class MovementByKeyboardBehaviorTests //: IDisposable
    {
        //public MovementByKeyboardBehaviorTests()
        //{
        //}

        //#region Constructor Tests

        //// [Fact]
        //public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        //{
        //    // Arrange
        //    SetKeyboardKey(KeyCode.Right);
        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);
        //    entity.Initialize();

        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
        //    {
        //        LinearSpeed = 1234,
        //    };

        //    // Act
        //    behavior.Update(new GameTime());

        //    // Assert
        //    Assert.Equal(1234, entity.Position.X);
        //}

        //// [Fact]
        //public void Ctor_WhenInvoking_CreatesMoveLeftBehavior()
        //{
        //    // Arrange
        //    SetKeyboardKey(KeyCode.Left);
        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);
        //    entity.Initialize();

        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
        //    {
        //        LinearSpeed = -5678,
        //    };

        //    // Act
        //    behavior.Update(new GameTime());

        //    // Assert
        //    Assert.Equal(-5678, entity.Position.X);
        //}

        //// [Fact]
        //public void Ctor_WhenInvoking_CreatesMoveUpBehavior()
        //{
        //    // Arrange
        //    SetKeyboardKey(KeyCode.Up);
        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);
        //    entity.Initialize();

        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
        //    {
        //        LinearSpeed = -1478,
        //    };

        //    // Act
        //    behavior.Update(new GameTime());

        //    // Assert
        //    Assert.Equal(-1478, entity.Position.Y);
        //}

        //// [Fact]
        //public void Ctor_WhenInvoking_CreatesMoveDownBehavior()
        //{
        //    // Arrange
        //    SetKeyboardKey(KeyCode.Down);
        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);
        //    entity.Initialize();

        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
        //    {
        //        LinearSpeed = 9876,
        //    };

        //    // Act
        //    behavior.Update(new GameTime());

        //    // Assert
        //    Assert.Equal(9876, entity.Position.Y);
        //}
        //#endregion

        //#region Prop Tests
        //[Fact]
        //public void MoveUpKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        //{
        //    // Arrange
        //    SetKeyboardKey(It.IsAny<KeyCode>());
        //    var entity = new DynamicEntity();
        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
        //    var expected = KeyCode.W;

        //    // Act
        //    behavior.MoveUpKey = KeyCode.W;
        //    var actual = behavior.MoveUpKey;

        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void MoveDownKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        //{
        //    // Arrange
        //    SetKeyboardKey(It.IsAny<KeyCode>());
        //    var entity = new DynamicEntity(It.IsAny<Vector2[]>(), It.IsAny<Vector2>());

        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
        //    var expected = KeyCode.S;

        //    // Act
        //    behavior.MoveDownKey = KeyCode.S;
        //    var actual = behavior.MoveDownKey;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void MoveLeftKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        //{
        //    // Arrange
        //    SetKeyboardKey(It.IsAny<KeyCode>());
        //    var entity = new DynamicEntity(It.IsAny<Vector2[]>(), It.IsAny<Vector2>());

        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
        //    var expected = KeyCode.S;

        //    // Act
        //    behavior.MoveLeftKey = KeyCode.S;
        //    var actual = behavior.MoveLeftKey;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void MoveRightKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        //{
        //    // Arrange
        //    SetKeyboardKey(It.IsAny<KeyCode>());
        //    var entity = new DynamicEntity(It.IsAny<Vector2[]>(), It.IsAny<Vector2>());

        //    var behavior = new MovementByKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
        //    var expected = KeyCode.S;

        //    // Act
        //    behavior.MoveRightKey = KeyCode.S;
        //    var actual = behavior.MoveRightKey;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}
        //#endregion
    }
}

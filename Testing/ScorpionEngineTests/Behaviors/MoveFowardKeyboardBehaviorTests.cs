// <copyright file="MoveFowardKeyboardBehaviorTests.cs" company="KinsonDigital">
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
    /// Unit tests to test the <see cref="MoveFowardKeyboardBehavior{T}"/> class.
    /// </summary>
    public class MoveFowardKeyboardBehaviorTests //: IDisposable
    {
        //public MoveFowardKeyboardBehaviorTests()
        //{
        //    this.mockKeyboard = new Mock<IKeyboard>();

        //    this.mockPhysicsBody = new Mock<IPhysicsBody>();
        //    this.mockPhysicsBody.SetupProperty(p => p.Angle);
        //    this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
        //    {
        //        this.mockPhysicsBody.Object.Angle += value;
        //    });
        //}

        //#region Prop Tests
        //[Fact]
        //public void MoveFowardKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);

        //    var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
        //    var expected = KeyCode.Space;

        //    // Act
        //    behavior.MoveFowardKey = KeyCode.Space;
        //    var actual = behavior.MoveFowardKey;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void RotateCWKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);
        //    var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
        //    var expected = KeyCode.Space;

        //    // Act
        //    behavior.RotateCWKey = KeyCode.Space;
        //    var actual = behavior.RotateCWKey;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void RotateCCWKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        //{
        //    // Arrange
        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);
        //    var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);
        //    var expected = KeyCode.Space;

        //    // Act
        //    behavior.RotateCCWKey = KeyCode.Space;
        //    var actual = behavior.RotateCCWKey;

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public void IsMovingFoward_WhenGettingValue_ReturnsTrue()
        //{
        //    // Arrange
        //    this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Up)).Returns(true);

        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);

        //    entity.Initialize();

        //    var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity);

        //    // Act
        //    behavior.Update(new GameTime() { CurrentFrameElapsed = new TimeSpan(0, 0, 0, 0, 16) });
        //    var actual = behavior.IsMovingForward;

        //    // Assert
        //    Assert.True(behavior.IsMovingForward);
        //}

        //[Fact]
        //public void LinearSpeed_WhenSettingValue_ReturnsCorrectValue()
        //{
        //    // Act
        //    var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(It.IsAny<IKeyboard>(), It.IsAny<DynamicEntity>())
        //    {
        //        LinearSpeed = 12,
        //    };

        //    // Assert
        //    Assert.Equal(12, behavior.LinearSpeed);
        //}
        //#endregion

        //#region Method Tests

        //// [Fact]
        //public void Update_WhenInvoked_InvokesEntityRotateCW()
        //{
        //    // Arrange
        //    this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Right)).Returns(true);

        //    var entityXVertices = new[] { 10f, 20f, 30f };
        //    var entityYVertices = new[] { 10f, 20f, 30f };

        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);

        //    entity.Initialize();

        //    var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
        //    {
        //        AngularSpeed = 10,
        //    };

        //    // Act
        //    behavior.Update(new GameTime() { CurrentFrameElapsed = new TimeSpan(0, 0, 0, 0, 16) });
        //    var actual = entity.Angle;

        //    // Assert
        //    Assert.Equal(10, actual);
        //}

        //// [Fact]
        //public void Update_WhenInvoked_InvokesEntityRotateCCW()
        //{
        //    // Arrange
        //    this.mockKeyboard.Setup(m => m.IsKeyDown(KeyCode.Left)).Returns(true);

        //    var entity = new DynamicEntity(this.mockPhysicsBody.Object);

        //    entity.Initialize();

        //    var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(this.mockKeyboard.Object, entity)
        //    {
        //        AngularSpeed = -20,
        //    };

        //    // Act
        //    behavior.Update(new GameTime() { CurrentFrameElapsed = new TimeSpan(0, 0, 0, 0, 16) });
        //    var actual = entity.Angle;

        //    // Assert
        //    Assert.Equal(-20, actual);
        //}
        //#endregion

        ///// <inheritdoc/>
        //public void Dispose()
        //{
        //    this.mockKeyboard = null;
        //    this.mockPhysicsBody = null;
        //}
    }
}

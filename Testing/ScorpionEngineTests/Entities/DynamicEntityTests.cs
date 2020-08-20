// <copyright file="DynamicEntityTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Entities
{
    using System;
    using System.Numerics;
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Exceptions;
    using KDScorpionEngineTests.Fakes;
    using Moq;
    using Raptor;
    using Raptor.Graphics;
    using Raptor.Plugins;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="DynamicEntity"/> class.
    /// </summary>
    public class DynamicEntityTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsBody> mockPhysicsBody;
        #endregion

        #region Constructors
        public DynamicEntityTests()
        {
            this.mockPhysicsBody = new Mock<IPhysicsBody>();
            this.mockPhysicsBody.SetupProperty(p => p.Angle);
            this.mockPhysicsBody.SetupProperty(p => p.AngularDeceleration);
            this.mockPhysicsBody.SetupProperty(p => p.AngularVelocity);
            this.mockPhysicsBody.SetupProperty(p => p.LinearVelocityX);
            this.mockPhysicsBody.SetupProperty(p => p.LinearVelocityY);

            // _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
            //    .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
            //    {
            //        _mockPhysicsBody.Object.LinearVelocityX = forceX;
            //        _mockPhysicsBody.Object.LinearVelocityY = forceY;
            //    });
            this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                this.mockPhysicsBody.Object.AngularVelocity = value;
            });
        }
        #endregion

        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithTextureAndPosition_CreatesAllBehaviors()
        {
            // Arrange
            var expectedTotalBehaviors = 6;

            // Act
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            var actualTotalBehaviors = entity.Behaviors.Count;

            // Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }

        [Fact]
        public void Ctor_WhenInvokingWithNoParams_CreatesAllBehaviors()
        {
            // Arrange
            var expectedTotalBehaviors = 6;

            // Act
            var entity = new DynamicEntity();
            var actualTotalBehaviors = entity.Behaviors.Count;

            // Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }

        [Fact]
        public void Ctor_WhenInvokingWithFrictionParam_CreatesAllBehaviors()
        {
            // Arrange
            var expectedTotalBehaviors = 6;

            // Act
            var entity = new DynamicEntity(1f);
            var actualTotalBehaviors = entity.Behaviors.Count;

            // Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }

        [Fact]
        public void Ctor_WhenInvokingWithPosition_CreatesAllBehaviors()
        {
            // Arrange
            var expectedTotalBehaviors = 6;

            // Act
            var entity = new DynamicEntity(Vector2.Zero);
            var actualTotalBehaviors = entity.Behaviors.Count;

            // Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }

        [Fact]
        public void Ctor_WhenInvokingWithVerticesAndPosition_CreatesAllBehaviors()
        {
            // Arrange
            var vertices = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66),
                new Vector2(77, 88),
            };

            var expectedTotalBehaviors = 6;

            // Act
            var entity = new DynamicEntity(vertices, It.IsAny<Vector2>());
            var actualTotalBehaviors = entity.Behaviors.Count;

            // Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }

        [Fact]
        public void Ctor_WhenInvokingWithTextureAndVerticesAndPosition_CreatesAllBehaviors()
        {
            // Arrange
            var vertices = new Vector2[]
            {
                new Vector2(11, 22),
                new Vector2(33, 44),
                new Vector2(55, 66),
                new Vector2(77, 88),
            };

            var expectedTotalBehaviors = 6;

            // Act
            var entity = new DynamicEntity(CreateTexture(), vertices, It.IsAny<Vector2>());
            var actualTotalBehaviors = entity.Behaviors.Count;

            // Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }

        [Fact]
        public void Ctor_WhenInvokingWithTextureAndPositionAndFriction_CreatesAllBehaviors()
        {
            // Arrange
            var expectedTotalBehaviors = 6;

            // Act
            var entity = new DynamicEntity(CreateTexture(), It.IsAny<Vector2>(), 1f);
            var actualTotalBehaviors = entity.Behaviors.Count;

            // Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }
        #endregion

        #region Prop Tests
        [Fact]
        public void IsMoving_WhenGettingValueWhenBodyIsNotMoving_ValueShouldReturnFalse()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();

            var expected = false;

            // Act
            var actual = entity.IsMoving;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsMoving_WhenGettingValueWhenBodyPerformingLinearMovement_ValueShouldReturnTrue()
        {
            // Arrange
            this.mockPhysicsBody.SetupProperty(p => p.LinearVelocityX);
            this.mockPhysicsBody.SetupProperty(p => p.LinearVelocityY);

            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            entity.Body.LinearVelocity = new Vector2(11, 22);

            var expected = true;

            // Act
            var actual = entity.IsMoving;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void IsMoving_WhenGettingValueWhenBodyPerformingAngularMovement_ValueShouldReturnTrue()
        {
            // Arrange
            this.mockPhysicsBody.SetupProperty(p => p.AngularVelocity);
            this.mockPhysicsBody.SetupProperty(p => p.Angle);
            this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                this.mockPhysicsBody.Object.AngularVelocity = 1;
            });

            var entity = new DynamicEntity(this.mockPhysicsBody.Object);

            entity.Initialize();
            entity.RotateCW();

            var expected = true;

            // Act
            var actual = entity.IsMoving;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RotationEnabled_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            var expected = true;

            // Act
            entity.RotationEnabled = true;
            var actual = entity.RotationEnabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Angle_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();

            var expected = 45.45f;

            // Act
            entity.Angle = 45.45f;
            var actual = entity.Angle;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Angle_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            var expected = 45.45f;

            // Act
            entity.Angle = 45.45f;
            var actual = entity.Angle;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MaxLinearSpeed_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            var expected = 123.456f;

            // Act
            entity.MaxLinearSpeed = 123.456f;
            var actual = entity.MaxLinearSpeed;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MaxRotationSpeed_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            var expected = 123.456f;

            // Act
            entity.MaxRotationSpeed = 123.456f;
            var actual = entity.MaxRotationSpeed;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LinearDeceleration_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            // Arrange
            this.mockPhysicsBody.SetupProperty(p => p.LinearDeceleration);

            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 123.456f;

            // Act
            entity.LinearDeceleration = 123.456f;
            var actual = entity.LinearDeceleration;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LinearDeceleration_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            // Arrange
            this.mockPhysicsBody.SetupProperty(p => p.LinearDeceleration);

            var entity = new DynamicEntity(this.mockPhysicsBody.Object);

            var expected = 123.456f;

            // Act
            entity.LinearDeceleration = 123.456f;
            var actual = entity.LinearDeceleration;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AngularDeceleration_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 123.456f;

            // Act
            entity.AngularDeceleration = 123.456f;
            var actual = entity.AngularDeceleration;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AngularDeceleration_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            var expected = 123.456f;

            // Act
            entity.AngularDeceleration = 123.456f;
            var actual = entity.AngularDeceleration;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWithNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity();

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() =>
            {
                entity.Update(It.IsAny<EngineTime>());
            });
        }

        // [Fact]
        public void Update_WhenInvoking_ShouldHaveEntityStopAfterMovementInXDirection()
        {
            // Arrange
            this.mockPhysicsBody.Object.LinearVelocityX = 100;
            this.mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    this.mockPhysicsBody.Object.LinearVelocityX += forceX;
                });

            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();

            var expected = false;

            // Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.StopMovement();
            entity.MoveRight();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void Update_WhenInvoking_ShouldHaveEntityStopAfterMovementInYDirection()
        {
            // Arrange
            this.mockPhysicsBody.Object.LinearVelocityY = 100;
            this.mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    this.mockPhysicsBody.Object.LinearVelocityY = 0;
                });

            var entity = new DynamicEntity(this.mockPhysicsBody.Object);

            entity.Initialize();

            var expected = false;

            // Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.StopMovement();
            entity.MoveDown();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_WhenInvokingWithMovingEntity_ShouldStopBodyWhenStopMovementIsInvoked()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object)
            {
                SpeedX = 10,
                SpeedY = 10,
                RotateSpeed = 100,
            };

            this.mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    this.mockPhysicsBody.Object.LinearVelocityX = entity.IsEntityStopping ? 0 : forceX;
                    this.mockPhysicsBody.Object.LinearVelocityY = entity.IsEntityStopping ? 0 : forceY;
                });

            this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                this.mockPhysicsBody.Object.AngularVelocity = entity.IsEntityStopping ? 0 : value;
            });

            entity.Initialize();
            var expected = false;

            // Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.MoveRight();
            entity.MoveDown();
            entity.RotateCW(1);
            entity.StopMovement();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void Update_WhenInvoked_MaintainsMaxAngularVelocity()
        {
            // Arrange
            this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                this.mockPhysicsBody.Object.AngularVelocity = 10;
            });
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);

            entity.Initialize();
            entity.MaxRotationSpeed = 8;

            // Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.RotateCW();
            entity.Update(engineTime);

            // Assert
            Assert.Equal(8, entity.Body.AngularVelocity);
        }

        [Fact]
        public void MoveRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveRight());
        }

        [Fact]
        public void MoveRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveRight(10));
        }

        [Fact]
        public void MoveLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveLeft());
        }

        [Fact]
        public void MoveLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveLeft(10));
        }

        [Fact]
        public void MoveUp_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUp());
        }

        [Fact]
        public void MoveUp_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUp(10));
        }

        [Fact]
        public void MoveDown_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDown());
        }

        [Fact]
        public void MoveDown_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDown(10));
        }

        // [Fact]
        public void MoveUpRight_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            // Arrange
            this.mockPhysicsBody.SetupGet(p => p.X).Returns(10);
            this.mockPhysicsBody.SetupGet(p => p.Y).Returns(20);
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector2(0.25f, -0.25f);

            // Act
            entity.MoveUpRight();

            var actual = entity.Body.LinearVelocity;

            // Assert
            this.mockPhysicsBody.Verify(m => m.ApplyForce(0.25f, -0.25f, 10, 20), Times.Once());
        }

        [Fact]
        public void MoveUpRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpRight());
        }

        [Fact]
        public void MoveUpRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpRight(10));
        }

        [Fact]
        public void MoveUpLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpLeft());
        }

        [Fact]
        public void MoveUpLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpLeft(10));
        }

        [Fact]
        public void MoveDownRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownRight());
        }

        [Fact]
        public void MoveDownRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownRight(10));
        }

        [Fact]
        public void MoveDownLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownLeft());
        }

        [Fact]
        public void MoveDownLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownLeft(10));
        }

        [Fact]
        public void MoveAtSetSpeed_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveAtSetSpeed());
        }

        [Fact]
        public void MoveAtSetAngle_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveAtSetAngle(10));
        }

        // [Fact]
        public void RotateCW_WithNoParams_ProperlySetsAngularVelocity()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 1f;

            // Act
            entity.RotateCW();
            var actual = entity.Body.AngularVelocity;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void RotateCW_WithParam_ProperlySetsAngularVelocity()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 123f;

            // Act
            entity.RotateCW(123f);
            var actual = entity.Body.AngularVelocity;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RotateCW_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCW());
        }

        [Fact]
        public void RotateCW_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCW(10));
        }

        // [Fact]
        public void RotateCCW_WithNoParams_ProperlySetsAngularVelocity()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = -1f;

            // Act
            entity.RotateCCW();
            var actual = entity.Body.AngularVelocity;

            // Assert
            Assert.Equal(expected, actual);
        }

        // [Fact]
        public void RotateCCW_WithParam_ProperlySetsAngularVelocity()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = -123f;

            // Act
            entity.RotateCCW(123f);
            var actual = entity.Body.AngularVelocity;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RotateCCW_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCCW());
        }

        [Fact]
        public void RotateCCW_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCCW(10));
        }

        [Fact]
        public void StopMovement_WhenInvoked_ProperlySetsLinearVelocity()
        {
            // Arrange
            this.mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    this.mockPhysicsBody.Object.LinearVelocityX = 0;
                    this.mockPhysicsBody.Object.LinearVelocityY = 0;
                });

            this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                this.mockPhysicsBody.Object.AngularVelocity = 0;
            });

            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            entity.Initialize();
            var expected = Vector2.Zero;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            // Act
            entity.MoveDownRight();
            entity.RotateCW();
            entity.StopMovement();
            entity.Update(engineTime);
            var actual = entity.Body.LinearVelocity;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StopRotation_WhenInvoked_ProperlySetsAngularVelocity()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);

            entity.Initialize();
            var expected = 0f;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            // Act
            entity.RotateCW();
            entity.StopRotation();
            entity.Update(engineTime);
            var actual = entity.Body.AngularVelocity;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StopRotation_WhenInvokedWithNullBody_ThrowsException()
        {
            // Arrange
            var entity = new DynamicEntity(null);
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            // Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.StopRotation());
        }

        [Fact]
        public void DynamicEntity_WhenMoving_MaintainsMaxRotationSpeed()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object);
            this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                entity.Angle = 1234;
            });
            entity.MaxRotationSpeed = 10;

            entity.Initialize();
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            var expected = 10f;

            // Act
            entity.RotateCW(20);
            entity.Update(engineTime);
            var actual = entity.MaxRotationSpeed;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Initialize_WhenInvokingWhileAlreadyInitialized_MaintainsMaxAngularLimit()
        {
            // Arrange
            var entity = new DynamicEntity(this.mockPhysicsBody.Object)
            {
                MaxRotationSpeed = 10,
            };

            this.mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                entity.Angle = 1234;
            });

            entity.Initialize();

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            var expected = 10f;

            // Act
            entity.RotateCW(It.IsAny<float>());
            entity.Update(engineTime);
            var actual = entity.MaxRotationSpeed;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        public void Dispose() => this.mockPhysicsBody = null;

        private static Texture CreateTexture()
        {
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);

            return new Texture(mockTexture.Object);
        }
    }
}

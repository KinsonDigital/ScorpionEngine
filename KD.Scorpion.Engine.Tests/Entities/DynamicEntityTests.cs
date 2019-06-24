using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Exceptions;
using KDScorpionEngineTests.Fakes;
using PluginSystem;
using KDScorpionCore.Plugins;

namespace KDScorpionEngineTests.Entities
{
    public class DynamicEntityTests : IDisposable
    {
        #region Private Fields
        private Vector[] _vertices;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #endregion


        #region Constructors
        public DynamicEntityTests()
        {
            _vertices = new Vector[]
            {
                Vector.Zero,
                Vector.Zero,
                Vector.Zero
            };

            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(p => p.Angle);
            _mockPhysicsBody.SetupProperty(p => p.AngularDeceleration);
            _mockPhysicsBody.SetupProperty(p => p.AngularVelocity);
            _mockPhysicsBody.SetupProperty(p => p.LinearVelocityX);
            _mockPhysicsBody.SetupProperty(p => p.LinearVelocityY);
            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    _mockPhysicsBody.Object.LinearVelocityX = forceX;
                    _mockPhysicsBody.Object.LinearVelocityY = forceY;
                });
            _mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                _mockPhysicsBody.Object.AngularVelocity = value;
            });
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithTextureAndPosition_CreatesAllBehaviors()
        {
            //Arrange
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Fact]
        public void Ctor_WhenInvokingWithNoParams_CreatesAllBehaviors()
        {
            //Arrange
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity();
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Fact]
        public void Ctor_WhenInvokingWithFrictionParam_CreatesAllBehaviors()
        {
            //Arrange
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(1f);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Fact]
        public void Ctor_WhenInvokingWithPosition_CreatesAllBehaviors()
        {
            //Arrange
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(Vector.Zero);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Fact]
        public void Ctor_WhenInvokingWithVerticesAndPosition_CreatesAllBehaviors()
        {
            //Arrange
            var vertices = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66),
                new Vector(77, 88)
            };

            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(vertices, It.IsAny<Vector>());
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Fact]
        public void Ctor_WhenInvokingWithTextureAndVerticesAndPosition_CreatesAllBehaviors()
        {
            //Arrange
            var vertices = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66),
                new Vector(77, 88)
            };

            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(CreateTexture(), vertices, It.IsAny<Vector>());
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.Equal(expectedTotalBehaviors, actualTotalBehaviors);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void IsMoving_WhenGettingValueWhenBodyIsNotMoving_ValueShouldReturnFalse()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = false;

            //Act
            var actual = entity.IsMoving;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsMoving_WhenGettingValueWhenBodyPerformingLinearMovement_ValueShouldReturnTrue()
        {
            //Arrange
            _mockPhysicsBody.SetupProperty(p => p.LinearVelocityX);
            _mockPhysicsBody.SetupProperty(p => p.LinearVelocityY);

            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            entity.Body.LinearVelocity = new Vector(11, 22);

            var expected = true;

            //Act
            var actual = entity.IsMoving;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsMoving_WhenGettingValueWhenBodyPerformingAngularMovement_ValueShouldReturnTrue()
        {
            //Arrange
            _mockPhysicsBody.SetupProperty(p => p.AngularVelocity);
            _mockPhysicsBody.SetupProperty(p => p.Angle);
            _mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                _mockPhysicsBody.Object.AngularVelocity = 1;
            });

            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            entity.Initialize();
            entity.RotateCW();

            var expected = true;

            //Act
            var actual = entity.IsMoving;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RotationEnabled_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var expected = true;

            //Act
            entity.RotationEnabled = true;
            var actual = entity.RotationEnabled;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Angle_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = 45.45f;

            //Act
            entity.Angle = 45.45f;
            var actual = entity.Angle;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Angle_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var expected = 45.45f;

            //Act
            entity.Angle = 45.45f;
            var actual = entity.Angle;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MaxLinearSpeed_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var expected = 123.456f;

            //Act
            entity.MaxLinearSpeed = 123.456f;
            var actual = entity.MaxLinearSpeed;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MaxRotationSpeed_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var expected = 123.456f;

            //Act
            entity.MaxRotationSpeed = 123.456f;
            var actual = entity.MaxRotationSpeed;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LinearDeceleration_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            //Arrange
            _mockPhysicsBody.SetupProperty(p => p.LinearDeceleration);

            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 123.456f;

            //Act
            entity.LinearDeceleration = 123.456f;
            var actual = entity.LinearDeceleration;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LinearDeceleration_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            //Arrange
            _mockPhysicsBody.SetupProperty(p => p.LinearDeceleration);

            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            var expected = 123.456f;

            //Act
            entity.LinearDeceleration = 123.456f;
            var actual = entity.LinearDeceleration;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngularDeceleration_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 123.456f;

            //Act
            entity.AngularDeceleration = 123.456f;
            var actual = entity.AngularDeceleration;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AngularDeceleration_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var expected = 123.456f;

            //Act
            entity.AngularDeceleration = 123.456f;
            var actual = entity.AngularDeceleration;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvokingWithNullBody_ThrowsException()
        {
            //Arrange
            FakePhysicsBody nullPhysicsBody = null;

            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns(nullPhysicsBody);

            var entity = new DynamicEntity();

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() =>
            {
                entity.Update(It.IsAny<EngineTime>());
            });
        }


        [Fact]
        public void Update_WhenInvoking_ShouldHaveEntityStopAfterMovementInXDirection()
        {
            //Arrange
            _mockPhysicsBody.Object.LinearVelocityX = 100;
            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    _mockPhysicsBody.Object.LinearVelocityX += forceX;
                });

            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = false;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.StopMovement();
            entity.MoveRight();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvoking_ShouldHaveEntityStopAfterMovementInYDirection()
        {
            //Arrange
            _mockPhysicsBody.Object.LinearVelocityY = 100;
            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    _mockPhysicsBody.Object.LinearVelocityY = 0;
                });

            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            entity.Initialize();

            var expected = false;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.StopMovement();
            entity.MoveDown();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvokingWithMovingEntity_ShouldStopBodyWhenStopMovementIsInvoked()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object)
            {
                SpeedX = 10,
                SpeedY = 10,
                RotateSpeed = 100
            };

            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    _mockPhysicsBody.Object.LinearVelocityX = entity.IsEntityStopping ? 0 : forceX;
                    _mockPhysicsBody.Object.LinearVelocityY = entity.IsEntityStopping ? 0 : forceY;
                });

            _mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                _mockPhysicsBody.Object.AngularVelocity = entity.IsEntityStopping ? 0 : value;
            });

            entity.Initialize();
            var expected = false;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.MoveRight();
            entity.MoveDown();
            entity.RotateCW(1);
            entity.StopMovement();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveRight_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(123.456f, 0);

            //Act
            entity.MoveRight(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveRight_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(0.25f, 0);

            //Act
            entity.MoveRight();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveRight());
        }


        [Fact]
        public void MoveRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveRight(10));
        }


        [Fact]
        public void MoveLeft_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(-123.456f, 0);

            //Act
            entity.MoveLeft(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveLeft_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(-0.25f, 0);

            //Act
            entity.MoveLeft();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveLeft());
        }


        [Fact]
        public void MoveLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveLeft(10));
        }


        [Fact]
        public void MoveUp_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(0, -123.456f);

            //Act
            entity.MoveUp(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveUp_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(0, -0.25f);

            //Act
            entity.MoveUp();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveUp_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUp());
        }


        [Fact]
        public void MoveUp_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUp(10));
        }


        [Fact]
        public void MoveDown_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(0, 123.456f);

            //Act
            entity.MoveDown(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDown_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(0, 0.25f);

            //Act
            entity.MoveDown();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDown_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDown());
        }


        [Fact]
        public void MoveDown_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDown(10));
        }


        [Fact]
        public void MoveUpRight_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(-123.456f, 123.456f);

            //Act
            entity.MoveUpRight(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveUpRight_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = new Vector(-0.25f, 0.25f);

            //Act
            entity.MoveUpRight();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveUpRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpRight());
        }


        [Fact]
        public void MoveUpRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpRight(10));
        }


        [Fact]
        public void MoveUpLeft_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = new Vector(-123.456f, -123.456f);

            //Act
            entity.MoveUpLeft(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveUpLeft_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = new Vector(-0.25f, -0.25f);

            //Act
            entity.MoveUpLeft();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveUpLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpLeft());
        }


        [Fact]
        public void MoveUpLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpLeft(10));
        }


        [Fact]
        public void MoveDownRight_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = new Vector(123.456f, 123.456f);

            //Act
            entity.MoveDownRight(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDownRight_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = new Vector(0.25f, 0.25f);

            //Act
            entity.MoveDownRight();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDownRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownRight());
        }


        [Fact]
        public void MoveDownRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownRight(10));
        }


        [Fact]
        public void MoveDownLeft_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = new Vector(-123.456f, 123.456f);

            //Act
            entity.MoveDownLeft(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDownLeft_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var expected = new Vector(-0.25f, 0.25f);

            //Act
            entity.MoveDownLeft();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDownLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownLeft());
        }


        [Fact]
        public void MoveDownLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownLeft(10));
        }


        [Fact]
        public void MoveAtSetSpeed_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object)
            {
                SpeedX = 123.456f,
                SpeedY = 456.123f
            };

            entity.Initialize();
            var expected = new Vector(123.456f, 456.123f);

            //Act
            entity.MoveAtSetSpeed();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveAtSetSpeed_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveAtSetSpeed());
        }


        [Fact]
        public void MoveAtSetAngle_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object)
            {
                Angle = 45f
            };
            entity.Initialize();
            var expected = new Vector(0, -40);

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.MoveAtSetAngle(45);
            entity.Update(engineTime);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveAtSetAngle_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveAtSetAngle(10));
        }


        [Fact]
        public void RotateCW_WithNoParams_ProperlySetsAngularVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 1f;

            //Act
            entity.RotateCW();
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RotateCW_WithParam_ProperlySetsAngularVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = 123f;

            //Act
            entity.RotateCW(123f);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RotateCW_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCW());
        }


        [Fact]
        public void RotateCW_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCW(10));
        }


        [Fact]
        public void RotateCCW_WithNoParams_ProperlySetsAngularVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = -1f;

            //Act
            entity.RotateCCW();
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RotateCCW_WithParam_ProperlySetsAngularVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = -123f;

            //Act
            entity.RotateCCW(123f);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RotateCCW_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCCW());
        }


        [Fact]
        public void RotateCCW_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCCW(10));
        }


        [Fact]
        public void StopMovement_WhenInvoked_ProperlySetsLinearVelocity()
        {
            //Arrange
            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    _mockPhysicsBody.Object.LinearVelocityX = 0;
                    _mockPhysicsBody.Object.LinearVelocityY = 0;
                });

            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();
            var expected = Vector.Zero;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act
            entity.MoveDownRight();
            entity.StopMovement();
            entity.Update(engineTime);
            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void StopRotation_WhenInvoked_ProperlySetsAngularVelocity()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            entity.Initialize();
            var expected = 0f;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act
            entity.RotateCW();
            entity.StopRotation();
            entity.Update(engineTime);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void StopRotation_WhenInvokedWithNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(null);
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.StopRotation());
        }


        [Fact]
        public void DynamicEntity_WhenMoving_MaintainsMaxMovementLimit()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object)
            {
                MaxLinearSpeed = 20f
            };

            entity.Initialize();

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            var expected = 20f;

            //Act
            entity.MoveRight(40);
            entity.Update(engineTime);
            var actual = entity.Body.LinearVelocity.X;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void DynamicEntity_WhenMoving_MaintainsMaxRotationSpeed()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            _mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                entity.Angle = 1234;
            });
            entity.MaxRotationSpeed = 10;

            entity.Initialize();
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            var expected = 10f;

            //Act
            entity.RotateCW(20);
            entity.Update(engineTime);
            var actual = entity.MaxRotationSpeed;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Initialize_WhenInvokingWhileAlreadyInitialized_MaintainsMaxAngularLimit()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object)
            {
                MaxRotationSpeed = 10
            };

            _mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                entity.Angle = 1234;
            });

            entity.Initialize();

            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            var expected = 10f;

            //Act
            entity.RotateCW(It.IsAny<float>());
            entity.Update(engineTime);
            var actual = entity.MaxRotationSpeed;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose() => _mockPhysicsBody = null;
        #endregion


        #region Private Methods
        private static Texture CreateTexture()
        {
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);


            return new Texture(mockTexture.Object);
        }
        #endregion
    }
}

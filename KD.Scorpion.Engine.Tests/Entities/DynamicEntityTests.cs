using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Graphics;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Exceptions;
using KDScorpionEngineTests.Fakes;
using System;
using System.Linq;

namespace KDScorpionEngineTests.Entities
{
    [TestFixture]
    public class DynamicEntityTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithTextureAndPosition_CreatesAllBehaviors()
        {
            //Arrange
            var texture = CreateTexture();
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(texture, Vector.Zero);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.AreEqual(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Test]
        public void Ctor_WhenInvokingWithNoParams_CreatesAllBehaviors()
        {
            //Arrange
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity();
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.AreEqual(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Test]
        public void Ctor_WhenInvokingWithFrictionParam_CreatesAllBehaviors()
        {
            //Arrange
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(1f);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.AreEqual(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Test]
        public void Ctor_WhenInvokingWithPosition_CreatesAllBehaviors()
        {
            //Arrange
            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(Vector.Zero);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.AreEqual(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Test]
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
            var entity = new DynamicEntity(vertices, Vector.Zero);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.AreEqual(expectedTotalBehaviors, actualTotalBehaviors);
        }


        [Test]
        public void Ctor_WhenInvokingWithTextureAndVerticesAndPosition_CreatesAllBehaviors()
        {
            //Arrange
            var texture = CreateTexture();
            var vertices = new Vector[]
            {
                new Vector(11, 22),
                new Vector(33, 44),
                new Vector(55, 66),
                new Vector(77, 88)
            };

            int expectedTotalBehaviors = 6;

            //Act
            var entity = new DynamicEntity(texture, vertices, Vector.Zero);
            var actualTotalBehaviors = entity.Behaviors.Count;

            //Assert
            Assert.AreEqual(expectedTotalBehaviors, actualTotalBehaviors);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void IsMoving_WhenGettingValueWhenBodyIsNotMoving_ValueShouldReturnFalse()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = false;

            //Act
            var actual = entity.IsMoving;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsMoving_WhenGettingValueWhenBodyPerformingLinearMovement_ValueShouldReturnTrue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            entity.Body.LinearVelocity = new Vector(11, 22);

            var expected = true;

            //Act
            var actual = entity.IsMoving;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsMoving_WhenGettingValueWhenBodyPerformingAngularMovement_ValueShouldReturnTrue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            entity.RotateCW();

            var expected = true;

            //Act
            var actual = entity.IsMoving;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotationEnabled_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            var expected = true;

            //Act
            entity.RotationEnabled = true;
            var actual = entity.RotationEnabled;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Angle_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = 45.45f;

            //Act
            entity.Angle = 45.45f;
            var actual = entity.Angle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Angle_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            var expected = 45.45f;

            //Act
            entity.Angle = 45.45f;
            var actual = entity.Angle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MaxLinearSpeed_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            var expected = 123.456f;

            //Act
            entity.MaxLinearSpeed = 123.456f;
            var actual = entity.MaxLinearSpeed;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MaxRotationSpeed_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            var expected = 123.456f;

            //Act
            entity.MaxRotationSpeed = 123.456f;
            var actual = entity.MaxRotationSpeed;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LinearDeceleration_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = 123.456f;

            //Act
            entity.LinearDeceleration = 123.456f;
            var actual = entity.LinearDeceleration;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void LinearDeceleration_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            var expected = 123.456f;

            //Act
            entity.LinearDeceleration = 123.456f;
            var actual = entity.LinearDeceleration;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularDeceleration_WhenGettingAndSettingValueAfterInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = 123.456f;

            //Act
            entity.AngularDeceleration = 123.456f;
            var actual = entity.AngularDeceleration;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void AngularDeceleration_WhenGettingAndSettingValueBeforeInitialize_ReturnsCorrectValue()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            var expected = 123.456f;

            //Act
            entity.AngularDeceleration = 123.456f;
            var actual = entity.AngularDeceleration;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Update_WhenInvokingWithNullBody_ThrowsException()
        {
            //Arrange
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>()));
            mockPhysicsBody.Setup(m => m.ApplyLinearImpulse(It.IsAny<float>(), It.IsAny<float>()));

            FakePhysicsBody nullPhysicsBody = null;

            var mockPlugin = new Mock<IPluginLibrary>();
            mockPlugin.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns(nullPhysicsBody);

            PluginSystem.LoadPhysicsPluginLibrary(mockPlugin.Object);

            var entity = new DynamicEntity();

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() =>
            {
                entity.Update(It.IsAny<EngineTime>());
            });
        }


        [Test]
        public void Update_WhenInvoking_ShouldHaveEntityStopAfterMovementInXDirection()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = false;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.StopMovement();
            entity.MoveRight();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvoking_ShouldHaveEntityStopAfterMovementInYDirection()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = false;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.StopMovement();
            entity.MoveDown();
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvoking_ShouldHaveEntityStopAfterMovementInXAndYDirection()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero)
            {
                SpeedX = 100,
                SpeedY = 100,
                RotateSpeed = 100
            };
            entity.Initialize();
            var expected = false;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 0) };
            entity.StopMovement();
            entity.MoveRight();
            entity.MoveDown();
            entity.Update(engineTime);
            //entity.Update(engineTime);

            var actual = entity.IsMoving;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvokingWithMovingEntity_ShouldStopBodyWhenStopMovementIsInvoked()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero)
            {
                SpeedX = 10,
                SpeedY = 10,
                RotateSpeed = 100
            };
            entity.Initialize();
            var expected = false;

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.StopMovement();
            entity.MoveRight();
            entity.MoveDown();
            entity.RotateCW(1);
            entity.Update(engineTime);

            var actual = entity.IsMoving;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveRight_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(123.456f, 0);

            //Act
            entity.MoveRight(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveRight_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(0.25f, 0);

            //Act
            entity.MoveRight();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveRight());
        }


        [Test]
        public void MoveRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveRight(10));
        }


        [Test]
        public void MoveLeft_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-123.456f, 0);

            //Act
            entity.MoveLeft(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveLeft_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-0.25f, 0);

            //Act
            entity.MoveLeft();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveLeft());
        }


        [Test]
        public void MoveLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveLeft(10));
        }


        [Test]
        public void MoveUp_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(0, -123.456f);

            //Act
            entity.MoveUp(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveUp_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(0, -0.25f);

            //Act
            entity.MoveUp();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveUp_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUp());
        }


        [Test]
        public void MoveUp_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUp(10));
        }


        [Test]
        public void MoveDown_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(0, 123.456f);

            //Act
            entity.MoveDown(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveDown_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(0, 0.25f);

            //Act
            entity.MoveDown();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveDown_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDown());
        }


        [Test]
        public void MoveDown_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDown(10));
        }


        [Test]
        public void MoveUpRight_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-123.456f, 123.456f);

            //Act
            entity.MoveUpRight(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveUpRight_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-0.25f, 0.25f);

            //Act
            entity.MoveUpRight();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveUpRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpRight());
        }


        [Test]
        public void MoveUpRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpRight(10));
        }


        [Test]
        public void MoveUpLeft_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-123.456f, -123.456f);

            //Act
            entity.MoveUpLeft(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveUpLeft_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-0.25f, -0.25f);

            //Act
            entity.MoveUpLeft();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveUpLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpLeft());
        }


        [Test]
        public void MoveUpLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveUpLeft(10));
        }


        [Test]
        public void MoveDownRight_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(123.456f, 123.456f);

            //Act
            entity.MoveDownRight(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveDownRight_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(0.25f, 0.25f);

            //Act
            entity.MoveDownRight();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveDownRight_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownRight());
        }


        [Test]
        public void MoveDownRight_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownRight(10));
        }


        [Test]
        public void MoveDownLeft_WhenInvokingWithParam_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-123.456f, 123.456f);

            //Act
            entity.MoveDownLeft(123.456f);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveDownLeft_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = new Vector(-0.25f, 0.25f);

            //Act
            entity.MoveDownLeft();

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveDownLeft_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownLeft());
        }


        [Test]
        public void MoveDownLeft_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveDownLeft(10));
        }


        [Test]
        public void MoveAtSetSpeed_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero)
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveAtSetSpeed_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveAtSetSpeed());
        }


        [Test]
        public void MoveAtSetAngle_WhenInvokingNoParamsOverload_CorrectlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero)
            {
                Angle = 45f
            };
            entity.Initialize();
            var expected = new Vector(0, -40);

            //Act
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            entity.MoveAtSetAngle(50);
            entity.Update(engineTime);

            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveAtSetAngle_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.MoveAtSetAngle(10));
        }


        [Test]
        public void RotateCW_WithNoParams_ProperlySetsAngularVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = 1f;

            //Act
            entity.RotateCW();
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateCW_WithParam_ProperlySetsAngularVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = 123f;

            //Act
            entity.RotateCW(123f);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateCW_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCW());
        }


        [Test]
        public void RotateCW_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCW(10));
        }


        [Test]
        public void RotateCCW_WithNoParams_ProperlySetsAngularVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = -1f;

            //Act
            entity.RotateCCW();
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateCCW_WithParam_ProperlySetsAngularVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = -123f;

            //Act
            entity.RotateCCW(123f);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateCCW_WhenInvokingWithNoParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCCW());
        }


        [Test]
        public void RotateCCW_WhenInvokingWithParamAndNullBody_ThrowsException()
        {
            //Arrange
            var entity = new DynamicEntity(CreateTexture(), Vector.Zero);

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.RotateCCW(10));
        }


        [Test]
        public void StopMovement_WhenInvoked_ProperlySetsLinearVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = Vector.Zero;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act
            entity.MoveDownRight();
            entity.StopMovement();
            entity.Update(engineTime);
            var actual = entity.Body.LinearVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void StopRotation_WhenInvoked_ProperlySetsAngularVelocity()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            entity.Initialize();
            var expected = 0f;
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act
            entity.RotateCW();
            entity.StopRotation();
            entity.Update(engineTime);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void StopRotation_WhenInvokedWithNullBody_ThrowsException()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero);
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };

            //Act/Assert
            Assert.Throws<EntityNotInitializedException>(() => entity.StopRotation());
        }


        [Test]
        public void DynamicEntity_WhenMoving_MaintainsMaxMovementLimit()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero)
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DynamicEntity_WhenMoving_MaintainsMaxAngularLimit()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero)
            {
                MaxRotationSpeed = 10
            };
            entity.Initialize();
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            var expected = 10f;

            //Act
            entity.RotateCW(20);
            entity.Update(engineTime);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Initialize_WhenInvokingWhileAlreadyInitialized_MaintainsMaxAngularLimit()
        {
            //Arrange
            var texture = CreateTexture();
            var entity = new DynamicEntity(texture, Vector.Zero)
            {
                MaxRotationSpeed = 10
            };
            entity.Initialize();
            var engineTime = new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) };
            var expected = 10f;

            //Act
            entity.RotateCW(20);
            entity.Update(engineTime);
            var actual = entity.Body.AngularVelocity;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            if (ShouldSkipSetup())
                return;

            var mockPlugin = new Mock<IPluginLibrary>();
            mockPlugin.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[0]);
            });

            PluginSystem.LoadPhysicsPluginLibrary(mockPlugin.Object);
        }


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
        }
        #endregion


        #region Private Methods
        private static Texture CreateTexture()
        {
            var mockTexture = new Mock<ITexture>();
            mockTexture.SetupGet(m => m.Width).Returns(100);
            mockTexture.SetupGet(m => m.Height).Returns(50);


            return new Texture(mockTexture.Object);
        }


        private static bool ShouldSkipSetup()
        {
            var testCategories = TestContext.CurrentContext.Test.Properties["Category"];

            bool skipSetup = testCategories != null && testCategories.Contains("SKIP_SETUP");


            return skipSetup;
        }
        #endregion
    }
}

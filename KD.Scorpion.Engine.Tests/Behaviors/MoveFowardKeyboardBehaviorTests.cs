using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;

namespace KDScorpionEngineTests.Behaviors
{
    public class MoveFowardKeyboardBehaviorTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> _mockKeyboard;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #endregion


        #region Constructors
        public MoveFowardKeyboardBehaviorTests()
        {
            _mockKeyboard = new Mock<IKeyboard>();

            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(p => p.Angle);
            _mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                _mockPhysicsBody.Object.Angle += value;
            });
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void MoveFowardKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);
            var expected = KeyCodes.Space;

            //Act
            behavior.MoveFowardKey = KeyCodes.Space;
            var actual = behavior.MoveFowardKey;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RotateCWKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);
            var expected = KeyCodes.Space;

            //Act
            behavior.RotateCWKey = KeyCodes.Space;
            var actual = behavior.RotateCWKey;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void RotateCCWKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);
            var expected = KeyCodes.Space;

            //Act
            behavior.RotateCCWKey = KeyCodes.Space;
            var actual = behavior.RotateCCWKey;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsMovingFoward_WhenGettingValue_ReturnsTrue()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Up)).Returns(true);

            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = behavior.IsMovingForward;

            //Assert
            Assert.True(behavior.IsMovingForward);
        }


        [Fact]
        public void LinearSpeed_WhenSettingValue_ReturnsCorrectValue()
        {
            //Act
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(It.IsAny<IKeyboard>(), It.IsAny<DynamicEntity>())
            {
                LinearSpeed = 12
            };

            //Assert
            Assert.Equal(12, behavior.LinearSpeed);
        }
        #endregion


        #region Method Tests
        [Fact]
        public void Update_WhenInvoked_InvokesEntityRotateCW()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Right)).Returns(true);

            var entityXVertices = new[] { 10f, 20f, 30f };
            var entityYVertices = new[] { 10f, 20f, 30f };

            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity)
            {
                AngularSpeed = 10
            };

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = entity.Angle;

            //Assert
            Assert.Equal(10, actual);
        }


        [Fact]
        public void Update_WhenInvoked_InvokesEntityRotateCCW()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(true);

            var entity = new DynamicEntity(_mockPhysicsBody.Object);

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity)
            {
                AngularSpeed = -20
            };

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = entity.Angle;

            //Assert
            Assert.Equal(-20, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _mockKeyboard = null;
            _mockPhysicsBody = null;
        }
        #endregion
    }
}

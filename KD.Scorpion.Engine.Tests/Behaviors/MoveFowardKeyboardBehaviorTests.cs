using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Physics;

namespace KDScorpionEngineTests.Behaviors
{
    public class MoveFowardKeyboardBehaviorTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> _mockKeyboard;
        #endregion


        #region Constructors
        public MoveFowardKeyboardBehaviorTests() => _mockKeyboard = new Mock<IKeyboard>();
        #endregion


        #region Prop Tests
        [Fact]
        public void MoveFowardKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0, _mockKeyboard.Object);
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
            var entity = new DynamicEntity();
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0, _mockKeyboard.Object);
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
            var entity = new DynamicEntity();
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0, _mockKeyboard.Object);
            var expected = KeyCodes.Space;

            //Act
            behavior.RotateCCWKey = KeyCodes.Space;
            var actual = behavior.RotateCCWKey;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void IsMovingFoward_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Up)).Returns(true);

            var entity = new DynamicEntity()
            {
                Body = new PhysicsBody(new Mock<IPhysicsBody>().Object)
            };

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0, _mockKeyboard.Object);
            var expected = true;

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = behavior.IsMovingForward;

            //Assert
            Assert.Equal(expected, actual);
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

            var mockPhysicsBody = new Mock<IPhysicsBody>();
            mockPhysicsBody.SetupProperty(p => p.Angle);

            var entity = new DynamicEntity();
            mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                entity.Angle = 26;
            });

            entity.Body = new PhysicsBody(mockPhysicsBody.Object);

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), It.IsAny<float>(), _mockKeyboard.Object);
            var expected = 26;

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = entity.Angle;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Update_WhenInvoked_InvokesEntityRotateCCW()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Left)).Returns(true);

            var entity = new DynamicEntity();
            var mockPhysicsBody = new Mock<IPhysicsBody>();
            mockPhysicsBody.SetupProperty(p => p.Angle);

            mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                entity.Angle = -10;
            });

            entity.Body = new PhysicsBody(mockPhysicsBody.Object);

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), It.IsAny<float>(), _mockKeyboard.Object);
            var expected = -10;

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = entity.Angle;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _mockKeyboard = null;
        }
        #endregion
    }
}

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
    public class MovementByKeyboardBehaviorTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> _mockKeyboard;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #endregion


        #region Constructors
        public MovementByKeyboardBehaviorTests()
        {
            _mockKeyboard = new Mock<IKeyboard>();
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(m => m.X);
            _mockPhysicsBody.SetupProperty(m => m.Y);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Right);
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>());

            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>())).
                Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    entity.Position = new Vector(10, It.IsAny<float>());
                });

            entity.Body = new PhysicsBody(_mockPhysicsBody.Object);

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _mockKeyboard.Object);

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(10, entity.Position.X);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveLeftBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Left);
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_mockPhysicsBody.Object)
            };

            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>())).
                Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    entity.Position = new Vector(20, It.IsAny<float>());
                });

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _mockKeyboard.Object);

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(20, entity.Position.X);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveUpBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Up);
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_mockPhysicsBody.Object)
            };

            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>())).
                Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    entity.Position = new Vector(It.IsAny<float>(), 15);
                });

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _mockKeyboard.Object);

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(15, entity.Position.Y);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveDownBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Down);
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_mockPhysicsBody.Object)
            };

            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>())).
                Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    entity.Position = new Vector(It.IsAny<float>(), 25);
                });

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _mockKeyboard.Object);

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(25, entity.Position.Y);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void MoveUpKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, It.IsAny<float>(), _mockKeyboard.Object);
            var expected = KeyCodes.W;

            //Act
            behavior.MoveUpKey = KeyCodes.W;
            var actual = behavior.MoveUpKey;

            //AssertIt.IsAny<Vector[]>(), It.IsAny<Vector>()
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDownKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>());

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _mockKeyboard.Object);
            var expected = KeyCodes.S;

            //Act
            behavior.MoveDownKey = KeyCodes.S;
            var actual = behavior.MoveDownKey;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveLeftKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>());

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _mockKeyboard.Object);
            var expected = KeyCodes.S;

            //Act
            behavior.MoveLeftKey = KeyCodes.S;
            var actual = behavior.MoveLeftKey;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveRightKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>());

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _mockKeyboard.Object);
            var expected = KeyCodes.S;

            //Act
            behavior.MoveRightKey = KeyCodes.S;
            var actual = behavior.MoveRightKey;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            _mockKeyboard = null;
            _mockPhysicsBody = null;
        }
        #endregion


        #region Private Methods
        private void SetKeyboardKey(KeyCodes key) => _mockKeyboard.Setup(m => m.IsKeyDown(key)).Returns(true);
        #endregion
    }
}

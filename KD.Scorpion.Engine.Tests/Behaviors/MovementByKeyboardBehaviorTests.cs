using System;
using Moq;
using Xunit;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using Raptor.Input;
using Raptor.Plugins;
using Raptor;

namespace KDScorpionEngineTests.Behaviors
{
    /// <summary>
    /// Unit tests to test the <see cref="MovementByKeyboardBehavior{T}"/> class.
    /// </summary>
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
            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>())).
                Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    _mockPhysicsBody.Object.X += forceX;
                    _mockPhysicsBody.Object.Y += forceY;
                });
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Right);
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity)
            {
                LinearSpeed = 1234
            };

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(1234, entity.Position.X);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveLeftBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Left);
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity)
            {
                LinearSpeed = -5678
            };

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(-5678, entity.Position.X);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveUpBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Up);
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity)
            {
                LinearSpeed = -1478
            };

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(-1478, entity.Position.Y);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveDownBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Down);
            var entity = new DynamicEntity(_mockPhysicsBody.Object);
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity)
            {
                LinearSpeed = 9876
            };

            //Act
            behavior.Update(new EngineTime());

            //Assert
            Assert.Equal(9876, entity.Position.Y);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void MoveUpKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var entity = new DynamicEntity();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);
            var expected = KeyCodes.W;

            //Act
            behavior.MoveUpKey = KeyCodes.W;
            var actual = behavior.MoveUpKey;

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDownKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>());

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);
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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);
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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(_mockKeyboard.Object, entity);
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

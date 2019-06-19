using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using PluginSystem;
using KDScorpionEngineTests.Fakes;

namespace KDScorpionEngineTests.Behaviors
{
    public class MovementByKeyboardBehaviorTests : IDisposable
    {
        #region Private Fields
        private Mock<IKeyboard> _mockCoreKeyboard;
        #endregion


        #region Constructors
        public MovementByKeyboardBehaviorTests()
        {
            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            _mockCoreKeyboard = new Mock<IKeyboard>();
            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(_mockCoreKeyboard.Object);
            Plugins.EnginePlugins = mockEnginePluginLibrary.Object;


            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });
            Plugins.PhysicsPlugins = mockPhysicsPluginLibrary.Object;
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Right);
            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            entity.Initialize();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 10f);
            var expected = 10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.X;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveLeftBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Left);
            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            entity.Initialize();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 10f);
            var expected = -10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.X;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveUpBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Up);
            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            entity.Initialize();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 10f);
            var expected = -10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.Y;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveDownBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Down);
            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            entity.Initialize();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 10f);
            var expected = 10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.Y;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void MoveUpKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
            var expected = KeyCodes.W;

            //Act
            behavior.MoveUpKey = KeyCodes.W;
            var actual = behavior.MoveUpKey;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void MoveDownKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetKeyboardKey(It.IsAny<KeyCodes>());
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
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
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
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
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
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
            Plugins.EnginePlugins = null;
            Plugins.PhysicsPlugins = null;
        }
        #endregion


        #region Private Methods
        private void SetKeyboardKey(KeyCodes key) => _mockCoreKeyboard.Setup(m => m.IsKeyDown(key)).Returns(true);
        #endregion
    }
}

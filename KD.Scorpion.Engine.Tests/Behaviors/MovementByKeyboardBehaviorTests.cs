using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Physics;
using PluginSystem;
using KDScorpionEngine;

namespace KDScorpionEngineTests.Behaviors
{
    public class MovementByKeyboardBehaviorTests : IDisposable
    {
        private Vector[] _vertices;
        #region Private Fields
        private Mock<IKeyboard> _mockKeyboard;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        private Mock<IPluginLibrary> _mockEnginePluginLib;
        private Mock<IPluginLibrary> _mockPhysicsPluginLib;
        private Plugins _plugins;
        #endregion


        #region Constructors
        public MovementByKeyboardBehaviorTests()
        {
            Dispose();

            _vertices = new Vector[]
            {
                Vector.Zero,
                Vector.Zero,
                Vector.Zero
            };

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

            _mockEnginePluginLib = new Mock<IPluginLibrary>();
            _mockEnginePluginLib.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(_mockKeyboard.Object);

            _mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            _mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns<object[]>((ctrParams) => _mockPhysicsBody.Object);

            _plugins = new Plugins()
            {
                EnginePlugins = _mockEnginePluginLib.Object,
                PhysicsPlugins = _mockPhysicsPluginLib.Object
            };

            EnginePluginSystem.SetPlugins(_plugins);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Right);
            var entity = new DynamicEntity(_vertices, It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_vertices, It.IsAny<Vector>())
            };
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 1234);

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
            var entity = new DynamicEntity(_vertices, It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_vertices, It.IsAny<Vector>())
            };
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 5678);

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
            var entity = new DynamicEntity(_vertices, It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_vertices, It.IsAny<Vector>())
            };
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 1478);

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
            var entity = new DynamicEntity(_vertices, It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_vertices, It.IsAny<Vector>())
            };
            entity.Initialize();

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, 9876);

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
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, It.IsAny<float>());
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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>());
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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>());
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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>());
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
            _mockEnginePluginLib = null;

            _mockPhysicsBody = null;
            _mockPhysicsPluginLib = null;

            _plugins = null;

            EnginePluginSystem.ClearPlugins();
        }
        #endregion


        #region Private Methods
        private void SetKeyboardKey(KeyCodes key) => _mockKeyboard.Setup(m => m.IsKeyDown(key)).Returns(true);
        #endregion
    }
}

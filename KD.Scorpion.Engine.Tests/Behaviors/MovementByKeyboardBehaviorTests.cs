using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using PluginSystem;
using KDScorpionEngine.Physics;
using KDScorpionEngine;

namespace KDScorpionEngineTests.Behaviors
{
    public class MovementByKeyboardBehaviorTests : IDisposable
    {
        #region Private Fields
        private Plugins_NEW _plugins;
        private Mock<IKeyboard> _mockCoreKeyboard;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #endregion


        #region Constructors
        public MovementByKeyboardBehaviorTests()
        {
            _plugins = new Plugins_NEW();

            _mockCoreKeyboard = new Mock<IKeyboard>();

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(_mockCoreKeyboard.Object);

            _plugins.EnginePlugins = mockEnginePluginLibrary.Object;

            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();

            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(m => m.X);
            _mockPhysicsBody.SetupProperty(m => m.Y);

            _plugins.PhysicsPlugins = mockPhysicsPluginLibrary.Object;
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            //Arrange
            SetKeyboardKey(KeyCodes.Right);
            var entity = new DynamicEntity(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            {
                Body = new PhysicsBody(_mockPhysicsBody.Object)
            };

            _mockPhysicsBody.Setup(m => m.ApplyForce(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>())).
                Callback<float, float, float, float>((forceX, forceY, worldLocationX, worldLocationY) =>
                {
                    entity.Position = new Vector(10, It.IsAny<float>());
                });

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _plugins.EnginePlugins);

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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _plugins.EnginePlugins);

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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _plugins.EnginePlugins);

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

            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), _plugins.EnginePlugins);

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
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, It.IsAny<float>(), _plugins.EnginePlugins);
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
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, It.IsAny<float>(), _plugins.EnginePlugins);
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
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, It.IsAny<float>(), _plugins.EnginePlugins);
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
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, It.IsAny<float>(), _plugins.EnginePlugins);
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
            _mockCoreKeyboard = null;
            _mockPhysicsBody = null;
            _plugins.EnginePlugins = null;
            _plugins.PhysicsPlugins = null;
            _plugins = null;
            EnginePluginSystem.ClearPlugin();
        }
        #endregion


        #region Private Methods
        private void SetKeyboardKey(KeyCodes key) => _mockCoreKeyboard.Setup(m => m.IsKeyDown(key)).Returns(true);
        #endregion
    }
}

using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using PluginSystem;
using KDScorpionCore.Physics;

namespace KDScorpionEngineTests.Behaviors
{
    public class MoveFowardKeyboardBehaviorTests : IDisposable
    {
        private Vector[] _vertices;
        #region Private Fields
        private Mock<IKeyboard> _mockKeyboard;
        private Mock<IPluginLibrary> _mockEnginePluginLib;
        private Mock<IPhysicsBody> _mockPhysicsBody;
        private Mock<IPluginLibrary> _mockPhysicsPluginLib;
        private Plugins _plugins;
        #endregion


        #region Constructors
        public MoveFowardKeyboardBehaviorTests()
        {
            Dispose();

            _vertices = new Vector[3]
            {
                new Vector(0, 0),
                new Vector(0, 0),
                new Vector(0, 0)
            };

            _mockKeyboard = new Mock<IKeyboard>();
            _mockEnginePluginLib = new Mock<IPluginLibrary>();
            _mockEnginePluginLib.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(_mockKeyboard.Object);

            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(p => p.Angle);
            _mockPhysicsBody.Setup(m => m.ApplyAngularImpulse(It.IsAny<float>())).Callback<float>((value) =>
            {
                _mockPhysicsBody.Object.Angle += value;
            });
            _mockPhysicsPluginLib = new Mock<IPluginLibrary>();
            _mockPhysicsPluginLib.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns<object[]>((ctrParams) => _mockPhysicsBody.Object);

            _plugins = new Plugins()
            {
                EnginePlugins = _mockEnginePluginLib.Object,
                PhysicsPlugins = _mockPhysicsPluginLib.Object
            };

            CorePluginSystem.SetPlugins(_plugins);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void MoveFowardKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
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
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
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
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
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

            var entity = new DynamicEntity()
            {
                Body = new PhysicsBody(_vertices, Vector.Zero)
            };

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = behavior.IsMovingForward;

            //Assert
            Assert.True(behavior.IsMovingForward);
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

            var entity = new DynamicEntity
            {
                Body = new PhysicsBody(_vertices, It.IsAny<Vector>())
            };

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), 10);

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

            var entity = new DynamicEntity();
            entity.Body = new PhysicsBody(_vertices, It.IsAny<Vector>());

            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, It.IsAny<float>(), -20);
            var expected = -20;

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
            _mockEnginePluginLib = null;
            _mockPhysicsBody = null;
            _mockPhysicsPluginLib = null;
            _plugins = null;
            CorePluginSystem.ClearPlugins();
        }
        #endregion
    }
}

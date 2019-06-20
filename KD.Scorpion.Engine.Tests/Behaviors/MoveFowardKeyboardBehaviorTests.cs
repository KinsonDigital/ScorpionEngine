using System;
using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngineTests.Fakes;
using PluginSystem;
using KDScorpionEngine;

namespace KDScorpionEngineTests.Behaviors
{
    public class MoveFowardKeyboardBehaviorTests : IDisposable
    {
        #region Private Fields
        private Plugins_NEW _plugins;
        private Mock<IKeyboard> _mockKeyboard;
        #endregion


        #region Constructors
        public MoveFowardKeyboardBehaviorTests()
        {
            _plugins = new Plugins_NEW();
            _mockKeyboard = new Mock<IKeyboard>();

            var entityXVertices = new[] { 10f, 20f, 30f };
            var entityYVertices = new[] { 10f, 20f, 30f };

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(_mockKeyboard.Object);
            _plugins.EnginePlugins = mockEnginePluginLibrary.Object;

            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1]);
            });
            _plugins.PhysicsPlugins = mockPhysicsPluginLibrary.Object;

            EnginePluginSystem.SetPlugin(_plugins);
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
        public void IsMovingFoward_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            _mockKeyboard.Setup(m => m.IsKeyDown(KeyCodes.Up)).Returns(true);

            var entity = new DynamicEntity();
            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
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

            var entity = new DynamicEntity();
            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 10);
            var expected = 10;

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
            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, -10);
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
            _plugins = null;
            EnginePluginSystem.ClearPlugin();
        }
        #endregion
    }
}

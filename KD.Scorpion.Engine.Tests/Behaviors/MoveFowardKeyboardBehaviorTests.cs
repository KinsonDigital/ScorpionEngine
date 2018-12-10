using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngineTests.Fakes;
using System;

namespace KDScorpionEngineTests.Behaviors
{
    [TestFixture]
    public class MoveFowardKeyboardBehaviorTests
    {
        #region Prop Tests
        [Test]
        public void MoveFowardKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity();
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
            var expected = InputKeys.Space;

            //Act
            behavior.MoveFowardKey = InputKeys.Space;
            var actual = behavior.MoveFowardKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateCWKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity();
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
            var expected = InputKeys.Space;

            //Act
            behavior.RotateCWKey = InputKeys.Space;
            var actual = behavior.RotateCWKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RotateCCWKey_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity();
            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
            var expected = InputKeys.Space;

            //Act
            behavior.RotateCCWKey = InputKeys.Space;
            var actual = behavior.RotateCCWKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsMovingFoward_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new DynamicEntity();
            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 0);
            var expected = true;

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = behavior.IsMovingForward;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Update_WhenInvoked_InvokesEntityRotateCW()
        {
            //Arrange
            TearDown();
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Right)).Returns(true);

            var entityXVertices = new[] { 10f, 20f, 30f };
            var entityYVertices = new[] { 10f, 20f, 30f };

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockKeyboard.Object);

            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParms) =>
            {
                return new FakePhysicsBody((float[])ctorParms[0], (float[])ctorParms[1]);
            });

            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLibrary.Object);
            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLibrary.Object);

            var entity = new DynamicEntity();
            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, 10);
            var expected = 10;

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = entity.Angle;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Update_WhenInvoked_InvokesEntityRotateCCW()
        {
            //Arrange
            var entity = new DynamicEntity();
            entity.Initialize();

            var behavior = new MoveFowardKeyboardBehavior<DynamicEntity>(entity, 0, -10);
            var expected = -10;

            //Act
            behavior.Update(new EngineTime() { ElapsedEngineTime = new TimeSpan(0, 0, 0, 0, 16) });
            var actual = entity.Angle;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            var mockKeyboard = new Mock<IKeyboard>();
            mockKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Up)).Returns(true);
            mockKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);
            mockKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Right)).Returns(true);

            var entityXVertices = new[] { 10f, 20f, 30f };
            var entityYVertices = new[] { 10f, 20f, 30f };

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockKeyboard.Object);

            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParms) =>
            {
                return new FakePhysicsBody((float[])ctorParms[0], (float[])ctorParms[1]);
            });

            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLibrary.Object);
            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLibrary.Object);
        }


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
        }
        #endregion
    }
}

using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Behaviors;
using ScorpionEngine.Entities;
using ScorpionEngine.Input;
using ScorpionEngine.Physics;
using ScorpionEngine.Tests.Fakes;

namespace ScorpionEngine.Tests.Behaviors
{
    [TestFixture]
    public class KeyboardMovementBehaviorTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            //Arrange
            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Right)).Returns(true);

            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLibrary.Object);
            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLibrary.Object);

            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            var behavior = new KeyboardMovementBehavior<DynamicEntity>(entity, 10f);
            var expected = 10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.X;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_CreatesMoveLeftBehavior()
        {
            //Arrange
            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Left)).Returns(true);

            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLibrary.Object);
            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLibrary.Object);

            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            var behavior = new KeyboardMovementBehavior<DynamicEntity>(entity, 10f);
            var expected = -10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.X;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_CreatesMoveUpBehavior()
        {
            //Arrange
            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Up)).Returns(true);

            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLibrary.Object);
            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLibrary.Object);

            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            var behavior = new KeyboardMovementBehavior<DynamicEntity>(entity, 10f);
            var expected = -10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.Y;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_CreatesMoveDownBehavior()
        {
            //Arrange
            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });

            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)InputKeys.Down)).Returns(true);

            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLibrary.Object);
            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLibrary.Object);

            var entity = new DynamicEntity(new Vector[0], Vector.Zero);
            var behavior = new KeyboardMovementBehavior<DynamicEntity>(entity, 10f);
            var expected = 10;

            //Act
            behavior.Update(new EngineTime());
            var actual = entity.Position.Y;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

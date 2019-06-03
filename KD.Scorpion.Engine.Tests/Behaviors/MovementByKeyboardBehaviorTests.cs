using Moq;
using NUnit.Framework;
using KDScorpionCore;
using KDScorpionCore.Input;
using KDScorpionCore.Plugins;
using KDScorpionEngine.Behaviors;
using KDScorpionEngine.Entities;
using KDScorpionEngineTests.Fakes;
using PluginSystem;

namespace KDScorpionEngineTests.Behaviors
{
    [TestFixture]
    public class MovementByKeyboardBehaviorTests
    {
        private Mock<IKeyboard> _mockCoreKeyboard;
        #region Constructor Tests
        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            _mockCoreKeyboard = new Mock<IKeyboard>();
            var mockPluginFactory = new Mock<IPluginFactory>();
            mockPluginFactory.Setup(m => m.CreatePhysicsBody(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });
            mockPluginFactory.Setup(m => m.CreateKeyboard()).Returns(_mockCoreKeyboard.Object);

            Plugins.LoadPluginFactory(mockPluginFactory.Object);
        }


        [TearDown]
        public void TearDown() => Plugins.UnloadPluginFactory();
        #endregion


        #region Private Methods
        private void SetKeyboardKey(KeyCodes key) => _mockCoreKeyboard.Setup(m => m.IsKeyDown(key)).Returns(true);
        #endregion
    }
}

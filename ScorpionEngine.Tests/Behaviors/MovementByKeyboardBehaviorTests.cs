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
    public class MovementByKeyboardBehaviorTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_CreatesMoveRightBehavior()
        {
            //Arrange
            SetupKeyboard(InputKeys.Right);
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
            SetupKeyboard(InputKeys.Left);
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
            SetupKeyboard(InputKeys.Up);
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
            SetupKeyboard(InputKeys.Down);
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
        public void TypeOfMovement_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            var mockKeyboard = new Mock<IKeyboard>();
            var mockEnginPluginLib = new Mock<IPluginLibrary>();
            mockEnginPluginLib.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockKeyboard.Object);

            PluginSystem.LoadEnginePluginLibrary(mockEnginPluginLib.Object);

            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
            var expected = MovementType.FourAxis;

            //Act
            behavior.TypeOfMovement = MovementType.FourAxis;
            var actual = behavior.TypeOfMovement;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveUpKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetupKeyboard(It.IsAny<InputKeys>());
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
            var expected = InputKeys.W;

            //Act
            behavior.MoveUpKey = InputKeys.W;
            var actual = behavior.MoveUpKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveDownKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetupKeyboard(It.IsAny<InputKeys>());
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
            var expected = InputKeys.S;

            //Act
            behavior.MoveDownKey = InputKeys.S;
            var actual = behavior.MoveDownKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveLeftKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetupKeyboard(It.IsAny<InputKeys>());
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
            var expected = InputKeys.S;

            //Act
            behavior.MoveLeftKey = InputKeys.S;
            var actual = behavior.MoveLeftKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void MoveRightKey_WhenGettingAndSettingValue_CorrectlySetsValue()
        {
            //Arrange
            SetupKeyboard(It.IsAny<InputKeys>());
            var mockEntity = new Mock<DynamicEntity>();
            var behavior = new MovementByKeyboardBehavior<DynamicEntity>(mockEntity.Object, 1);
            var expected = InputKeys.S;

            //Act
            behavior.MoveRightKey = InputKeys.S;
            var actual = behavior.MoveRightKey;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Public Methods
        [SetUp]
        public void Setup()
        {
            var mockPhysicsPluginLibrary = new Mock<IPluginLibrary>();
            mockPhysicsPluginLibrary.Setup(m => m.LoadPlugin<IPhysicsBody>(It.IsAny<object[]>())).Returns((object[] ctorParams) =>
            {
                return new FakePhysicsBody((float[])ctorParams[0], (float[])ctorParams[1], (float)ctorParams[2], (float)ctorParams[3]);
            });
            
            PluginSystem.LoadPhysicsPluginLibrary(mockPhysicsPluginLibrary.Object);
        }


        [TearDown]
        public void TearDown()
        {
            PluginSystem.ClearPlugins();
        }
        #endregion


        #region Private Methods
        private void SetupKeyboard(InputKeys key)
        {
            var mockCoreKeyboard = new Mock<IKeyboard>();
            mockCoreKeyboard.Setup(m => m.IsKeyDown((int)key)).Returns(true);
            var mockEnginePluginLibrary = new Mock<IPluginLibrary>();
            mockEnginePluginLibrary.Setup(m => m.LoadPlugin<IKeyboard>()).Returns(mockCoreKeyboard.Object);

            PluginSystem.LoadEnginePluginLibrary(mockEnginePluginLibrary.Object);
        }
        #endregion
    }
}

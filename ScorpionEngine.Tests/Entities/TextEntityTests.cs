using Moq;
using NUnit.Framework;
using ScorpionCore;
using ScorpionCore.Plugins;
using ScorpionEngine.Entities;
using ScorpionEngine.Tests.Fakes;
using System.Drawing;

namespace ScorpionEngine.Tests.Entities
{
    [TestFixture]
    public class TextEntityTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_CorrectlySetsTextProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.Red, Color.Red, Vector.Zero);
            var expected = "text";

            //Act
            var actual = entity.Text;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_CorrectlySetsForeColorProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.FromArgb(11, 22, 33), Color.Red, Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            var actual = entity.ForeColor;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_CorrectlySetsBackColorProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.Red, Color.FromArgb(11, 22, 33), Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            var actual = entity.BackColor;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvoking_CorrectlySetsPositionProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.Red, Color.Red, new Vector(11, 22));
            entity.Initialize();
            var expected = new Vector(11, 22);

            //Act
            var actual = entity.Position;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Test]
        public void Text_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new TextEntity("", Color.Red, Color.Red, Vector.Zero);
            var expected = "hello world";

            //Act
            entity.Text = "hello world";
            var actual = entity.Text;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ForeColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new TextEntity("", Color.Red, Color.Red, Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            entity.ForeColor = Color.FromArgb(11, 22, 33);
            var actual = entity.ForeColor;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void BackColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new TextEntity("", Color.Red, Color.Red, Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            entity.BackColor = Color.FromArgb(11, 22, 33);
            var actual = entity.BackColor;

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
    }
}

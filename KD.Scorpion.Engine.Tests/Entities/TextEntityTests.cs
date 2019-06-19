using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionEngine.Entities;
using KDScorpionEngineTests.Fakes;
using System.Drawing;
using PluginSystem;
using KDScorpionCore.Plugins;
using System;

namespace KDScorpionEngineTests.Entities
{
    public class TextEntityTests : IDisposable
    {
        #region Constructors
        public TextEntityTests()
        {
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
        public void Ctor_WhenInvoking_CorrectlySetsTextProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.Red, Color.Red, Vector.Zero);
            var expected = "text";

            //Act
            var actual = entity.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvoking_CorrectlySetsForeColorProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.FromArgb(11, 22, 33), Color.Red, Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            var actual = entity.ForeColor;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvoking_CorrectlySetsBackColorProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.Red, Color.FromArgb(11, 22, 33), Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            var actual = entity.BackColor;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvoking_CorrectlySetsPositionProp()
        {
            //Arrange
            var entity = new TextEntity("text", Color.Red, Color.Red, new Vector(11, 22));
            entity.Initialize();
            var expected = new Vector(11, 22);

            //Act
            var actual = entity.Position;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Text_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new TextEntity("", Color.Red, Color.Red, Vector.Zero);
            var expected = "hello world";

            //Act
            entity.Text = "hello world";
            var actual = entity.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ForeColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new TextEntity("", Color.Red, Color.Red, Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            entity.ForeColor = Color.FromArgb(11, 22, 33);
            var actual = entity.ForeColor;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BackColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var entity = new TextEntity("", Color.Red, Color.Red, Vector.Zero);
            var expected = Color.FromArgb(11, 22, 33);

            //Act
            entity.BackColor = Color.FromArgb(11, 22, 33);
            var actual = entity.BackColor;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose() => Plugins.PhysicsPlugins = null;
        #endregion
    }
}

using Moq;
using Xunit;
using KDScorpionCore;
using KDScorpionEngine.Entities;
using System.Drawing;
using KDScorpionCore.Plugins;
using System;
using KDScorpionCore.Physics;

namespace KDScorpionEngineTests.Entities
{
    public class TextEntityTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #endregion


        #region Constructors
        public TextEntityTests() => _mockPhysicsBody = new Mock<IPhysicsBody>();
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
            _mockPhysicsBody.SetupProperty(p => p.X);
            _mockPhysicsBody.SetupProperty(p => p.Y);

            var entity = new TextEntity("text", Color.Red, Color.Red, new Vector(11, 22))
            {
                Body = new PhysicsBody(It.IsAny<Vector[]>(), It.IsAny<Vector>())
            };
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
        public void Dispose() => _mockPhysicsBody = null;
        #endregion
    }
}

using Moq;
using Xunit;
using KDScorpionEngine.Entities;
using System;
using Raptor.Graphics;
using Raptor.Plugins;

namespace KDScorpionEngineTests.Entities
{
    /// <summary>
    /// Unit tests to test the <see cref="TextEntity"/> class.
    /// </summary>
    public class TextEntityTests : IDisposable
    {
        #region Private Fields
        private Mock<IPhysicsBody> _mockPhysicsBody;
        #endregion


        #region Constructors
        public TextEntityTests()
        {
            _mockPhysicsBody = new Mock<IPhysicsBody>();
            _mockPhysicsBody.SetupProperty(p => p.X);
            _mockPhysicsBody.SetupProperty(p => p.Y);
        }
        #endregion


        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CorrectlySetsUpEntity()
        {
            // Arrange
            var entity = new TextEntity(_mockPhysicsBody.Object);

            // Assert
            Assert.Equal(string.Empty, entity.Text);
            Assert.Equal(new GameColor(255, 0, 0, 0), entity.ForeColor);
            Assert.Equal(new GameColor(0, 0, 0, 0), entity.BackColor);
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void Text_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new TextEntity(_mockPhysicsBody.Object);
            var expected = "hello world";

            // Act
            entity.Text = "hello world";
            var actual = entity.Text;

            // Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ForeColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new TextEntity(_mockPhysicsBody.Object);
            var expected = new GameColor(255, 11, 22, 33);

            // Act
            entity.ForeColor = new GameColor(255, 11, 22, 33);
            var actual = entity.ForeColor;

            // Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void BackColor_WhenGettingAndSettingValue_ReturnsCorrectValue()
        {
            // Arrange
            var entity = new TextEntity(_mockPhysicsBody.Object);
            var expected = new GameColor(255, 11, 22, 33);

            // Act
            entity.BackColor = new GameColor(255, 11, 22, 33);
            var actual = entity.BackColor;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Public Methods
        public void Dispose() => _mockPhysicsBody = null;
        #endregion
    }
}

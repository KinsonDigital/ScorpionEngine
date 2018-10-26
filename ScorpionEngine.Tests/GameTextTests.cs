using Moq;
using ScorpionCore;
using ScorpionEngine.Graphics;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class GameTextTests
    {
        #region Prop Tests
        [Fact]
        public void Text_WhenSettingValue_ProperlyReturnsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.Setup(m => m.Text).Returns("Hello World");
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = "Hello World";

            //Act
            var actual = gameText.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Text_WhenSettingValue_ProperlySetsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.SetupProperty(m => m.Text, "");
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = "Hello World";

            //Act
            gameText.Text = "Hello World";
            var actual = gameText.Text;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Width_WhenSettingValue_ProperlyReturnsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.Setup(m => m.Width).Returns(40);
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = 40;

            //Act
            var actual = gameText.Width;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Width_WhenSettingValue_ProperlySetsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.SetupProperty(m => m.Width, 0);
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = 10;

            //Act
            gameText.Width = 10;
            var actual = gameText.Width;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Height_WhenSettingValue_ProperlyReturnsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.Setup(m => m.Height).Returns(40);
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = 40;

            //Act
            var actual = gameText.Height;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Height_WhenSettingValue_ProperlySetsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.SetupProperty(m => m.Height, 0);
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = 10;

            //Act
            gameText.Height = 10;
            var actual = gameText.Height;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Color_WhenSettingValue_ProperlySetsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.SetupProperty(m => m.Color, new byte[] { 11, 22, 33, 44 });
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = new GameColor(11, 22, 33, 44);

            //Act
            gameText.Color = new GameColor(11, 22, 33, 44);
            var actual = gameText.Color;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

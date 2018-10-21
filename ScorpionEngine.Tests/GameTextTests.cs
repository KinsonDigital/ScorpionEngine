using Moq;
using ScorpionCore;
using ScorpionEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #endregion
    }
}

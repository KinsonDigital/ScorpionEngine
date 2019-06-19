﻿using Moq;
using KDScorpionCore.Graphics;
using KDScorpionCore;
using Xunit;

namespace KDScorpionCoreTests.Graphics
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
        public void Color_WhenSettingValue_ProperlySetsInternalValue()
        {
            //Arrange
            var mockInternalText = new Mock<IText>();
            mockInternalText.SetupProperty(m => m.Color, new GameColor(0, 0, 0, 0));
            var gameText = new GameText()
            {
                InternalText = mockInternalText.Object
            };
            var expected = new GameColor(44, 11, 22, 33);

            //Act
            gameText.Color = new GameColor(44, 11, 22, 33);
            var actual = gameText.Color;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion


        #region Overloaded Operator Tests
        [Fact]
        public void AddOperator_WhenAddingTwoObjects_ReturnsCorrectValue()
        {
            //Arrange
            var mockTextA = new Mock<IText>();
            mockTextA.SetupProperty(m => m.Text);

            var mockTextB = new Mock<IText>();
            mockTextB.SetupProperty(m => m.Text);

            var textA = new GameText()
            {
                InternalText = mockTextA.Object,
                Text = "Hello "
            };
            var textB = new GameText()
            {
                InternalText = mockTextB.Object,
                Text = "World"
            };
            var expected = "Hello World";

            //Act
            var actual = textA + textB;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AddOperator_WhenAddingObjectAndString_ReturnsCorrectValue()
        {
            //Arrange
            var mockText = new Mock<IText>();
            mockText.SetupProperty(m => m.Text);

            var textA = new GameText()
            {
                InternalText = mockText.Object,
                Text = "Hello "
            };
            var expected = "Hello World";

            //Act
            var actual = textA + "World";

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void AddOperator_WhenAddingStringAndObject_ReturnsCorrectValue()
        {
            //Arrange
            var mockText = new Mock<IText>();
            mockText.SetupProperty(m => m.Text);

            var textB = new GameText()
            {
                InternalText = mockText.Object,
                Text = "World"
            };
            var expected = "Hello World";

            //Act
            var actual = "Hello " + textB;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

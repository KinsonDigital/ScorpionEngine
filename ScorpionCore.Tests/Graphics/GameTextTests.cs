using Moq;
using NUnit.Framework;
using ScorpionCore.Graphics;

namespace ScorpionCore.Tests.Graphics
{
    public class GameTextTests
    {
        #region Prop Tests
        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }
        #endregion


        #region Overloaded Operator Tests
        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }


        [Test]
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
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

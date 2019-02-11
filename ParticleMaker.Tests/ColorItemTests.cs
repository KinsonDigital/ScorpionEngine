using NUnit.Framework;
using System.Windows.Media;

namespace ParticleMaker.Tests
{
    [TestFixture]
    public class ColorItemTests
    {
        #region Prop Tests
        [Test]
        public void Id_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var colorItem = new ColorItem();
            var expected = 1234;

            //Act
            colorItem.Id = 1234;
            var actual = colorItem.Id;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ColorBrush_WhenGettingSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var colorItem = new ColorItem();
            var expected = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33));

            //Act
            colorItem.ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33));
            var actual = colorItem.ColorBrush;

            //Assert
            Assert.AreEqual(expected.Color, actual.Color);
        }
        #endregion


        #region Methods Tests
        [Test]
        public void ToString_WhenInvoking_ReturnsCorrectStringValue()
        {
            //Arrange
            var colorItem = new ColorItem
            {
                Id = 1234,
                ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33))
            };

            var expected = "R:11, G:22, B:33, A:255";

            //Act
            var actual = colorItem.ToString();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Equals_WhenInvokingAgainstNullObject_ReturnsFalse()
        {
            //Arrange
            var colorItem = new ColorItem
            {
                Id = 1234,
                ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33))
            };

            ColorItem otherItem = null;

            //Act
            var actual = colorItem.Equals(otherItem);

            //Assert
            Assert.IsFalse(actual);
        }


        [Test]
        public void Equals_WhenInvokingAgainstEqualObject_ReturnsTrue()
        {
            //Arrange
            var colorItemA = new ColorItem
            {
                Id = 1234,
                ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33))
            };

            var colorItemB = new ColorItem
            {
                Id = 1234,
                ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33))
            };

            //Act
            var actual = colorItemA.Equals(colorItemB);

            //Assert
            Assert.IsTrue(actual);
        }


        [Test]
        public void Equals_WhenInvokingAgainstNotEqualObject_ReturnsTrue()
        {
            //Arrange
            var colorItemA = new ColorItem
            {
                Id = 1234,
                ColorBrush = new SolidColorBrush(Color.FromArgb(255, 11, 22, 33))
            };

            var colorItemB = new ColorItem
            {
                Id = 1234,
                ColorBrush = new SolidColorBrush(Color.FromArgb(33, 22, 11, 255))
            };

            //Act
            var actual = colorItemA.Equals(colorItemB);

            //Assert
            Assert.False(actual);
        }


        [Test]
        public void GetHashCode_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var colorItem = new ColorItem
            {
                Id = 1234,
                ColorBrush = new SolidColorBrush(Color.FromArgb(11, 22, 33, 44))
            };

            var expected = -1172984920;

            //Act
            var actual = colorItem.GetHashCode();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

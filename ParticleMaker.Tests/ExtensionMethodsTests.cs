using Xunit;
using System.Windows.Media;
using NETColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace ParticleMaker.Tests
{
    public class ExtensionMethodsTests
    {
        #region Method Tests
        [Fact]
        public void ToRadians_WhenInvoking_ReturnsCorrectValue()
        {
            //Arrange
            var angle = 45.45f;
            var expected = 0.79325211f;

            //Act
            var actual = angle.ToRadians();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ToColorItem_WhenInvoked_ReturnsCorrectColorItem()
        {
            //Arrange
            var gameColor = NETColor.FromArgb(44, 11, 22, 33);
            var expected = new ColorItem()
            {
                Id = 0,
                ColorBrush = new SolidColorBrush(MediaColor.FromArgb(44, 11, 22, 33))
            };

            //Act
            var actual = gameColor.ToColorItem();

            //Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.ColorBrush.Color, actual.ColorBrush.Color);
        }


        [Fact]
        public void Join_WhenInvokingWithItemExclude_ReturnsCorrectValue()
        {
            //Arrange
            var items = new[]
            {
                "C:",
                "parent-folder",
                "child-folder",
                "file-a.txt"
            };
            var expected = @"C:\parent-folder\child-folder";

            //Act
            var actual = items.Join("file-a.txt");

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Join_WhenInvokingWithNoExclude_ReturnsCorrectValue()
        {
            //Arrange
            var items = new[]
            {
                "C:",
                "parent-folder",
                "child-folder",
                "file-a.txt"
            };
            var expected = @"C:\parent-folder\child-folder\file-a.txt";

            //Act
            var actual = items.Join();

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void GetPropertyNames_WhenInvoked_ReturnsCorrectNames()
        {
            //Arrange
            var testObject = new
            {
                PropertyA = "prop-A",
                PropertyB = 1234,
                PropertyC = 1234.1234f
            };

            var expected = new[]
            {
                "PropertyA",
                "PropertyB",
                "PropertyC"
            };

            //Act
            var actual = testObject.GetPropertyNames();

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

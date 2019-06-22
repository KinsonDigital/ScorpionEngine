using Xunit;
using ParticleMaker.UserControls;
using System.Windows.Media;

namespace ParticleMaker.Tests.UserControls
{
    public class ExtensionMethodsTests
    {
        #region Method Tests
        [Fact]
        public void ToNegativeBrush_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var color = Color.FromArgb(255, 100, 100, 100);
            var expected = Color.FromArgb(255, 155, 155, 155);

            //Act
            var actual = color.ToNegativeBrush().Color;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ToNegative_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var brush = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
            var expected = Color.FromArgb(255, 155, 155, 155);

            //Act
            var actual = brush.ToNegative().Color;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

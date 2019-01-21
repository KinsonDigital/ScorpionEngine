using NUnit.Framework;
using ParticleMaker.UserControls;
using System.Windows.Media;

namespace ParticleMaker.Tests.UserControls
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        #region Method Tests
        [Test]
        public void ToNegativeBrush_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var color = Color.FromArgb(255, 100, 100, 100);
            var expected = Color.FromArgb(255, 155, 155, 155);

            //Act
            var actual = color.ToNegativeBrush().Color;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ToNegative_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var brush = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
            var expected = Color.FromArgb(255, 155, 155, 155);

            //Act
            var actual = brush.ToNegative().Color;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

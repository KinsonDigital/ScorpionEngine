using KDScorpionCore;
using Xunit;

namespace KDScorpionCoreTests
{
    public class ExtensionMethodTests
    {
        [Fact]
        public void RotateAround_WhenInvoked_ReturnsCorrectResult()
        {
            //Arrange
            var vectorToRotate = new Vector(0, 0);
            var origin = new Vector(5, 5);
            var angle = 13f;
            var expected = new Vector(1.25290489f, -0.996605873f);

            //Act
            var actual = vectorToRotate.RotateAround(origin, angle);

            //Assert
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
        }


        [Fact]
        public void RotateAround_WhenInvokedWithClockwiseFalse_ReturnsCorrectResult()
        {
            //Arrange
            var vectorToRotate = new Vector(0, 0);
            var origin = new Vector(5, 5);
            var angle = 45f;
            var expected = new Vector(-2.07106781f, 5f);

            //Act
            var actual = vectorToRotate.RotateAround(origin, angle, false);

            //Assert
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
        }
    }
}

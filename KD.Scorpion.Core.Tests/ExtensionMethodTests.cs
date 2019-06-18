using KDScorpionCore;
using NUnit.Framework;

namespace KDScorpionCoreTests
{
    [TestFixture]
    public class ExtensionMethodTests
    {
        [Test]
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
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
        }


        [Test]
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
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
        }
    }
}

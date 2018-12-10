using NUnit.Framework;
using KDScorpionCore;
using KDScorpionEngine.Utils;

namespace KDScorpionEngine.Tests.Utils
{
    public class ToolsTests
    {
        [Test]
        public void RotateAround_WhenInvoked_ReturnsCorrectResult()
        {
            //Arrange
            var vectorToRotate = new Vector(0, 0);
            var origin = new Vector(5, 5);
            var angle = 13f;
            var expected = new Vector(1.25290489f, -0.9966054f);

            //Act
            var actual = Tools.RotateAround(vectorToRotate, origin, angle);

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
            var actual = Tools.RotateAround(vectorToRotate, origin, angle, false);

            //Assert
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
        }
    }
}

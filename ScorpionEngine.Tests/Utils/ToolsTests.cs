using NUnit.Framework;
using ScorpionEngine.Physics;
using ScorpionEngine.Utils;


namespace ScorpionEngine.Tests.Utils
{
    public class ToolsTests
    {
        [Test]
        public void RotateAround_WhenInvoked_ReturnsCorrectResult()
        {
            //Arrange
            var vectorToRotate = new Vector(0, 0);
            var origin = new Vector(5, 5);
            var angle = 0.6161012f;
            var expected = new Vector(3.8086f, -1.969976f);

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
            var angle = 0.6161012f;
            var expected = new Vector(-1.969976f, 3.8086f);

            //Act
            var actual = Tools.RotateAround(vectorToRotate, origin, angle, false);

            //Assert
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
        }
    }
}

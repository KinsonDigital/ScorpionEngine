using ScorpionEngine.Physics;
using ScorpionEngine.Utils;
using Xunit;

namespace ScorpionEngine.Tests.Utils
{
    public class ToolsTests
    {
        [Fact]
        public void RotateAround_WhenInvoked_ReturnsValueResult()
        {
            //Arrange
            var vectorToRotate = new Vector(0, 0);
            var origin = new Vector(5, 5);
            var angle = 0.6161012f;
            var expected = new Vector(3.8086f, -1.969976f);

            //Act
            var actual = Tools.RotateAround(vectorToRotate, origin, angle);

            //Assert
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
        }
    }
}

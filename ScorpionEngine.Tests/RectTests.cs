using NUnit.Framework;
using ScorpionEngine.Physics;


namespace ScorpionEngine.Tests
{
    public class RectTests
    {
        [Test]
        public void Contains_ShouldReturnCorrectValueWithXAndY()
        {
            //Arrange
            var rect = new Rect(0, 0, 10, 10);
            var expected = true;

            //Act
            var actual = rect.Contains(5, 5);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        [Test]
        public void Contains_ShouldReturnCorrectValueWithVector()
        {
            //Arrange
            var rect = new Rect(0, 0, 10, 10);
            var vector = new Vector(5, 5);
            var expected = true;

            //Act
            var actual = rect.Contains(vector);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        [Test]
        public void Contains_ShouldReturnCorrectValueWithRect()
        {
            //Arrange
            var rectA = new Rect(0, 0, 10, 10);
            var rectB = new Rect(5, 5, 10, 10);

            var expected = true;

            //Act
            var actual = rectA.Contains(rectB);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        [Test]
        public void Empty_ShouldReturnEmptyRectangle()
        {
            //Act
            var expected = 0;
            var actual = Rect.Empty;

            //Assert
            Assert.AreEqual(actual.X, expected);
            Assert.AreEqual(actual.Y, expected);
            Assert.AreEqual(actual.Width, expected);
            Assert.AreEqual(actual.Height, expected);
        }
    }
}

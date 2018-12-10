using NUnit.Framework;
using KDScorpionCore;

namespace ScorpionEngine.Tests
{
    [TestFixture]
    public class RectTests
    {
        #region Prop Tests
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
        #endregion


        #region Method Tests
        [Test]
        public void Contains_WhenContainingXAndY_ReturnTrue()
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
        public void Contains_WhenNotContainingXAndY_ReturnFalse()
        {
            //Arrange
            var rect = new Rect(0, 0, 10, 10);
            var expected = false;

            //Act
            var actual = rect.Contains(50, 50);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        [Test]
        public void Contains_WhenContainingVector_ReturnsTrue()
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
        public void Contains_WhenContainingTopLeftCorner_ReturnsTrue()
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
        public void Contains_WhenContainingTopRightCorner_ReturnsTrue()
        {
            //Arrange
            var rectA = new Rect(0, 0, 10, 10);
            var rectB = new Rect(-5, 5, 10, 10);

            var expected = true;

            //Act
            var actual = rectA.Contains(rectB);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        [Test]
        public void Contains_WhenContainingBottomLeftCorner_ReturnsTrue()
        {
            //Arrange
            var rectA = new Rect(0, 0, 10, 10);
            var rectB = new Rect(5,- 5, 10, 10);

            var expected = true;

            //Act
            var actual = rectA.Contains(rectB);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        [Test]
        public void Contains_WhenContainingBottomRightCorner_ReturnsTrue()
        {
            //Arrange
            var rectA = new Rect(0, 0, 10, 10);
            var rectB = new Rect(-5, -5, 10, 10);

            var expected = true;

            //Act
            var actual = rectA.Contains(rectB);

            //Assert
            Assert.AreEqual(actual, expected);
        }
        #endregion
    }
}

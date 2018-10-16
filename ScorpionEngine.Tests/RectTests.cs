using ScorpionEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class RectTests
    {
        [Fact]
        public void Contains_ShouldReturnCorrectValueWithXAndY()
        {
            //Arrange
            var rect = new Rect(0, 0, 10, 10);
            var expected = true;

            //Act
            var actual = rect.Contains(5, 5);

            //Assert
            Assert.Equal(actual, expected);
        }


        [Fact]
        public void Contains_ShouldReturnCorrectValueWithVector()
        {
            //Arrange
            var rect = new Rect(0, 0, 10, 10);
            var vector = new Vector(5, 5);
            var expected = true;

            //Act
            var actual = rect.Contains(vector);

            //Assert
            Assert.Equal(actual, expected);
        }


        [Fact]
        public void Contains_ShouldReturnCorrectValueWithRect()
        {
            //Arrange
            var rectA = new Rect(0, 0, 10, 10);
            var rectB = new Rect(5, 5, 10, 10);

            var expected = true;

            //Act
            var actual = rectA.Contains(rectB);

            //Assert
            Assert.Equal(actual, expected);
        }


        [Fact]
        public void Empty_ShouldReturnEmptyRectangle()
        {
            //Act
            var expected = 0;
            var actual = Rect.Empty;

            //Assert
            Assert.Equal(actual.X, expected);
            Assert.Equal(actual.Y, expected);
            Assert.Equal(actual.Width, expected);
            Assert.Equal(actual.Height, expected);
        }
    }
}

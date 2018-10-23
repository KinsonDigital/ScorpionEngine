using ScorpionEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScorpionEngine.Tests
{
    public class GameColorTests
    {
        #region Prop Tests
        [Fact]
        public void Red_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(11, 0, 0, 0);
            var expected = 11;

            //Act
            var actual = gameColor.Red;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Green_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(0, 22, 0, 0);
            var expected = 22;

            //Act
            var actual = gameColor.Green;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Blue_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(0, 0, 33, 0);
            var expected = 33;

            //Act
            var actual = gameColor.Blue;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Alpha_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(0, 0, 0, 44);
            var expected = 44;

            //Act
            var actual = gameColor.Alpha;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

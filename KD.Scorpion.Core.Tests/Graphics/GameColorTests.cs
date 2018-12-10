using NUnit.Framework;
using KDScorpionCore.Graphics;

namespace KDScorpionCoreTests.Graphics
{
    public class GameColorTests
    {
        #region Prop Tests
        [Test]
        public void Red_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(11, 0, 0, 0);
            var expected = 11;

            //Act
            var actual = gameColor.Red;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Green_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(0, 22, 0, 0);
            var expected = 22;

            //Act
            var actual = gameColor.Green;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Blue_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(0, 0, 33, 0);
            var expected = 33;

            //Act
            var actual = gameColor.Blue;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Alpha_WhenGettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var gameColor = new GameColor(0, 0, 0, 44);
            var expected = 44;

            //Act
            var actual = gameColor.Alpha;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

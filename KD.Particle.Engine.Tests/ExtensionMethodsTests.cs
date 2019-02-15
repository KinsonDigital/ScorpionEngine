using KDParticleEngine;
using KDScorpionCore.Graphics;
using NUnit.Framework;
using System;

namespace KDParticleEngineTests
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        #region Method Tests
        [Test]
        public void Next_WhenInvoked_ReturnsValueWithinMinAndMax()
        {
            //Arrange
            var random = new Random();
            var expected = true;

            //Act
            var randomNum = random.Next(50f, 100f);
            var actual = randomNum >= 50f && randomNum <= 100f;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ToGameColor_WhenInvoked_ReturnsParticleColor()
        {
            //Arrange
            var expected = new GameColor(44, 11, 22, 33);

            //Act
            var actual = new ParticleColor(44, 11, 22, 33).ToGameColor();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ToGameColors_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var expected = new GameColor[]
            {
                new GameColor(1, 2, 3, 4),
                new GameColor(11, 22, 33, 44)
            };

            //Act
            var actual = new ParticleColor[] { new ParticleColor(1, 2, 3, 4), new ParticleColor(11, 22, 33, 44) }.ToGameColors();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ToParticleColor_WhenInvoked_ReturnsParticleColor()
        {
            //Arrange
            var expected = new ParticleColor(44, 11, 22, 33);

            //Act
            var actual = new GameColor(44, 11, 22, 33).ToParticleColor();

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ToParticleColor_WhenInvoked_ReturnsCorrectValue()
        {
            //Arrange
            var expected = new ParticleColor[]
            {
                new ParticleColor(1, 2, 3, 4),
                new ParticleColor(11, 22, 33, 44)
            };

            //Act
            var actual = new GameColor[] { new GameColor(1, 2, 3, 4), new GameColor(11, 22, 33, 44) }.ToParticleColors();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

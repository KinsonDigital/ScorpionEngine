using KDParticleEngine;
using Xunit;
using System;

namespace KDParticleEngineTests
{
    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    public class ExtensionMethodsTests
    {
        #region Method Tests
        [Fact]
        public void Next_WhenInvokedWithMinLessThanMax_ReturnsValueWithinMinAndMax()
        {
            //Arrange
            var random = new Random();
            var expected = true;

            //Act
            var randomNum = random.Next(50f, 100f);
            var actual = randomNum >= 50f && randomNum <= 100f;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Next_WhenInvokedWithMinMoreThanMax_ReturnsMaxValue()
        {
            //Arrange
            var random = new Random();
            var expected = 98f;

            //Act
            var actual = random.Next(124f, 98f);

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

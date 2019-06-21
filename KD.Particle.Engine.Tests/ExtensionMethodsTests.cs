using KDParticleEngine;
using Xunit;
using System;

namespace KDParticleEngineTests
{
    public class ExtensionMethodsTests
    {
        #region Method Tests
        [Fact]
        public void Next_WhenInvoked_ReturnsValueWithinMinAndMax()
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
        #endregion
    }
}

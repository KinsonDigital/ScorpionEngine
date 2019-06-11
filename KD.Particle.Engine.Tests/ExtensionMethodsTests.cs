using KDParticleEngine;
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
        #endregion
    }
}

using NUnit.Framework;
using ParticleMaker.Services;
using System;

namespace ParticleMaker.Tests.Services
{
    [TestFixture]
    public class LogTests
    {
        #region Prop Tests
        [Test]
        public void Data_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var log = new Log();
            var expected = "test-data";

            //Act
            log.Data = "test-data";
            var actual = log.Data;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void DateTime_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var log = new Log();
            var expected = new DateTime(2019, 2, 20);
            
            //Act
            log.DateTimeStamp = new DateTime(2019, 2, 20);
            var actual = log.DateTimeStamp;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ErrorNumber_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var log = new Log();
            var expected = 1234;

            //Act
            log.ErrorNumber = 1234;
            var actual = log.ErrorNumber;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void IsError_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var log = new Log();
            var expected = true;

            //Act
            log.IsError = true;
            var actual = log.IsError;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

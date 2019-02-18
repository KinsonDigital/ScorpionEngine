﻿using NUnit.Framework;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class InvalidCommandActionParamTypeExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoked_ProperlyCreatesExceptionMessage()
        {
            //Arrange
            var expected = "The test-method method parameter 'test-param' not a valid type.";

            var exception = new InvalidCommandActionParamTypeException("test-method", "test-param");

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
﻿using Xunit;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Exceptions;

namespace KDScorpionEngineTests.Exceptions
{
    public class MissingVerticesExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"An {nameof(Entity)} must have vertices and at least a total of 3 vertices.";

            //Act
            var exception = new MissingVerticesException();
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithMessageParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"Unit testing is awesome!";

            //Act
            var exception = new MissingVerticesException("Unit testing is awesome!");
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

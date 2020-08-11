// <copyright file="IdAlreadyExistsExceptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Exceptions
{
    using KDScorpionEngine.Exceptions;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="IdAlreadyExistsException"/> class.
    /// </summary>
    public class IdAlreadyExistsExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokedWithNoParams_ProperlySetsExceptionMessage()
        {
            // Arrange
            var expected = "That ID already exists.";

            // Act
            var exception = new IdAlreadyExistsException();
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_WhenInvokedWithSceneId_ProperlySetsExceptionMessage()
        {
            // Arrange
            var expected = "The ID '1234' already exists.";

            // Act
            var exception = new IdAlreadyExistsException(1234);
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_WhenInvokedMessage_ProperlySetsExceptionMessage()
        {
            // Arrange
            var expected = "The is a custome message.";

            // Act
            var exception = new IdAlreadyExistsException("The is a custome message.");
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

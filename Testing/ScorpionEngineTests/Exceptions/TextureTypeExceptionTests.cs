// <copyright file="TextureTypeExceptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Exceptions
{
    using KDScorpionEngine.Exceptions;
    using Xunit;

    public class TextureTypeExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParams_ProperlySetsExceptionMessage()
        {
            // Arrange
            var expected = "The texture type is invalid.";

            // Act
            var exception = new TextureTypeException();
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_WhenInvokingWithMessage_ProperlySetsExceptionMessage()
        {
            // Arrange
            var expected = "This is a custom message.";

            // Act
            var exception = new TextureTypeException("This is a custom message.");
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

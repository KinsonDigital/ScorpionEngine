﻿// <copyright file="NameNotFoundExceptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests.Exceptions
{
    using KDScorpionEngine.Exceptions;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="NameNotFoundException"/> class.
    /// </summary>
    public class NameNotFoundExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParams_ProperlySetsExceptionMessage()
        {
            // Arrange
            var expected = "A scene with that name does not exist.";

            // Act
            var exception = new NameNotFoundException();
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
            var exception = new NameNotFoundException("This is a custom message.");
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

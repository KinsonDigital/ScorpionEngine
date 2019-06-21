using Xunit;
using ParticleMaker.Exceptions;
using System;

namespace ParticleMaker.Tests.Exceptions
{
    public class IllegalProjectNameExceptionTests : Exception
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var expected = "Illegal project name.  Cannot not use characters \\/:*?\"<>|";

            //Act
            var exception = new IllegalProjectNameException();
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var expected = "The project 'test*-pro|ject'.  Cannot not use characters \\/:*?\"<>|";

            //Act
            var exception = new IllegalProjectNameException("test*-pro|ject");
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

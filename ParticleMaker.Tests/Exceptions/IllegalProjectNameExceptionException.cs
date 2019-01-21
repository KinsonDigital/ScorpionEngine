using NUnit.Framework;
using ParticleMaker.Exceptions;
using System;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class IllegalProjectNameExceptionException : Exception
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var expected = "Illegal project name.  Cannot not use characters \\/:*?\"<>|";

            //Act
            var exception = new IllegalProjectNameException();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var expected = "The project 'test*-pro|ject'.  Cannot not use characters \\/:*?\"<>|";

            //Act
            var exception = new IllegalProjectNameException("test*-pro|ject");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

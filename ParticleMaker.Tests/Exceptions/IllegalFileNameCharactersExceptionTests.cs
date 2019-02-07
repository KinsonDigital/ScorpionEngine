using NUnit.Framework;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class IllegalFileNameCharactersExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_CreatesCorrectErrorMessage()
        {
            //Arrange
            var exception = new IllegalFileNameCharactersException();
            var expected = "The file name contains illegal characters.  The following characters are not aloud. \n'\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.'";

            //Act
            var act = exception.Message;

            //Assert
            Assert.AreEqual(expected, act);
        }
        #endregion
    }
}

using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    /// <summary>
    /// Unit tests to test the <see cref="IllegalFileNameCharactersException"/> class.
    /// </summary>
    public class IllegalFileNameCharactersExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatesCorrectErrorMessage()
        {
            //Arrange
            var exception = new IllegalFileNameCharactersException();
            var expected = "The file name contains illegal characters.  The following characters are not aloud. \n'\\', '/', ':', '*', '?', '\"', '<', '>', '|', '.'";

            //Act
            var act = exception.Message;

            //Assert
            Assert.Equal(expected, act);
        }
        #endregion
    }
}

using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    /// <summary>
    /// Unit tests to test the <see cref="InvalidCommandActionParamTypeException"/> class.
    /// </summary>
    public class InvalidCommandActionParamTypeExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoked_ProperlyCreatesExceptionMessage()
        {
            //Arrange
            var expected = "The test-method method parameter 'test-param' not a valid type.";

            var exception = new InvalidCommandActionParamTypeException("test-method", "test-param");

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

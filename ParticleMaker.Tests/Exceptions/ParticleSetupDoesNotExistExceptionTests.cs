using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    /// <summary>
    /// Unit tests to test the <see cref="ParticleSetupDoesNotExistException"/> class.
    /// </summary>
    public class ParticleSetupDoesNotExistExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleSetupDoesNotExistException();
            var expected = "The particle setup does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleSetupDoesNotExistException("test-setup");
            var expected = "The particle setup with the name 'test-setup' does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

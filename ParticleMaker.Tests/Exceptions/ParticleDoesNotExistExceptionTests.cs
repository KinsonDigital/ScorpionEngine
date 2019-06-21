using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    public class ParticleDoesNotExistExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokedWithNoParams_BuildsCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleDoesNotExistException();
            var expected = "The particle does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokedWithParams_BuildsCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleDoesNotExistException("test-particle", @"C:\temp\missing-particle-file.png");
            var expected = @"The particle 'test-particle' at the path 'C:\temp\missing-particle-file.png' does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

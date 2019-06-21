using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    public class ParticleSetupAlreadyExistsExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenCreatingWithNoArgument_CreatesValidMessage()
        {
            //Arrange
            var expected = "The particle setup already exists.";

            //Arrange
            var exception = new ParticleSetupAlreadyExistsException();
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenCreatingWithArgument_CreatesValidMessage()
        {
            //Arrange
            var expected = "The particle setup with the name 'test-setup' already exists.";

            //Arrange
            var exception = new ParticleSetupAlreadyExistsException("test-setup");
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

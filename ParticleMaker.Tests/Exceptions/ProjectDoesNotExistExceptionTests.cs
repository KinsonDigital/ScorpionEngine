using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    public class ProjectDoesNotExistExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ProjectDoesNotExistException();
            var expected = "The project does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ProjectDoesNotExistException("test-project");
            var expected = "The project 'test-project' does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

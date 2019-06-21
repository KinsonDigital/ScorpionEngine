using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    public class ProjectAlreadyExistsExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ProjectAlreadyExistsException();
            var expected = "The project already exists.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ProjectAlreadyExistsException("test-project");
            var expected = "The project 'test-project' already exists.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

using Xunit;
using KDScorpionEngine.Exceptions;

namespace KDScorpionEngineTests.Exceptions
{
    /// <summary>
    /// Unit tests to test the <see cref="IdNotFoundException"/> class.
    /// </summary>
    public class IdNotFoundExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParams_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "The ID has not been found.";

            //Act
            var exception = new IdNotFoundException();
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithSceneId_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "An ID with the number '9876' has not been found.";

            //Act
            var exception = new IdNotFoundException(9876);
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithMessage_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "This is a custom message.";

            //Act
            var exception = new IdNotFoundException("This is a custom message.");
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

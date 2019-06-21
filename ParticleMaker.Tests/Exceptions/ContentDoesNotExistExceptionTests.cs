using Xunit;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    public class ContentDoesNotExistExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingNoParamOverload_SetsCorrectExceptionMsg()
        {
            //Arrange
            var expected = "The content item does not exist in the root content directory.";

            //Act
            var exception = new ContentDoesNotExistException();
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithParamOverload_SetsCorrectExceptionMsg()
        {
            //Arrange
            var contentItemName = "CONTENT-ITEM";
            var expected = $"The content item '{contentItemName}' does not exist in the root content directory.";

            //Act
            var exception = new ContentDoesNotExistException(contentItemName);
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

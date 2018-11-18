using NUnit.Framework;
using ScorpionEngine.Exceptions;

namespace ScorpionEngine.Tests.Exceptions
{
    [TestFixture]
    public class IdNotFoundExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParams_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "The id has not been found.";

            //Act
            var exception = new IdNotFoundException();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithSceneId_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "An id with the number '9876' has not been found.";

            //Act
            var exception = new IdNotFoundException(9876);
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithMessage_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "This is a custom message.";

            //Act
            var exception = new IdNotFoundException("This is a custom message.");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

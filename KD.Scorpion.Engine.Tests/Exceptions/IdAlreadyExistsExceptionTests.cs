using NUnit.Framework;
using KDScorpionEngine.Exceptions;

namespace KDScorpionEngineTests.Exceptions
{
    [TestFixture]
    public class IdAlreadyExistsExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokedWithNoParams_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "That id already exists.";

            //Act
            var exception = new IdAlreadyExistsException();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokedWithSceneId_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "The id '1234' already exists.";

            //Act
            var exception = new IdAlreadyExistsException(1234);
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokedMessage_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "The is a custome message.";

            //Act
            var exception = new IdAlreadyExistsException("The is a custome message.");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

using NUnit.Framework;
using KDScorpionEngine.Exceptions;

namespace KDScorpionEngineTests.Exceptions
{
    [TestFixture]
    public class NameNotFoundExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParams_ProperlySetsExceptionMessage()
        {
            //Arrange
            var expected = "A scene with that name does not exist.";

            //Act
            var exception = new NameNotFoundException();
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
            var exception = new NameNotFoundException("This is a custom message.");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

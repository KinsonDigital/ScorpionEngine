using NUnit.Framework;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Exceptions;

namespace KDScorpionEngineTests.Exceptions
{
    [TestFixture]
    public class EntityAlreadyInitializedExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"{nameof(Entity)} is already initialized.  Invocation must be performed before using the {nameof(Entity)}.{nameof(Entity.Initialize)}() method.";

            //Act
            var exception = new EntityAlreadyInitializedException();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithMessageParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"I Love Kristen!!";

            //Act
            var exception = new EntityAlreadyInitializedException("I Love Kristen!!");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

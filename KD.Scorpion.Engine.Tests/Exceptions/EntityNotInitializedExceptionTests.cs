using NUnit.Framework;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Exceptions;
using KDScorpionEngine.Physics;

namespace KDScorpionEngine.Tests.Exceptions
{
    [TestFixture]
    public class EntityNotInitializedExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParams_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"{nameof(Entity)} not initialized.  Must be initialized before being added to a {nameof(PhysicsWorld)} using {nameof(Entity)}.{nameof(Entity.Initialize)}() method.";

            //Act
            var exception = new EntityNotInitializedException();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithMessageParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = "This is an exception message";

            //Act
            var exception = new EntityNotInitializedException("This is an exception message");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

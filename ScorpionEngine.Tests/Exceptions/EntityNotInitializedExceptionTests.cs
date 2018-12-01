using NUnit.Framework;
using ScorpionEngine.Entities;
using ScorpionEngine.Exceptions;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Tests.Exceptions
{
    [TestFixture]
    public class EntityNotInitializedExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParams_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"{nameof(Entity)} not initialized.  Must be initialized before being added to a {nameof(PhysicsWorld)}";

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

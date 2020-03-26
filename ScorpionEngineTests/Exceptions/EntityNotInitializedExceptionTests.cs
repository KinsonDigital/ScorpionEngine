using Xunit;
using KDScorpionEngine.Entities;
using KDScorpionEngine.Exceptions;
using Raptor.Physics;

namespace KDScorpionEngineTests.Exceptions
{
    /// <summary>
    /// Unit tests to test the <see cref="EntityNotInitializedException"/> class.
    /// </summary>
    public class EntityNotInitializedExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParams_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = $"{nameof(Entity)} not initialized.  Must be initialized before being added to a {nameof(PhysicsWorld)} using {nameof(Entity)}.{nameof(Entity.Initialize)}() method.";

            //Act
            var exception = new EntityNotInitializedException();
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Ctor_WhenInvokingWithMessageParam_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = "This is an exception message";

            //Act
            var exception = new EntityNotInitializedException("This is an exception message");
            var actual = exception.Message;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

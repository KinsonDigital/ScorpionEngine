namespace KDScorpionEngineTests.Exceptions
{
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Exceptions;
    using Xunit;

    /// <summary>
    /// Unit tests to test the <see cref="EntityAlreadyInitializedException"/> class.
    /// </summary>
    public class EntityAlreadyInitializedExceptionTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvokingWithNoParam_CorrectlySetsExceptionMessage()
        {
            // Arrange
            var expected = $"{nameof(Entity)} is already initialized.  Invocation must be performed before using the {nameof(Entity)}.{nameof(Entity.Initialize)}() method.";

            // Act
            var exception = new EntityAlreadyInitializedException();
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_WhenInvokingWithMessageParam_CorrectlySetsExceptionMessage()
        {
            // Arrange
            var expected = $"I Love Kristen!!";

            // Act
            var exception = new EntityAlreadyInitializedException("I Love Kristen!!");
            var actual = exception.Message;

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

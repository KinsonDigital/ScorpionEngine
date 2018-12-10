using NUnit.Framework;
using KDScorpionEngine.Exceptions;

namespace KDScorpionEngineTests.Exceptions
{
    [TestFixture]
    public class SceneNotFoundExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithSceneId_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var sceneId = 10;
            var expected = "The scene with the id of '10' was not found.";

            //Act
            var exception = new SceneNotFoundException(sceneId);
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithMessage_CorrectlySetsExceptionMessage()
        {
            //Arrange
            var expected = "This is a message";

            //Act
            var exception = new SceneNotFoundException("This is a message");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

using NUnit.Framework;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class ParticleSetupDoesNotExistExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleSetupDoesNotExistException();
            var expected = "The particle setup does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleSetupDoesNotExistException("test-setup");
            var expected = "The particle setup with the name 'test-setup' does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

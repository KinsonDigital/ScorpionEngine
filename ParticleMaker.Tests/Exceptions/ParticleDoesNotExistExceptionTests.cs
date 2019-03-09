using NUnit.Framework;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class ParticleDoesNotExistExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokedWithNoParams_BuildsCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleDoesNotExistException();
            var expected = "The particle does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokedWithParams_BuildsCorrectExceptionMessage()
        {
            //Arrange
            var exception = new ParticleDoesNotExistException("test-particle", @"C:\temp\missing-particle-file.png");
            var expected = @"The particle 'test-particle' at the path 'C:\temp\missing-particle-file.png' does not exist.";

            //Act
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

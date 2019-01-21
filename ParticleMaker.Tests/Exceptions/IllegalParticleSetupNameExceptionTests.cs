using NUnit.Framework;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class IllegalParticleSetupNameExceptionTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvokingWithNoParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var expected = "Illegal particle setup name.  Cannot not use characters \\/:*?\"<>|";

            //Act
            var exception = new IllegalParticleSetupNameException();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenInvokingWithParam_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var expected = "The particle setup 'test*-pro|ject'.  Cannot not use characters \\/:*?\"<>|";

            //Act
            var exception = new IllegalParticleSetupNameException("test*-pro|ject");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

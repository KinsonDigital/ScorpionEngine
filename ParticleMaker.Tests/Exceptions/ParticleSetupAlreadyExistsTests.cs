using NUnit.Framework;
using ParticleMaker.Exceptions;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class ParticleSetupAlreadyExistsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenCreatingWithNoArgument_CreatesValidMessage()
        {
            //Arrange
            var expected = "A particle setup with that name already exists.";

            //Arrange
            var exception = new ParticleSetupAlreadyExists();
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Ctor_WhenCreatingWithArgument_CreatesValidMessage()
        {
            //Arrange
            var expected = "A particle setup with the name 'test-setup' already exists.";

            //Arrange
            var exception = new ParticleSetupAlreadyExists("test-setup");
            var actual = exception.Message;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

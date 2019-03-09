using NUnit.Framework;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    [TestFixture]
    public class AddParticleEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_ProperlySetsPropertyValue()
        {
            //Arrange
            var expected = "test-item";

            //Act
            var eventArgs = new AddParticleEventArgs("test-item");
            var actual = eventArgs.ParticleFilePath;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

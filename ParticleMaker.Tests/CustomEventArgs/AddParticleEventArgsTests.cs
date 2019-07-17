using Xunit;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    /// <summary>
    /// Unit tests to test the <see cref="AddParticleEventArgs"/> class.
    /// </summary>
    public class AddParticleEventArgsTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_ProperlySetsPropertyValue()
        {
            //Arrange
            var expected = "test-item";

            //Act
            var eventArgs = new AddParticleEventArgs("test-item");
            var actual = eventArgs.ParticleFilePath;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

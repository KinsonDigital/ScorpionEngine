using Xunit;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    /// <summary>
    /// Unit tests to test the <see cref="DeploySetupEventArgs"/> class.
    /// </summary>
    public class DeploySetupEventArgsTests
    {
        #region Constructor Tests
        [Fact]
        public void Ctor_WhenInvoking_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new DeploySetupEventArgs(@"C:\DeployDestination");
            var expected = @"C:\DeployDestination";

            //Act
            var actual = exception.DeploymentPath;

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

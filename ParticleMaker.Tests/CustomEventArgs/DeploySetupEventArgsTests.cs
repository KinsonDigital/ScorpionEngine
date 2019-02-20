using NUnit.Framework;
using ParticleMaker.CustomEventArgs;

namespace ParticleMaker.Tests.CustomEventArgs
{
    [TestFixture]
    public class DeploySetupEventArgsTests
    {
        #region Constructor Tests
        [Test]
        public void Ctor_WhenInvoking_CreatesCorrectExceptionMessage()
        {
            //Arrange
            var exception = new DeploySetupEventArgs(@"C:\DeployDestination");
            var expected = @"C:\DeployDestination";

            //Act
            var actual = exception.DeploymentPath;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}

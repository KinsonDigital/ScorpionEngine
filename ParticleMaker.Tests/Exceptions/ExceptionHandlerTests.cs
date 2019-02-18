using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class ExceptionHandlerTests
    {
        #region Prop Tests
        [Test]
        public void LoggingEnabled_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange & Act
            ExceptionHandler.LoggingEnabled = true;

            //Assert
            Assert.IsTrue(ExceptionHandler.LoggingEnabled);
        }


        [Test]
        public void ShowDialogEnabled_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange & Act
            ExceptionHandler.ShowDialogEnabled = true;

            //Assert
            Assert.IsTrue(ExceptionHandler.ShowDialogEnabled);
        }


        [Test]
        public void Logger_WhenSettingValue_ReturnsCorrectValue()
        {
            //Arrange
            var mockLoggerService = new Mock<ILoggerService>();

            //Act
            ExceptionHandler.Logger = mockLoggerService.Object;

            //Assert
            Assert.IsNotNull(ExceptionHandler.Logger);
        }
        #endregion
    }
}

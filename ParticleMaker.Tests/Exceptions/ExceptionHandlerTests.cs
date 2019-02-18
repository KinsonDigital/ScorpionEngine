using Moq;
using NUnit.Framework;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;
using System;
using System.Windows;

namespace ParticleMaker.Tests.Exceptions
{
    [TestFixture]
    public class ExceptionHandlerTests
    {
        #region Fields
        private Mock<ILoggerService> _mockLoggerService;
        private Mock<IExceptionMessage> _mockExceptionMessage;
        #endregion


        #region Prop Tests
        [Test]
        public void LoggingEnabled_WhenGettingValueWithLoggerSet_ReturnsTrue()
        {
            //Arrange
            var mockLoggerService = new Mock<ILoggerService>();

            ExceptionHandler.Logger = mockLoggerService.Object;

            //Act
            ExceptionHandler.LoggingEnabled = true;

            //Assert
            Assert.IsTrue(ExceptionHandler.LoggingEnabled);
        }


        [Test]
        public void LoggingEnabled_WhenGettingValueWithLoggerNotSet_ReturnsFalse()
        {
            //Act
            ExceptionHandler.Logger = null;
            ExceptionHandler.LoggingEnabled = true;

            //Assert
            Assert.IsFalse(ExceptionHandler.LoggingEnabled);
        }


        [Test]
        public void ShowMessageBoxEnabled_WhenGettingValueWithExceptionMessageBoxSet_ReturnsTrue()
        {
            //Act
            ExceptionHandler.ShowMessageBoxEnabled = true;

            //Assert
            Assert.IsTrue(ExceptionHandler.ShowMessageBoxEnabled);
        }


        [Test]
        public void ShowMessageBoxEnabled_WhenGettingValueWithExceptionMessageBoxNotSet_ReturnsFalse()
        {
            //Act
            ExceptionHandler.ExceptionMessageBox = null;
            ExceptionHandler.ShowMessageBoxEnabled = true;

            //Assert
            Assert.IsFalse(ExceptionHandler.ShowMessageBoxEnabled);
        }


        [Test]
        public void Logger_WhenSettingValue_ReturnsCorrectValue()
        {
            //Assert
            Assert.IsNotNull(ExceptionHandler.Logger);
        }


        [Test]
        public void ExceptionMessageBox_WhenSettingValue_ReturnsCorrectValue()
        {
            //Assert
            Assert.IsNotNull(ExceptionHandler.ExceptionMessageBox);
        }
        #endregion


        #region Method Tests
        [Test]
        public void Handle_WhenInvokedWithLoggerSetAndEnabled_InvokesLogErrorMethod()
        {
            //Arrange
            ExceptionHandler.ShowMessageBoxEnabled = true;
            ExceptionHandler.LoggingEnabled = true;

            //Act
            ExceptionHandler.Handle(new NullReferenceException());

            //Assert
            _mockLoggerService.Verify(m => m.LogError(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void Handle_WhenInvokedWithLoggerAndExceptionmessageBoxSetAndEnabled_InvokesLogErrorMethod()
        {
            //Arrange
            ExceptionHandler.ShowMessageBoxEnabled = false;
            ExceptionHandler.LoggingEnabled = true;

            //Act & Assert
            Assert.Throws(typeof(NullReferenceException), () =>
            {
                ExceptionHandler.Handle(new NullReferenceException());
            });

            _mockLoggerService.Verify(m => m.LogError(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }


        [Test]
        public void Handle_WhenInvokedWithExceptionMessageBoxNotSetAndDisabled_RethrowsException()
        {
            //Act & Assert
            Assert.Throws(typeof(NullReferenceException), () =>
            {
               ExceptionHandler.Handle(new NullReferenceException());
            });
        }


        [Test]
        public void Handle_WhenInvokedWithMessageBoxSetAndEnabled_InvokesShowMessageMethod()
        {
            //Arrange
            ExceptionHandler.ShowMessageBoxEnabled = true;
            ExceptionHandler.LoggingEnabled = true;

            //Act
            ExceptionHandler.Handle(new NullReferenceException());

            //Assert
            _mockExceptionMessage.Verify(m => m.ShowMessage(It.IsAny<NullReferenceException>(), It.IsAny<Window>()), Times.Once());
        }
        #endregion


        #region Private Methods
        [SetUp]
        public void Setup()
        {
            _mockLoggerService = new Mock<ILoggerService>();
            _mockExceptionMessage = new Mock<IExceptionMessage>();

            ExceptionHandler.Logger = _mockLoggerService.Object;
            ExceptionHandler.ExceptionMessageBox = _mockExceptionMessage.Object;
        }


        [TearDown]
        public void TearDown()
        {
            ExceptionHandler.Logger = null;
            ExceptionHandler.LoggingEnabled = false;
            ExceptionHandler.ShowMessageBoxEnabled = false;
        }
        #endregion
    }
}

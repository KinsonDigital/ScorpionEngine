using System;
using System.Windows;
using Moq;
using Xunit;
using ParticleMaker.Exceptions;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Exceptions
{
    public class ExceptionHandlerTests : IDisposable
    {
        #region Private Fields
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly Mock<IExceptionMessage> _mockExceptionMessage;
        #endregion


        #region Constructors
        public ExceptionHandlerTests()
        {
            _mockLoggerService = new Mock<ILoggerService>();
            _mockExceptionMessage = new Mock<IExceptionMessage>();

            ExceptionHandler.Logger = _mockLoggerService.Object;
            ExceptionHandler.ExceptionMessageBox = _mockExceptionMessage.Object;
        }
        #endregion


        #region Prop Tests
        [Fact]
        public void LoggingEnabled_WhenGettingValueWithLoggerSet_ReturnsTrue()
        {
            //Arrange
            var mockLoggerService = new Mock<ILoggerService>();

            ExceptionHandler.Logger = mockLoggerService.Object;

            //Act
            ExceptionHandler.LoggingEnabled = true;

            //Assert
            Assert.True(ExceptionHandler.LoggingEnabled);
        }


        [Fact]
        public void LoggingEnabled_WhenGettingValueWithLoggerNotSet_ReturnsFalse()
        {
            //Act
            ExceptionHandler.Logger = null;
            ExceptionHandler.LoggingEnabled = true;

            //Assert
            Assert.False(ExceptionHandler.LoggingEnabled);
        }


        [Fact]
        public void ShowMessageBoxEnabled_WhenGettingValueWithExceptionMessageBoxSet_ReturnsTrue()
        {
            //Act
            ExceptionHandler.ShowMessageBoxEnabled = true;

            //Assert
            Assert.True(ExceptionHandler.ShowMessageBoxEnabled);
        }


        [Fact]
        public void ShowMessageBoxEnabled_WhenGettingValueWithExceptionMessageBoxNotSet_ReturnsFalse()
        {
            //Act
            ExceptionHandler.ExceptionMessageBox = null;
            ExceptionHandler.ShowMessageBoxEnabled = true;

            //Assert
            Assert.False(ExceptionHandler.ShowMessageBoxEnabled);
        }


        [Fact]
        public void Logger_WhenSettingValue_ReturnsCorrectValue()
        {
            //Assert
            Assert.NotNull(ExceptionHandler.Logger);
        }


        [Fact]
        public void ExceptionMessageBox_WhenSettingValue_ReturnsCorrectValue()
        {
            //Assert
            Assert.NotNull(ExceptionHandler.ExceptionMessageBox);
        }
        #endregion


        #region Method Tests
        [Fact]
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


        [Fact]
        public void Handle_WhenInvokedWithLoggerAndExceptionmessageBoxSetAndEnabled_InvokesLogErrorMethod()
        {
            //Arrange
            ExceptionHandler.ShowMessageBoxEnabled = false;
            ExceptionHandler.LoggingEnabled = true;

            //Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                ExceptionHandler.Handle(new NullReferenceException());
            });

            _mockLoggerService.Verify(m => m.LogError(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }


        [Fact]
        public void Handle_WhenInvokedWithExceptionMessageBoxNotSetAndDisabled_RethrowsException()
        {
            //Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
               ExceptionHandler.Handle(new NullReferenceException());
            });
        }


        [Fact]
        public void Handle_WhenInvokedWithMessageBoxSetAndEnabled_InvokesShowMessageMethod()
        {
            //Arrange
            ExceptionHandler.ShowMessageBoxEnabled = true;
            ExceptionHandler.LoggingEnabled = true;

            //Act
            ExceptionHandler.Handle(new NullReferenceException());

            //Assert
            _mockExceptionMessage.Verify(m => m.ShowExceptionDialog(It.IsAny<NullReferenceException>(), It.IsAny<Window>()), Times.Once());
        }
        #endregion


        #region Public Methods
        public void Dispose()
        {
            ExceptionHandler.Logger = null;
            ExceptionHandler.LoggingEnabled = false;
            ExceptionHandler.ShowMessageBoxEnabled = false;
        }
        #endregion
    }
}

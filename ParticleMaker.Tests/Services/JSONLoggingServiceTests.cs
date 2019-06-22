using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using ParticleMaker.Services;

namespace ParticleMaker.Tests.Services
{
    public class JSONLoggingServiceTests
    {
        #region Method Tests
        [Fact]
        public void Log_WhenInvoking_InvokesDirServiceExistsMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.Log("test-data", DateTime.Now);

            //Assert
            mockDirectoryService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Log_WhenLogLogsDirectoryDoesNotExist_InvokesDirServiceCreateMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.Log("test-data", DateTime.Now);

            //Assert
            mockDirectoryService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void Log_WhenLogFileDoesNotExist_InvokesFileServiceCreateMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.Log("test-data", DateTime.Now);

            //Assert
            mockFileService.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<object>()));
        }


        [Fact]
        public void Log_WhenLogFileDoesNotExist_BuildsProperPath()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var expected = $"Info-Logs_20-Feb-2019.json";

            var actual = string.Empty;
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((path, data) =>
            {
                var sections = path.Split(new[] { "\\" }, StringSplitOptions.None);

                actual = sections.Length > 0 ? sections[sections.Length - 1] : "";
            });

            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.Log("test-data", new DateTime(2019, 2, 20));

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Log_WhenInvoked_InvokesFileServiceExistsMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.Log("test-data", DateTime.Now);

            //Assert
            mockFileService.Verify(m => m.Exists(It.IsAny<string>()));
        }


        [Fact]
        public void Log_WhenLogFileDoesExist_InvokesFileServiceLoadMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<LogData>(It.IsAny<string>())).Returns(new LogData()
            {
                Logs = new List<Log>()
            });

            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.Log("test-data", DateTime.Now);

            //Assert
            mockFileService.Verify(m => m.Load<LogData>(It.IsAny<string>()));
        }


        [Fact]
        public void Log_WhenLogFileDoesExist_InvokesFileServiceSaveMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<LogData>(It.IsAny<string>())).Returns(new LogData()
            {
                Logs = new List<Log>()
            });

            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.Log("test-data", DateTime.Now);

            //Assert
            mockFileService.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<LogData>()));
        }


        [Fact]
        public void LogError_WhenInvoking_InvokesDirServiceExistsMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", DateTime.Now, 1234);

            //Assert
            mockDirectoryService.Verify(m => m.Exists(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void LogError_WhenLogLogsDirectoryDoesNotExist_InvokesDirServiceCreateMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", DateTime.Now, 1234);

            //Assert
            mockDirectoryService.Verify(m => m.Create(It.IsAny<string>()), Times.Once());
        }


        [Fact]
        public void LogError_WhenLogFileDoesNotExist_InvokesFileServiceCreateMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", DateTime.Now, 1234);

            //Assert
            mockFileService.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<object>()));
        }


        [Fact]
        public void LogError_WhenLogFileDoesNotExist_BuildsProperPath()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var expected = $"Error-Logs_20-Feb-2019.json";

            var actual = string.Empty;
            mockFileService.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<LogData>())).Callback<string, LogData>((path, data) =>
            {
                var sections = path.Split(new[] { "\\" }, StringSplitOptions.None);

                actual = sections.Length > 0 ? sections[sections.Length - 1] : "";
            });

            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", new DateTime(2019, 2, 20), 1234);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LogError_WhenCheckingIfLogFileExists_BuildsProperPath()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var expected = $"Error-Logs_20-Feb-2019.json";

            var actual = string.Empty;
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Callback<string>((path) =>
            {
                var sections = path.Split(new[] { "\\" }, StringSplitOptions.None);

                actual = sections.Length > 0 ? sections[sections.Length - 1] : "";
            });

            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", new DateTime(2019, 2, 20), 1234);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LogError_WhenInvoked_InvokesFileServiceExistsMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", DateTime.Now, 1234);

            //Assert
            mockFileService.Verify(m => m.Exists(It.IsAny<string>()));
        }


        [Fact]
        public void LogError_WhenLogFileDoesExist_InvokesFileServiceLoadMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<LogData>(It.IsAny<string>())).Returns(new LogData()
            {
                Logs = new List<Log>()
            });

            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", DateTime.Now, 1234);

            //Assert
            mockFileService.Verify(m => m.Load<LogData>(It.IsAny<string>()));
        }


        [Fact]
        public void LogError_WhenLogFileDoesExist_InvokesFileServiceSaveMethod()
        {
            //Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService.Setup(m => m.Exists(It.IsAny<string>())).Returns(true);
            mockFileService.Setup(m => m.Load<LogData>(It.IsAny<string>())).Returns(new LogData()
            {
                Logs = new List<Log>()
            });

            var mockDirectoryService = new Mock<IDirectoryService>();

            var service = new JSONLoggerService(mockDirectoryService.Object, mockFileService.Object);

            //Act
            service.LogError("test-data", DateTime.Now, 1234);

            //Assert
            mockFileService.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<LogData>()));
        }
        #endregion
    }
}

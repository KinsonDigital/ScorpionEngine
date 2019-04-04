using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ParticleMaker.Services
{
    public class JSONLoggerService : ILoggerService
    {
        #region Fields
        private IFileService _fileService;
        private IDirectoryService _directoryService;
        private readonly string _logsDirectory;
        private readonly Dictionary<int, string> _monthNames = new Dictionary<int, string>()
        {
            { 1, "Jan" }, { 2, "Feb" }, { 3, "Mar" }, { 4, "April" },
            { 5, "May" }, { 6, "June" }, { 7, "July" }, { 8, "Aug" },
            { 9, "Sept" }, { 10, "Oct" }, { 11, "Nov" }, { 12, "Dec" },
        };
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="JSONLoggerService"/>.
        /// </summary>
        /// <param name="directoryService">manages the directory that the logging file will be located in.</param>
        /// <param name="fileService">Manages the file that will contain the logs.</param>
        public JSONLoggerService(IDirectoryService directoryService, IFileService fileService)
        {
            _fileService = fileService;
            _directoryService = directoryService;

            _logsDirectory = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Logs";
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Logs genral information using the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data/info to log.</param>
        /// <param name="dateTime">The date time stamp of when the log will occur.</param>
        public void Log(string data, DateTime dateTime)
        {
            var monthName = _monthNames[dateTime.Month];

            var dateTimeStamp = $"{dateTime.Day}-{monthName}-{dateTime.Year}";
            var logFilePath = $@"{_logsDirectory}\Info-Logs_{dateTimeStamp}.json";

            if (!_directoryService.Exists(_logsDirectory))
                _directoryService.Create(_logsDirectory);

            if (_fileService.Exists(logFilePath))
            {
                var logData = _fileService.Load<LogData>(logFilePath);

                logData.Logs.Add(new Log()
                {
                    Data = data,
                    DateTimeStamp = dateTime,
                    IsError = false,
                    ErrorNumber = -1
                });

                _fileService.Save(logFilePath, logData);
            }
            else
            {
                _fileService.Create(logFilePath, data);
            }
        }


        /// <summary>
        /// Logs an error using the given information.
        /// </summary>
        /// <param name="data">The data/info to log.</param>
        /// <param name="dateTime">The date time stamp of when the error has occured.</param>
        /// <param name="errorNumber">The error number associated with the error.</param>
        public void LogError(string data, DateTime dateTime, int errorNumber)
        {
            var monthName = _monthNames[dateTime.Month];

            var dateTimeStamp = $"{dateTime.Day}-{monthName}-{dateTime.Year}";
            var logFilePath = $@"{_logsDirectory}\Error-Logs_{dateTimeStamp}.json";

            if (!_directoryService.Exists(_logsDirectory))
                _directoryService.Create(_logsDirectory);

            if (_fileService.Exists(logFilePath))
            {
                var logData = _fileService.Load<LogData>(logFilePath);

                logData.Logs.Add(new Log()
                {
                    Data = data,
                    DateTimeStamp = dateTime,
                    IsError = true,
                    ErrorNumber = errorNumber
                });

                _fileService.Save(logFilePath, logData);
            }
            else
            {
                var logData = new LogData()
                {
                    Logs = new List<Log>()
                    {
                        new Log()
                        {
                            Data = data,
                            DateTimeStamp = dateTime,
                            IsError = true,
                            ErrorNumber = errorNumber
                        }
                    }
                };

                _fileService.Create(logFilePath, logData);
            }
        }
        #endregion
    }
}

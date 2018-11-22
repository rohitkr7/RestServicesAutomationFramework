using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using System;

namespace RestServicesAutomationFramework.Logger
{
    class Log4NetHelper
    {
        #region Fields

        private static ILog _logger;
        private static ConsoleAppender _consoleAppender;
        private static FileAppender _fileAppender;
        private static RollingFileAppender _rolliingFileAppender;
        private static string _layout = "%date{MM-dd-yyyy-HH:mm:ss} [%level] [%method] - %message%newline";

        #endregion

        #region Property

        public static string Layout
        {
            set { _layout = value; }
        }

        #endregion

        #region private

        private static PatternLayout GetPatternLayout()
        {
            var patternLayout = new PatternLayout()
            {
                ConversionPattern = _layout
            };
            patternLayout.ActivateOptions();
            return patternLayout;
        }

        private static ConsoleAppender GetConsoleAppender()
        {
            var consoleAppender = new ConsoleAppender()
            {
                Name = "ConsoleAppender",
                Layout = GetPatternLayout(),
                Threshold = Level.All
            };
            consoleAppender.ActivateOptions();

            return consoleAppender;
        }

        private static FileAppender GetFileAppender(string fileLocation, string testCaseId)
        {
            var fileAppender = new FileAppender()
            {
                Name = "FileAppender",
                Layout = GetPatternLayout(),
                Threshold = Level.All,
                AppendToFile = true,
                File = fileLocation + testCaseId+".log"
            };
            fileAppender.ActivateOptions();

            return fileAppender;
        }

        private static RollingFileAppender GetRollingFileAppender()
        {
            var rollingFileAppender = new RollingFileAppender()
            {
                Name = "RollingFileAppender",
                Layout = GetPatternLayout(),
                Threshold = Level.All,
                AppendToFile = true,
                File = "RollingFileLogger.log",
                MaximumFileSize = "1MB",
                MaxSizeRollBackups = 15
            };
            rollingFileAppender.ActivateOptions();

            return rollingFileAppender;
        }

        #endregion

        #region Public 
        public static ILog GetLogger(Type type, string fileLocation, string testCaseId)
        {
            if (_consoleAppender == null)
            {
                _consoleAppender = GetConsoleAppender();
            }

            if(_fileAppender == null)
            {
                _fileAppender = GetFileAppender(fileLocation, testCaseId);
            }

            if(_rolliingFileAppender == null)
            {
                _rolliingFileAppender = GetRollingFileAppender();
            }

            if(_logger != null)
            {
                return _logger;
            }

            //BasicConfigurator.Configure(_consoleAppender, _fileAppender, _rolliingFileAppender);
            BasicConfigurator.Configure(_consoleAppender, _fileAppender);

            _logger = LogManager.GetLogger(type);
            return _logger;
        }
        #endregion

    }
}

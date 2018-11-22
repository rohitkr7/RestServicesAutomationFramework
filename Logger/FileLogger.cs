using System;
using System.IO;

namespace RestServicesAutomationFramework.Logger
{

    class FileLogger : LogBase
    {
        string testCaseId;
        Boolean fileExists = false;
        private readonly string datetimeFormat;
        private readonly string logFilename;

        public FileLogger(string testCaseId)
        {
            logFilename = filePath + testCaseId + "_" + System.DateTime.Now.Date.ToString("MM_dd_yyyy") + "_.txt";
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.f";
            this.testCaseId = testCaseId;
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("FileLogger.cs");

        public string filePath = @"C:\Users\rohit_knw2paf\Desktop\testingAutomation\RestServicesAutomationFramework\RestServicesAutomationFramework\Output\";

        public override void Log(string message)
        {
            lock (lockObj)
            {
                DateTime currentDateTime = System.DateTime.Now;
                if (!fileExists)
                {
                    using (StreamWriter streamWriter = new StreamWriter(logFilename))
                    {
                        streamWriter.WriteLine("**------------------------** " + testCaseId + " **--------------------------**");
                        streamWriter.WriteLine("Logging Date: " + currentDateTime);
                        streamWriter.WriteLine();
                        streamWriter.WriteLine("<" + currentDateTime + ">: " + message);
                        streamWriter.Close();
                    }
                    fileExists = true;
                }
                else if (fileExists)
                {
                    using (StreamWriter streamWriter = File.AppendText(logFilename))
                    {
                        streamWriter.WriteLine("<" + currentDateTime + ">: " + message);
                        streamWriter.Close();
                    }
                }
            }
        }

        public void WriteFormattedLog(LogLevel level, string text)
        {
            string pretext;
            switch (level)
            {
                case LogLevel.TRACE:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                    break;
                case LogLevel.INFO:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                    break;
                case LogLevel.DEBUG:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                    break;
                case LogLevel.WARNING:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                    break;
                case LogLevel.ERROR:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                    break;
                case LogLevel.FATAL:
                    pretext = System.DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                    break;
                default:
                    pretext = "";
                    break;
            }

            WriteLine(pretext + text);
        }

        private void WriteLine(string text, bool append = true)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logFilename, append, System.Text.Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        [System.Flags]
        public enum LogLevel
        {
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }

        /// <summary>
        /// Log a DEBUG message
        /// </summary>
        /// <param name="text">Message</param>
        public void Debug(string text)
        {
            WriteFormattedLog(LogLevel.DEBUG, text);
        }

        /// <summary>
        /// Log an ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Error(string text)
        {
            WriteFormattedLog(LogLevel.ERROR, text);
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Fatal(string text)
        {
            WriteFormattedLog(LogLevel.FATAL, text);
        }

        /// <summary>
        /// Log an INFO message
        /// </summary>
        /// <param name="text">Message</param>
        public void Info(string text)
        {
            WriteFormattedLog(LogLevel.INFO, text);
        }

        /// <summary>
        /// Log a TRACE message
        /// </summary>
        /// <param name="text">Message</param>
        public void Trace(string text)
        {
            WriteFormattedLog(LogLevel.TRACE, text);
        }

        /// <summary>
        /// Log a WARNING message
        /// </summary>
        /// <param name="text">Message</param>
        public void Warning(string text)
        {
            WriteFormattedLog(LogLevel.WARNING, text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

namespace FirstGenesis.Mobius.Logging
{
    /// <summary>
    /// Loggers typically allow applications to log the following types of messages or exceptions
    /// 1. Errors 
    /// 2. Warnings
    /// 3. Debug Messages
    /// 4. Info Messages
    /// This class gives capability to log any usefull information of aforesaid types while application is running. 
    /// </summary>
    public class Logger
    {

        #region Private Fields

        private static Logger logger = null;
        private static object LOCK = new Object();
        private string logFileName = null;
        private LogSeverity logSeverity = LogSeverity.INFO;

        /// <summary>
        /// Indicates that the WriteLog method has been called the first time.
        /// After the first invocation, this value will be set to false.
        /// This is used to insert a new line to separate the Logs from this run of the application
        /// from the previous runs.
        /// </summary>
        private bool log_FirstCall = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor is private so that it can not be called from the outside.
        /// </summary>
        private Logger()
        {
            try
            {
                logFileName = ConfigurationManager.AppSettings.Get("Log.File.Name");
                logSeverity = LogSeverity.GetLevelGivenInt(ConfigurationManager.AppSettings.Get("Log.Severity"));
            }
            catch
            {
                //Ignore
            }

            if (logFileName == null)
            {
                logFileName = GetDefaultLogFile();
            }

            CheckLogFile();
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is static and called to get the instance of this class. This method also 
        /// makes sure that only one instance of this class is there in that application domain.
        /// </summary>
        /// <returns>Instance of class Logger</returns>
        public static Logger GetInstance()
        {
            if (logger == null)
            {
                lock (LOCK)
                {
                    if(logger == null)
                        logger = new Logger();
                }
            }

            return logger;
        }

        /// <summary>
        /// Logging the message
        /// </summary>
        /// <param name="message">Message needs to be logged.</param>
        public void WriteLog(string message, string className)
        {
            WriteLog(LogSeverity.INFO, className,message);
        }

        /// <summary>
        /// Logging the message with the specified log severity.
        /// </summary>
        /// <param name="severity">LogSeverity level</param
        /// <param name="className">Name of the class from where this method is called.</param>
        /// <param name="message">Message needs to be logged.</param>
        public void WriteLog(LogSeverity severity, string className, string message)
        {
            //if (severity.GetLogSeverity() > logSeverity.GetLogSeverity())
            //    return;
            string now = "[" + DateTime.Now.ToString() + "]";
            string logMessage_Begin = severity.ToString() + now + "[";
            string logMessage_End = "] -" + message;
            WriteToStream(logMessage_Begin + className + logMessage_End);
            
        }
        /// <summary>
        /// Logging the message with the specified log severity and exception.
        /// </summary>
        /// <param name="severity">LogSeverity level</param>
        /// <param name="className">Name of the class from where this method is called.</param>
        /// <param name="exception">Application exception</param>
        /// <param name="message">Message needs to be logged.</param>
        public void WriteLog(LogSeverity severity, string className, Exception exception, string message)
        {
            if (severity.GetLogSeverity() > logSeverity.GetLogSeverity())
                return;

            string now = "[" + DateTime.Now.ToString() + "]";
            string logMessage_Begin = severity.ToString() + now + "[";
            string logMessage_End = "] -" + message;

            if (exception != null)
            {
                logMessage_End += "  Exception :" + exception.ToString();
                string stackTrace = exception.StackTrace;
                if (stackTrace == null || stackTrace.Trim().Length == 0)
                {
                    //Do Nothing ..
                }
                else
                {
                    logMessage_End += "   StackTrace -- >" + exception.StackTrace;
                }
            }

            WriteToStream(logMessage_Begin + className + logMessage_End);
        }

        /// <summary>
        /// Initialize the logger class. This method will be called from the constructor.
        /// </summary>
        /// <param name="fileName">Name of the log file</param>
        private void InitializeLogger(string fileName)
        {
            //logFilePath = Path.Combine(LOGDIRPATH, fileName);

            //logFileStream = File.Open(logFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            //logFileStream.Seek(0, SeekOrigin.End);

            //logStream = new BufferedStream(logFileStream, BUFFERSIZE);
        }

        /// <summary>
        /// This method do the job to write data to the buffered stream.
        /// </summary>
        /// <param name="message">Message that needs to be written in the log file</param>
        private void WriteToStream(string message)
        {
            lock (LOCK)
            {
                CheckLogFile();
                StreamWriter logWritter = new StreamWriter(logFileName, true);

                if (log_FirstCall)
                {
                    /// Indicates that the WriteLog method has been called the first time.
                    /// After the first invocation, this value will be set to false.
                    /// This is used to insert  newlines to separate the Logs from this run of the application
                    /// from the previous runs.

                    logWritter.WriteLine(Environment.NewLine + Environment.NewLine);
                    log_FirstCall = false;
                }

                logWritter.WriteLine(message);
                logWritter.Flush();
                logWritter.Close();
                logWritter = null;

            }
            //byte[] bytes = ASCIIEncoding.ASCII.GetBytes(message);
            //lock (this)
            //{
            //    logStream.Write(bytes, 0, bytes.Length);
            //    logStream.Flush();
            //}
        }

        /// <summary>
        /// Used when no Log File Name is Specified.
        /// </summary>
        /// <returns></returns>
        private string GetDefaultLogFile()
        {
            return "FirstGenesis.Mobius.Logger.log";
        }

        /// <summary>
        /// Check if the Log File exists.
        /// If Not, Create ..
        /// </summary>
        private void CheckLogFile()
        {
            if (!File.Exists(logFileName))
            {
                FileStream fStream = File.Create(logFileName);
                fStream.Close();
            }
        }

        #endregion

    }

    ///// <summary>
    ///// Represents the severity of the logged information.
    ///// </summary>
    //public enum LogSeverity
    //{
    //    ERROR,
    //    WARNING,
    //    DEBUG,
    //    INFO
    //}
}

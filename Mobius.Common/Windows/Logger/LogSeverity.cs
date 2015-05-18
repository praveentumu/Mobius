using System;
using System.Collections.Generic;
using System.Text;

namespace FirstGenesis.Mobius.Logging
{
    /// <summary>
    /// This class reprsents the Logging Severity.
    /// </summary>
    public class LogSeverity
    {
        #region Public Constant Fields ..
		/// <summary>
        /// A LogSeverity instance indicating that only Error messages be logged by the Logger
        /// that has this Severity.
		/// </summary>
        public static readonly LogSeverity ERROR = new LogSeverity(TYPE_ERROR);
		/// <summary>
        /// A LogSeverity instance indicating that only Warning messages be logged by the Logger
        /// that has this Severity.
		/// </summary>
        public static readonly LogSeverity WARNING = new LogSeverity(TYPE_WARNING);
		/// <summary>
        /// A LogSeverity instance indicating that only DEBUG messages be logged by the Logger
        /// that has this Severity.
		/// </summary>
        public static readonly LogSeverity DEBUG = new LogSeverity(TYPE_DEBUG);
		/// <summary>
        /// A LogSeverity instance indicating that only INFO messages be logged by the Logger
        /// that has this Severity.
		/// </summary>
        public static readonly LogSeverity INFO = new LogSeverity(TYPE_INFO);
		#endregion

		#region Private Instance Fields 

		private const int TYPE_ERROR = 1;
		private const int TYPE_WARNING = 2;
		private const int TYPE_DEBUG = 3;
		private const int TYPE_INFO = 4;

        //The actual Severity associated with this class.
		private int logSeverityValue;
		#endregion
		
		#region Private Constructor ..
        private LogSeverity(int level)
		{
			if (level < 1 || level > 4)
			{
				throw new ArgumentException("Unrecognized Log Severity");
			}
            this.logSeverityValue = level;
		}
		#endregion

        #region Methods
        /// <summary>
        /// Overridden to return a friendly reprsentation of this Log Severity.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (logSeverityValue)
            {
                case TYPE_ERROR:
                    return "[ERROR]";
                case TYPE_WARNING:
                    return "[WARNING]";
                case TYPE_DEBUG:
                    return "[DEBUG]";
                case TYPE_INFO:
                    return "[INFO]";
                default:
                    throw new InvalidOperationException("Unrecognized Log Severity.");

            }
        }

        /// <summary>
        /// Get the severity of the log.
        /// </summary>
        /// <returns></returns>
        internal int GetLogSeverity()
        {
            return this.logSeverityValue;
        }

        /// <summary>
        /// Get LogSeverity instance based on the log severity value.
        /// </summary>
        /// <param name="number">Value of the logseverity as string</param>
        /// <returns></returns>
        internal static LogSeverity GetLevelGivenInt(string number)
        {
            int severity;
            if (!int.TryParse(number, out severity))
                severity = -1;

            switch (severity)
            {
                case TYPE_ERROR:
                    return ERROR;
                case TYPE_WARNING:
                    return WARNING;
                case TYPE_DEBUG:
                    return DEBUG;
                case TYPE_INFO:
                    return INFO;
                default:
                    throw new InvalidOperationException("Unrecognized Log Severity. Internal Error");

            }
        }
        #endregion

    }
}

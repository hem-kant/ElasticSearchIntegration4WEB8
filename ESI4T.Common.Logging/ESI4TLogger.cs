using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESI4T.Common.Logging
{
    public enum ELogLevel
    {
        DEBUG = 1,
        ERROR,
        FATAL,
        INFO,
        WARN
    }
    public static class ESI4TLogger
    {
        #region Members
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ESI4TLogger));
        #endregion

        #region Constructors
        static ESI4TLogger()
        {


            string LOGFILECONFIG = ConfigurationManager.AppSettings["LoggingConfigPath"];

            System.IO.FileInfo config = new System.IO.FileInfo(LOGFILECONFIG);
            XmlConfigurator.Configure(config);
        }
        #endregion

        #region Methods
        public static void WriteLog(ELogLevel logLevel, String log)
        {

            if (ESI4TLogger.Logger.IsDebugEnabled && logLevel.Equals(ELogLevel.DEBUG))
            {

                Logger.Debug(log);

            }

            else if (ESI4TLogger.Logger.IsErrorEnabled && logLevel.Equals(ELogLevel.ERROR))
            {

                Logger.Error(log);

            }

            else if (ESI4TLogger.Logger.IsFatalEnabled && logLevel.Equals(ELogLevel.FATAL))
            {

                Logger.Fatal(log);

            }

            else if (ESI4TLogger.Logger.IsInfoEnabled && logLevel.Equals(ELogLevel.INFO))
            {

                Logger.Info(log);

            }

            else if (logLevel.Equals(ELogLevel.WARN))
            {
                Logger.Warn(log);
            }
        }

        #endregion
    }
}

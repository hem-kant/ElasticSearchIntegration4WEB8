using ESI4T.Common.Logging;
using ESI4T.Common.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESI4T.Common.ExceptionManagement
{
    public class ExceptionHelper
    {
        private static void LogException(ESI4TIndexingException ampException)
        {
            ESI4TLogger.WriteLog(ELogLevel.ERROR, ampException.Message + ", Code " + ampException.Code);
        }
        public static void HandleException(Exception exception)
        {
            ESI4TIndexingException ampException = exception as ESI4TIndexingException;
            if (ampException != null)
            {
                LogException(ampException);
            }
        }
        public static void HandleException(Exception exception, out ESI4TServiceFault fault)
        {
            ESI4TIndexingException ampException = exception as ESI4TIndexingException;
            fault = new ESI4TServiceFault();
            if (ampException != null)
            {
                fault.Code = ampException.Code;
                fault.Message = ampException.Message;
            }
            else
            {
                fault.Code = ESI4T.Common.Services.ESI4TServiceConstants.ServiceFault.UNKNOWN_EXCEPTION_CODE;
                fault.Message = ESI4T.Common.Services.ESI4TServiceConstants.ServiceFault.UNKNOWN_EXCEPTION_MESSAGE;
            }
        }

        public static void HandleCustomException(Exception ex, string LogMessage)
        {
            ESI4TIndexingException ampEx = ex as ESI4TIndexingException;
            if (ampEx != null)
            {
                ESI4TLogger.WriteLog(ELogLevel.WARN, LogMessage);
            }
            else
            {
                ESI4TLogger.WriteLog(ELogLevel.ERROR, LogMessage);
            }
        }
    }
}

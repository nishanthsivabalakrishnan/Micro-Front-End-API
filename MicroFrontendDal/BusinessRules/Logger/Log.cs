using NLog;
using System.Diagnostics;

namespace MicroFrontendDal.BusinessRules.Logger
{
    public class Log
    {
        #region Logger
        protected static readonly NLog.Logger objNlog = LogManager.GetCurrentClassLogger();
        public void ErrorLog(string fileName,string methodName, Exception ex)
        {
            try
            {
                NLog.Logger objNlog = LogManager.GetCurrentClassLogger();
                string Message = "Problem in :: " + fileName + " :: "+ methodName + "Error Message :: " + GetErrorMessage(ex) + " :: ";
                objNlog.Error(ex, Message);
            }
            catch (Exception)
            {
                Debug.Write("Err");
            }
        }
        public void InfoLog(string methodName, string message)
        {
            try
            {
                NLog.Logger objNlog = LogManager.GetCurrentClassLogger();
                string infoMessage=methodName+ ":: " + message;
                objNlog.Info(infoMessage);
            }
            catch (Exception)
            {
                Debug.Write("Err");
            }
        }
        public void WarnLog(string methodName, string message)
        {
            try
            {
                NLog.Logger objNlog = LogManager.GetCurrentClassLogger();
                string infoMessage = methodName + ":: " + message;
                objNlog.Warn(infoMessage);
            }
            catch (Exception)
            {
                Debug.Write("Err");
            }
        }
        public static string GetErrorMessage(Exception ex)
        {
            string message = ((ex.InnerException != null) ? ex.Message.ToString() + GetErrorMessage(ex.InnerException) : ex.Message.ToString());
            return message;
        }
        #endregion
    }
}

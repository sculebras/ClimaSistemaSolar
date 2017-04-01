using log4net;
using SF.Logger.Filters;
using SF.Logger.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using SF.Core.Extensions;

namespace SF.Logger
{
    public static class Logger
    {
        //public static ILog Log;
        public static TraceSource trace = new TraceSource("TraceLogger");

        public static void Config(HttpConfiguration config)
        {
            //Error Manager Configuration
            //log4net.Config.XmlConfigurator.Configure();
            //Log = log4net.LogManager.GetLogger(GetAppName());
            //Log.Info("Application start");

            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());
            //config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            
            //config.Filters.Add(new GlobalExceptionFilterAttribute());

            //config.Services.Remove(typeof(IHttpActionInvoker), config.Services.GetActionInvoker());
            //config.Services.Replace(typeof(IHttpActionInvoker), new GlobalApiControllerActionInvoker());

            trace.TraceEvent(TraceEventType.Start, 0, "Application start");
        }


        

        /// <summary>
        /// Devuelve un error en formato string como para loguear completo.
        /// con Tipo de Excepcion, Url ,Mensaje y StackTrace.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetErrorCompleteString(Exception ex, HttpRequestMessage request = null)
        {
            string strUrl = string.Format("{0}URL: {1}", Environment.NewLine, RequestToString(request));            
            string strError = string.Format("{0}Type: [{1}]{0}Message: {2}{3}{0}StackTrace:{0}{4}{0}{0}",
                                Environment.NewLine, ex.GetType().Name, GetAggregateExceptionMessages(ex), strUrl, ex.StackTrace);
            return strError;
        }

        /// <summary>
        /// Devuelve la URL completa del Request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request != null)
            {
                if (request.Method != null)
                    message.Append(request.Method);

                if (request.RequestUri != null)
                    message.Append(" ").Append(request.RequestUri);
            }
            return message.ToString();
        }


        /// <summary>
        /// Analiza si la excepcion es de tipo  AggregateException.
        /// Si es asi concatena los mensajes de error de todos los errores y los devuelve.
        /// </summary>
        /// <param name="ex">Excepcion de donde tomar el o los mensajes</param>
        /// <returns></returns>
        public static string GetAggregateExceptionMessages(Exception ex)
        {
            if (ex.GetType() == typeof(AggregateException))
            {
                AggregateException oAggregateException = (AggregateException)ex;
                StringBuilder sb = new StringBuilder();
                int iCant = oAggregateException.InnerExceptions.Count;
                for (int i = 0; i < iCant; i++)
                {
                    var innerExecption = oAggregateException.InnerExceptions[i];
                    string  strMessageInner = innerExecption.Message;
                    
                    sb.AppendLine(string.Format("{0}-[{1}] {2}", i + 1, innerExecption.GetType().FullName, strMessageInner));
                }
                string strMessage = string.Format("{0} ({1}):\n{2}",
                    ex.Message,
                    oAggregateException.InnerExceptions.Count,
                    sb.ToString());
                return strMessage;
            }
            else
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Loguea error.
        /// con Tipo de Excepcion, Url ,Mensaje y StackTrace.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="request"></param>
        public static void TraceErrorComplete(Exception ex, HttpRequestMessage request)
        {
            string strError = Logger.GetErrorCompleteString(ex, request);
            TraceError(strError);
        }
        

        /// <summary>
        /// Logs error with default trace.
        /// </summary>
        /// <param name="strError"></param>
        public static void TraceError(string strError)
        {
            Logger.trace.TraceEvent(TraceEventType.Error, 0, strError);
        }
        /// <summary>
        /// Logs Exception.
        /// </summary>
        /// <param name="ex"></param>
        public static void TraceError(Exception ex)
        {
            string message = string.Format("Exception has been thrown.{0}{1}{0}", Environment.NewLine, GetFullErrorMessageStackTrace(ex));
            Logger.TraceError(message);
        }

        /// <summary>
        /// Devuelve Mensaje de error completo con su StackTrace de sus innerExceptions y sus tipos.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetFullErrorMessageStackTrace(Exception ex)
        {
            List<string> listMsgs = new List<string>();
            List<string> listTypes = new List<string>();
            List<string> listStackTrace = new List<string>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Type: [{0}]", ex.GetType().Name);
            sb.AppendLine();
            sb.AppendLine("Message: ");
            do
            {
                sb.AppendLine(ex.Message);
                listMsgs.Add(ex.Message);
                listTypes.Add(ex.GetType().Name);
                listStackTrace.Add(ex.StackTrace);
                ex = ex.InnerException;
            } while (ex!=null);

            sb.AppendLine("StackTrace: ");
            for (int i = listMsgs.Count; i > 0; i--)
            {
                sb.AppendFormat("Type: [{0}] - {1}", listTypes[i-1], listMsgs[i-1]);
                sb.AppendLine();
                sb.AppendLine(listStackTrace[i-1]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Logs events with default trace.
        /// </summary>
        /// /// <param name="enumTraceEventType"></param>
        /// <param name="msg"></param>
        public static void Trace(System.Diagnostics.TraceEventType enumTraceEventType, string msg)
        {
            Logger.trace.TraceEvent(enumTraceEventType, 0, msg);
        }

        /// <summary>
        /// Logs events with default trace.
        /// </summary>
        /// /// <param name="enumTraceEventType"></param>
        /// <param name="format">Message with format.</param>
        /// /// <param name="parameters">string parameters to replace in the format string.</param>
        public static void Trace(System.Diagnostics.TraceEventType enumTraceEventType, string format, params string[] parameters)
        {
            Logger.trace.TraceEvent(enumTraceEventType, 0, string.Format(format, parameters));
        }

        /// <summary>
        /// Logs Method start event with default trace.
        /// </summary>
        /// <param name="strMethod">(Optional) Name of the Method. If not entered Reflection will be used. (Rembember it has its performance cost)</param>
        public static void TraceStart(string strMethod = null)
        {
            TraceMethodState(TraceEventType.Start, strMethod);
        }
        /// <summary>
        /// Logs Method Stop event with default trace.
        /// </summary>
        /// <param name="strMethod">(Optional) Name of the Method. If not entered Reflection will be used. (Rembember it has its performance cost)</param>
        public static void TraceStop(string strMethod = null)
        {
            TraceMethodState(TraceEventType.Stop, strMethod);
        }

        /// <summary>
        /// Logs Method state event with default trace.
        /// </summary>
        /// <param name="enumTraceEventType">Current State of the method (Ex: Start, End,...)</param>
        /// <param name="strMethod">(Optional) Name of the Method. If not entered Reflection will be used. (Rembember it has its performance cost)</param>
        public static void TraceMethodState(TraceEventType enumTraceEventType, string strMethod = null)
        {
            if (string.IsNullOrEmpty(strMethod))
            {
                strMethod = new System.Diagnostics.StackTrace(1, false).GetFrame(1).GetMethod().Name;
            }
            Logger.Trace(enumTraceEventType, string.Format("{0} {1}.", strMethod, enumTraceEventType.GetName()));
        }

        /// <summary>
        /// Obtiene y devuelve el nombre del metodo y hace trace de inicio con el nombre del metodo.
        /// </summary>
        /// <param name="skipFrames"></param>
        /// <returns></returns>
        public static string TraceStartMethod(int skipFrames = 1)
        {
            string strMethod = new System.Diagnostics.StackTrace(skipFrames, false).GetFrame(0).GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
            Logger.TraceStart(strMethod);
            return strMethod;
        }

        /// <summary>
        /// Close Trace.
        /// </summary>
        public static void TraceClose()
        {
            trace.Flush();
            trace.Close();
        }


    }//End class
}//End Namespace	

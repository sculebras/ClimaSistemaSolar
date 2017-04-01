//using log4net;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;

namespace SF.Logger.Handlers
{
    public class GlobalExceptionLogger : ExceptionLogger
    {

        public override void Log(ExceptionLoggerContext context)
        {
            Logger.TraceErrorComplete(context.Exception, context.Request);
        }

        public override bool ShouldLog(ExceptionLoggerContext context)
        {
            return true; //To solve CORS issue
            //return base.ShouldLog(context);
        }


        
    }
}
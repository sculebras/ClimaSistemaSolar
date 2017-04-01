using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace SF.Logger.Filters
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.TraceErrorComplete(context.Exception, context.Request);

            base.OnException(context);
            //if (context.Exception is NotImplementedException)
            //{
            //    context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            //}
        }

        
    }
}

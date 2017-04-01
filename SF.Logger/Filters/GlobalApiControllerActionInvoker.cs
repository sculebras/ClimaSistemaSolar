using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace SF.Logger.Filters
{
    public class GlobalApiControllerActionInvoker : ApiControllerActionInvoker
    {
        public override Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var result = base.InvokeActionAsync(actionContext, cancellationToken);

            if (result.Exception != null /*&& result.Exception.GetBaseException() != null*/)
            {
                Logger.TraceErrorComplete(result.Exception, actionContext.Request);
                //Exception baseException = result.Exception.GetBaseException();

                //if (baseException is BusinessException)
                //{
                //    return Task.Run<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                //    {
                //        Content = new StringContent(baseException.Message),
                //        ReasonPhrase = "Error"

                //    });
                //}
                //else
                //{
                //    //Log critical error
                //    Debug.WriteLine(baseException);

                //    return Task.Run<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)
                //    {
                //        Content = new StringContent(baseException.Message),
                //        ReasonPhrase = "Critical Error"
                //    });
                //}
            }

            return result;
        }
    }
}

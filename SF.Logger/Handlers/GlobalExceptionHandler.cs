using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace SF.Logger.Handlers
{
    //NO SE ESTA USANDO (Al implementar ejecuta 2 veces GlobalExceptionLogger.Log
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private class ErrorInformation
        {
            public string Message { get; set; }
            public DateTime ErrorDate { get; set; }
        }
        public override void Handle(ExceptionHandlerContext context)
        {
            //Exception ex = context.Exception;
            //var log = log4net.LogManager.GetLogger(typeof(Startup).FullName);
            //log.Error(string.Format("{0} {1}\nMensaje: \nStackTrace:\n{2}\n\n",
            //                        ex.GetType().Name, ex.Message, ex.StackTrace));

            base.Handle(context);
            //Return a DTO representing what happened
            //context.Result = new ResponseMessageResult(context.Request.CreateResponse(HttpStatusCode.InternalServerError,
            //new ErrorInformation { Message = "We apologize but an unexpected error occured. Please try again later.", ErrorDate = DateTime.UtcNow }));

            //This is commented out, but could also serve the purpose if you wanted to only return some text directly, rather than JSON that the front end will bind to.
            //context.Result = new ResponseMessageResult(context.Request.CreateResponse(HttpStatusCode.InternalServerError, "We apologize but an unexpected error occured. Please try again later."));
        }

        public override async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            await base.HandleAsync(context, cancellationToken);
            //ExceptionDispatchInfo info = ExceptionDispatchInfo.Capture(context.Exception);
            //info.Throw();

        }


        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true; //To solve CORS issue
            //return base.ShouldHandle(context);
        }
    }
}
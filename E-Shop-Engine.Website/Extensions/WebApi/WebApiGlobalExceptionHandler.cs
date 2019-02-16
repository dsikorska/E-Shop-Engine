using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace E_Shop_Engine.Website.Extensions.WebApi
{
    public class WebApiGlobalExceptionHandler : ExceptionHandler
    {
        public override async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = context.Exception.Message
                });

            response.Headers.Add("X-Error", context.Exception.Message);
            context.Result = new ResponseMessageResult(response);
        }
    }
}
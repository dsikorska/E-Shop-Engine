using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using NLog;

namespace E_Shop_Engine.Website.CustomFilters
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Log(LogLevel.Error, actionExecutedContext.Exception, actionExecutedContext.Exception.Message);

            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
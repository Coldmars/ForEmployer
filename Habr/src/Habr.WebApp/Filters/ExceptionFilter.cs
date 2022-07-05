using Habr.Common.Exceptions;
using Habr.Common.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Habr.WebApp.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var response = GetResponse(context.Exception);

            context.HttpContext.Response.StatusCode = response.StatusCode;
            context.Result = new JsonResult(response);
            Log(response);

            base.OnException(context);
        }

        private  Response GetResponse(Exception ex) =>
            ex switch
            {
                NotFoundException => CreateClientErrorResponse(ex.Message, HttpStatusCode.NotFound),
                ValidationException => CreateClientErrorResponse(ex.Message, HttpStatusCode.BadRequest),
                UnauthorizedException => CreateClientErrorResponse(ex.Message, HttpStatusCode.Unauthorized),
                ForbiddenException => CreateClientErrorResponse(ex.Message, HttpStatusCode.Forbidden),
                BusinessLogicException => CreateClientErrorResponse(ex.Message, HttpStatusCode.NotAcceptable),
 
                _ => CreateServerErrorResponse(ex.Message, ex.StackTrace, HttpStatusCode.InternalServerError)
            };

        private ClientErrorResponse CreateClientErrorResponse(string message, HttpStatusCode code) =>
             new ClientErrorResponse
             {
                 StatusCode = (int)code,
                 Message = message,
             };

        private ServerErrorResponse CreateServerErrorResponse(string message, 
                                                              string stackTrace, 
                                                              HttpStatusCode code) =>
             new ServerErrorResponse
             {
                 StatusCode = (int)code,
                 Message = message,
                 StackTrace = stackTrace
             };

        private void Log(Response response)
        {
            if (response.StatusCode == 500)
                _logger.LogError($"{response.StatusCode}, {response.Message}");
            else
                _logger.LogWarning($"{response.StatusCode}, {response.Message}");
        }
    }
}

using Common.Exceptions;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using ILogger = Common.Services.Logger.ILogger;


namespace Common.Filters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ServerExceptionTypeFilterAttribute : TypeFilterAttribute
    {
        public ServerExceptionTypeFilterAttribute() : base(typeof(ServerExceptionFilter)) { }

        private class ServerExceptionFilter : ExceptionFilterAttribute
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILogger _logger;

            public ServerExceptionFilter(IHttpContextAccessor httpContextAccessor, ILogger logger)
            {
                _httpContextAccessor = httpContextAccessor;
                _logger = logger;
            }

            public override void OnException(ExceptionContext context)
            {
                if (context.Exception is BadRequestException)
                {
                    _logger.Log(GetType(), Level.Error, "Bad request error", context.Exception);

                    context.Result = new ObjectResult(new { Message = "Internal Server Error" })
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest
                    };
                }
                else if (context.Exception is UnauthorizedException)
                {
                    context.Result = new ObjectResult(new { Message = "No authorization" })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                }
                else
                {
                    _logger.Log(GetType(), Level.Error, "Server error", context.Exception);

                    context.Result = new ObjectResult(new { Message = "Internal Server Error" })
                    {
                        StatusCode = context.Exception is not Exception ? (int)HttpStatusCode.InternalServerError : (int)HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}

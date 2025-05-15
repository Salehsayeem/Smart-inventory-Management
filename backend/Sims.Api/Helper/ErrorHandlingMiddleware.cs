using System.Net;
using Sims.Api.Dto;

namespace Sims.Api.Helper
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new CommonResponseDto();

            switch (exception)
            {
                case { } ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        response.Message = ex.Message;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = ex.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = exception.Message;
                    break;
            }
            _logger.LogError(exception.Message);
            await context.Response.WriteAsync(new CommonResponseDto()
            {
                StatusCode = response.StatusCode,
                Message = response.Message,
            }.ToString());
        }
    }
}

using System.Net;
using Newtonsoft.Json;


namespace MyWebApi.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occured");
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new
            {
                error = "An internal server error has occurred.",
                details = exception.Message
            });

            return context.Response.WriteAsync(result);
        }
    }

}
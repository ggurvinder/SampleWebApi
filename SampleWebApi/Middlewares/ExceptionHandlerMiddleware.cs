using System.Net;

namespace SampleWebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,RequestDelegate next)
        {
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // log the exception
                var errorId=Guid.NewGuid().ToString();
                _logger.LogError(ex,$"{errorId} : {ex.Message}");

                //return 
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var error=new { 
                Id=errorId,
                ErrorMessage="Something went wrong! we are looking into resolving this."
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }


    }
}

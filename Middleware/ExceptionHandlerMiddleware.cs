namespace AssessmentAPI.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string message;

            switch (exception)
            {
                case ArgumentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message; 
                    break;

                case HttpRequestException:
                    statusCode = StatusCodes.Status503ServiceUnavailable;
                    message = "Failed to communicate with an external service. Please try again later.";
                    break;

                case InvalidOperationException:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = exception.Message; 
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";


            var errorResponse = new
            {
                StatusCode = statusCode,
                Message = message
            };

            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}

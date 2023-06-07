using System.Text;

namespace Claims.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);

            await _next.Invoke(context);

            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            _logger.LogInformation("HTTP REQUEST {method} {path}", context.Request.Method.ToUpper(), context.Request.Path);
        }

        private async Task LogResponse(HttpContext context)
        {
            _logger.LogInformation("HTTP RESPONSE with status code: {statusCode}", context.Response.StatusCode);
        }
    }
}

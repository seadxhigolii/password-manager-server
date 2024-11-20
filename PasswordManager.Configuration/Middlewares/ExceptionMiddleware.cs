using Microsoft.AspNetCore.Http;
using System.Net;
using PasswordManager.Persistence.Contexts;

using pmc = PasswordManager.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace PasswordManager.Configuration.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public ExceptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
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
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PasswordManagerDbContext>();

                var methodName = GetMethodNameFromException(exception);

                var logEntry = new pmc.Log
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.UtcNow,
                    Level = "Error",
                    Message = exception.Message,
                    Exception = exception.ToString(),
                    Properties = null,
                    LogEvent = methodName
                };

                await dbContext.Logs.AddAsync(logEntry);
                await dbContext.SaveChangesAsync();
            }

            var result = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Details = exception.InnerException?.ToString() ?? string.Empty,
            };

            await context.Response.WriteAsJsonAsync(result);
        }

        private string GetMethodNameFromException(Exception exception)
        {
            try
            {
                var stackTrace = new System.Diagnostics.StackTrace(exception, true);
                var frame = stackTrace.GetFrame(0);
                var method = frame?.GetMethod();
                return method?.DeclaringType?.FullName + "." + method?.Name ?? "UnknownMethod";
            catch
            {
                return "UnknownMethod";
            }
        }

    }
}

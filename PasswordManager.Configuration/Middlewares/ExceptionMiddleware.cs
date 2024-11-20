using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Security;

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
                Log.Error(ex, "An unhandled exception occurred.");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var (statusCode, message) = GetExceptionDetails(exception);

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Details = exception.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static (int StatusCode, string Message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "You are not authorized to access this resource."),

                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "The requested resource was not found."),
                FileNotFoundException => ((int)HttpStatusCode.NotFound, "The requested file was not found."),
                DirectoryNotFoundException => ((int)HttpStatusCode.NotFound, "The requested directory was not found."),

                InvalidOperationException => ((int)HttpStatusCode.BadRequest, "The request could not be processed."),
                ArgumentException => ((int)HttpStatusCode.BadRequest, "One or more arguments provided are invalid."),

                TimeoutException => ((int)HttpStatusCode.RequestTimeout, "The request timed out. Please try again later."),

                NotSupportedException => ((int)HttpStatusCode.MethodNotAllowed, "This operation is not supported."),

                AccessViolationException => ((int)HttpStatusCode.Forbidden, "Access to the requested resource is forbidden."),
                SecurityException => ((int)HttpStatusCode.Forbidden, "A security error occurred while processing the request."),

                InvalidCastException => ((int)HttpStatusCode.UnprocessableEntity, "The server could not process the request due to invalid data."),
                FormatException => ((int)HttpStatusCode.UnprocessableEntity, "The data format is invalid."),
                DivideByZeroException => ((int)HttpStatusCode.UnprocessableEntity, "A division by zero error occurred."),

                NotImplementedException => ((int)HttpStatusCode.NotImplemented, "The requested functionality is not implemented."),

                HttpRequestException => ((int)HttpStatusCode.BadGateway, "A network error occurred while processing the request."),

                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.")
            };
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using WordCounter.Models.Error;

namespace WordCounter.Middlewares
{
    /// <summary>
    /// Error handler middleware to catch all errors and do not push anything to user
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                // ignored by design as request cancellation is not an error
            }
            catch (Exception ex)
            {
                Guid responseGuid = Guid.NewGuid();
                logger.LogError(ex, responseGuid.ToString());

                await HandleException(responseGuid).ExecuteResultAsync(new ActionContext(
                    context,
                    new RouteData(),
                    new ControllerActionDescriptor()));
            }
        }

        private IActionResult HandleException(Guid newGuid)
        {
            ObjectResult result = new ObjectResult(
                new ErrorInformation()
                {
                    Guid = Guid.NewGuid(),
                    Message = "Server error",
                });
            result.StatusCode = StatusCodes.Status500InternalServerError;

            return result;
        }
    }
}

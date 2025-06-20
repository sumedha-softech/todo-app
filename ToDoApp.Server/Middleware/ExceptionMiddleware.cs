using ToDoApp.Server.Models;

namespace ToDoApp.Server.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ResponseModel { Data = null, IsSuccess = false, Message = "Something went wrong, please try again later!!" };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}


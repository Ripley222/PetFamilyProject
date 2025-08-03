using PetFamily.API.Response;
using PetFamily.Domain.Shared;

namespace PetFamily.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        { 
            await next(context); //вызов следующего middleware
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            
            var responseError = Error.Failure("server.internal", ex.Message);
            var envelope = Envelope.Error(responseError);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}
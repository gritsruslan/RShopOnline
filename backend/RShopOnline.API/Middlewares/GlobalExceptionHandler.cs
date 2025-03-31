namespace RShopAPI_Test.Middlewares;

public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsync($"Unhandled error occured! {ex}");
        }
    }
}
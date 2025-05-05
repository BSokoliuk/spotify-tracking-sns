namespace Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

  public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new { Success = false, ex.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}